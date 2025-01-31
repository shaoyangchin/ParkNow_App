using Microsoft.EntityFrameworkCore;
using ParkNow.Data;
using ParkNow.Models;
namespace ParkNow.Services;

public class CarparkService : ICarparkService
{
    // Get Database Context and Constructor to include context
    private readonly ILogger<CarparkService> _logger;
    private readonly AppDbContext _context;
    public CarparkService(AppDbContext context, ILogger<CarparkService> logger) {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Carpark>> GetAllCarparks() {
        return await _context.Carparks.Where(c => c.ShortTermParkingType != "NO").ToListAsync();
    }

    public async Task<Carpark> GetCarpark(string carparkId) {
        return await _context.Carparks.Where(c => c.CarparkId == carparkId).FirstOrDefaultAsync();
    }
}
