/*
This class implements the Payment entity with the attributes
PaymentId, BookingId, UserId, Amount, VoucherId, Discount,
Timestamp, Status
*/
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace ParkNow.Models;
public class Payment {
    public enum Statuses
    {
        Success,
        Failed,
        Refunded,
        Processing,
    }
    public int PaymentId {get; set;}
    
    [ForeignKey("BookingId")]
    public required Booking Booking {get; set;}

    [Precision(18, 2)]
    public required decimal Amount {get; set;}

    [ForeignKey("VoucherId")]
    public Voucher? Voucher {get; set;}
    [Precision(18, 2)]
    public decimal? Discount {get; set;}
    public required DateTime Timestamp {get; set;}
    public required Statuses Status {get; set;}
}