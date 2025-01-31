using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ParkNow.Data;
using ParkNow.Models;

namespace ParkNow.Background;

// Updates Carpark Lot Status
public class CarparkUpdater : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<CarparkUpdater> _logger;

    public CarparkUpdater(IServiceScopeFactory serviceScopeFactory, ILogger<CarparkUpdater> logger)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                try
                {
                    await UpdateAvailability(context);
                }
                catch (System.Exception ex)
                {
                    
                    _logger.LogInformation(ex.Message);
                }
                // Every 5 min call
                await Task.Delay(300000, stoppingToken);
            }
        }
    }
    private async Task UpdateAvailability(AppDbContext context) {
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://api.data.gov.sg/v1/transport/carpark-availability"),
        };
        using (var response = await client.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            using (JsonDocument document = JsonDocument.Parse(body))
                {
                    var avail = document.RootElement.GetProperty("items")[0].GetProperty("carpark_data").EnumerateArray();
                    List<Carpark> all_carparks = await context.Carparks.ToListAsync();
                    foreach (var cp in avail) {
                        // Extract carpark ID and availability from the JSON
                        var carparkId = cp.GetProperty("carpark_number").GetString();

                        int availableLots = 0;
                        int totalLots = 0;
                        foreach (var lot in cp.GetProperty("carpark_info").EnumerateArray()) {
                            availableLots += Convert.ToInt32(lot.GetProperty("lots_available").ToString());
                            totalLots += Convert.ToInt32(lot.GetProperty("total_lots").ToString());
                        }
                        // Find matching carpark
                        var carpark = all_carparks.FirstOrDefault(c => c.CarparkId == carparkId);
                        if (carpark != null) {
                            // Update 
                            carpark.LotsAvailable = availableLots;
                            carpark.TotalLots = totalLots;
                            //_logger.LogInformation("Saving Carpark {a}, {b}/{c}", carpark.CarparkId, availableLots, totalLots);
                            await context.SaveChangesAsync();
                        }
                    }
                }
        }
    }
}

