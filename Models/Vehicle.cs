/*
This class implements the Vehicle entity with the attributes
VehicleId, UserId, LicensePlate, Model, CarType
*/
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ParkNow.Models;
public class Vehicle {
    public enum CarTypes
    {
        Private,
        Commercial,
        SpecialPurpose,
        Electric,
        PublicService
    }
    public int VehicleId {get; set;}
    public string Username { get; set; }
    [ForeignKey(nameof(Username))]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public required User User {get; set;}
    public required bool Deleted {get; set;}
    public required string LicensePlate {get; set;}
    public string? Model {get; set;}
    public required CarTypes CarType {get; set;}
}