/*
This class implements the Booking entity with the attributes
BookingId, PaymentId, UserId, VehicleId, CarparkId,
StartTime, EndTime, BookingTime, Cost, Status
*/
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace ParkNow.Models;
public class Booking {
    public enum Statuses
    {
        Active,
        Completed,
        Cancelled,
        Scheduled,
    }
    public int BookingId {get; set;}
    public Payment? Payment {get; set;}
    public string Username { get; set; } 

    [ForeignKey(nameof(Username))]
    public required User User {get; set;}

    [ForeignKey("VehicleId")]
    public required Vehicle Vehicle {get; set;}

    [ForeignKey("CarparkId")]
    public required Carpark Carpark {get; set;}
    public required DateTime? StartTime {get; set;}
    public required DateTime? EndTime {get; set;}
    public required DateTime BookingTime {get; set;}
    
    [Precision(18, 2)]
    public required decimal Cost {get; set;}
    public required Statuses Status {get; set;}
}