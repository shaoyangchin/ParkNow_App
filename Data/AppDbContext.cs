using Microsoft.EntityFrameworkCore;
using ParkNow.Models;

namespace ParkNow.Data;
public class AppDbContext : DbContext
{
    protected readonly IConfiguration Configuration;
    public AppDbContext(IConfiguration configuration){
        Configuration = configuration;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
    }
    public DbSet<User> Users {get; set;}
    public DbSet<Booking> Bookings {get; set;}
    public DbSet<Carpark> Carparks {get; set;}
    public DbSet<Payment> Payments {get; set;}
    public DbSet<Vehicle> Vehicles {get; set;}
    public DbSet<Voucher> Vouchers {get; set;}

}
