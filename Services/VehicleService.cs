using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using ParkNow.Data;
using ParkNow.Models;
namespace ParkNow.Services;

public class VehicleService : IVehicleService
{
    // Get Database Context and Constructor to include context
    private readonly ILogger<VehicleService> _logger;
    private readonly AppDbContext _context;

    public VehicleService(AppDbContext context, ILogger<VehicleService> logger) {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Vehicle>> GetUserVehicles(string username) {
        return await _context.Vehicles.Where(v => v.User.Username == username && v.Deleted != true).ToListAsync();
    }

    public async Task<Vehicle> GetVehicle(string licenseplate) {
        return await _context.Vehicles.Where(v => v.LicensePlate == licenseplate).FirstOrDefaultAsync();
    }

    public async Task<bool> CreateVehicle(Vehicle vehicle) {
       try {
            // Add vehicle
            await _context.Vehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();
            return true;
       }
       catch {
            return false;
       }
    }

    public async Task<bool> UpdateVehicle(Vehicle vehicle) {
       try {
            // Add vehicle
            Vehicle? db_Vehicle = await _context.Vehicles.Where(v => v.VehicleId == vehicle.VehicleId).FirstOrDefaultAsync();
            if (db_Vehicle == null) {
                return false;
            }
            db_Vehicle.LicensePlate = vehicle.LicensePlate;
            db_Vehicle.Model = vehicle.Model;
            db_Vehicle.CarType = vehicle.CarType;
            await _context.SaveChangesAsync();
       }
       catch {
            return false;
       }
        return true;
    }

    public async Task<bool> DeleteVehicle(int vehicleId) {
        int vehicleBookings = await _context.Bookings.Where(b => b.Vehicle.VehicleId == vehicleId && b.Status != Booking.Statuses.Completed).CountAsync();
        if (vehicleBookings >= 1) {
            return false;
        }
        try {
            Vehicle? db_Vehicle = await _context.Vehicles.Where(v => v.VehicleId == vehicleId).FirstOrDefaultAsync();
            if (db_Vehicle == null) {
                return false;
            }
            db_Vehicle.Deleted = true;
            await _context.SaveChangesAsync();
       }
       catch {
            return false;
       }
        return true;
    }
}
