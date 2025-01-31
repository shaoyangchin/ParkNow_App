using ParkNow.Models;

namespace ParkNow.Services;
/*
This interface implements the methods of the Carpark entity
GetAllCarparks, GetCarpark

This entity has only READ operations
*/
public interface ICarparkService
{
    // CRUD
    /*
    Returns all carparks other than those with ShortTermParking = NO
    Returns a list of carparks, or an empty list if there are no carparks
    */ 
    Task<List<Carpark>> GetAllCarparks();
    /*
    Gets a specific carpark by CarparkId
    Returns the found carpark entity or null if not found
    */ 
    Task<Carpark> GetCarpark(string carparkId);
}
