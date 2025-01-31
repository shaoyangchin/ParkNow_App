using ParkNow.Models;

namespace ParkNow.Services;
/*
This interface implements the methods of the Vehicle entity
CreateVehicle, GetVehicle, GetUserVehicles, UpdateVehicle, DeleteVehicle

This entity has CRUD operations
*/
public interface IVehicleService
{
    // CRUD
    /*
    Creates a new vehicle associated with the user
    Returns true on successful creation, false if not
    */ 
    Task<bool> CreateVehicle(Vehicle vehicle);
    /*
    Get a specific vehicle by licenseplate
    Returns the found Vehicle entity or null if not found
    */ 
    Task<Vehicle> GetVehicle(string licenseplate);
    /*
    Return all vehicles associated with a user that are not marked deleted
    Returns a list of Vehicle entities or an empty list if not found
    */ 
    Task<List<Vehicle>> GetUserVehicles(string username);
    /*
    Update vehicle information, updates licenseplate, model, cartype
    Returns true if Vehicle entity successfully updated or false if not
    */ 
    Task<bool> UpdateVehicle(Vehicle vehicle);
    /*
    Sets a vehicle to deleted (DOES NOT DELETE FROM DATABASE)
    Returns true if deleted attributed is successfully set or false if not
    */ 
    Task<bool> DeleteVehicle(int vehicleId);
    
}
