/*
This class implements the Voucher entity with the attributes
VoucherId, UserId, Amount, Issue, Expiry
*/
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace ParkNow.Models;
public class Voucher
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public required string VoucherId {get; set;}
    
    public string? Username { get; set; } 

    [ForeignKey(nameof(Username))]
    public User? User {get; set;}

    [Precision(18, 2)]
    public decimal Amount {get; set;}
    public DateTime Issue {get; set;}
    public DateTime Expiry {get; set;}
    public required bool Deleted {get; set;}
}
