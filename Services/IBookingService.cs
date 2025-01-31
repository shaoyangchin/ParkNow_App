using ParkNow.Models;
namespace ParkNow.Services;
/*
This interface implements the methods of the Booking entity
CreateBooking, GetUserBookings, GetBooking, UpdateBooking, Delete Booking, CalculatePrice

This entity has CRUD operations
*/
public interface IBookingService
{
    // CRUD
    /*
    Create booking without voucher
    Returns true on successful creation, false if not
    */ 
    Task<bool> CreateBooking(Booking booking);
    /*
    Overloaded Create booking when voucher is used
    Returns true on successful creation, false if not
    */ 
    Task<bool> CreateBooking(Booking booking, Voucher voucher);
    /*
    Return all bookings associated with a user that are not marked deleted
    Returns a list of Booking entities or an empty list if not found
    */ 
    Task<List<Booking>> GetUserBookings(string username);
    /*
    Get a specific booking by BookingId
    Returns the found Booking entity or null if not found
    */ 
    Task<Booking> GetBooking(int bookingId);
    /*
    Update booking information, updates starttime, endtime, cost and status
    Returns true if Booking entity successfully updated or false if not
    */ 
    Task<bool> UpdateBooking(Booking booking);
    /*
    Sets a booking to deleted (DOES NOT DELETE FROM DATABASE)
    Returns true if deleted attributed is successfully set or false if not
    */ 
    Task<bool> DeleteBooking(int bookingId);

    // Non CRUD
    /*
    Calculates price for a specific time range and carpark
    Returns a decimal if a price can be calculated or null if the carpark does not allow parking in the time range
    */
    decimal CalculatePrice(DateTime start, DateTime end, Carpark carpark);

    
}
