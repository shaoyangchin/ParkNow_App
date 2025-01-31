using MudBlazor.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using ParkNow.Components;
using ParkNow.Data;
using ParkNow.Services;
using ParkNow.Components.Account;
using ParkNow.Background;



var builder = WebApplication.CreateBuilder(args);

// Configure Logging
builder.Logging.ClearProviders();
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add dbcontext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add cascading auth
builder.Services.AddCascadingAuthenticationState();

// Add authentication
builder.Services.AddAuthentication("parknow-auth")
    .AddCookie("parknow-auth", options => 
    {
        options.Cookie.Name = "auth_token";
        options.LoginPath = "/Account/login";
        options.LogoutPath = "/Account/logout";
        options.AccessDeniedPath = "/access-denied";
        
        options.Cookie.MaxAge = TimeSpan.FromMinutes(60);
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

    });

// Add http client
builder.Services.AddHttpClient();

// Scoped Services 
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICarparkService, CarparkService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IVoucherService, VoucherService>();

// Hosted Background Services
builder.Services.AddHostedService<CarparkUpdater>();

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.HideTransitionDuration = 100;
});

builder.Services.AddRazorPages();
var app = builder.Build();

// Auto Migrate + Do any DB Startup tasks
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
