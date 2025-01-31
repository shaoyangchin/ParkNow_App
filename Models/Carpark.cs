/*
This class implements the Carpark entity using information from the gov dataset:
https://data.gov.sg/datasets/d_23f946fa557947f93a8043bbef41dd09/view
with the attributes
CarparkId, Address, XCord, YCord, Type, SystemType,
ShortTermParkingType, FreeParking, NightParking, 
CentralCharge, GantryHeight, CarparkBasement
The following additional attributes are updated via a Background Service using
information from the gov api:
https://api.data.gov.sg/v1/transport/carpark-availability 
*/
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkNow.Models;
public class Carpark {
    public required string CarparkId {get; set;}
    public required string Address {get; set;}
    public required string XCord {get; set;}
    public required string YCord {get; set;}
    public required string Type {get; set;}
    public required string SystemType {get; set;}
    public required string ShortTermParkingType {get; set;}
    public required string FreeParking {get; set;}
    public required bool NightParking {get; set;}
    public required bool CentralCharge {get; set;}
    
    // To be filled in on api call
    public int? TotalLots {get; set;}
    public int? LotsAvailable {get; set;}
    
    // Bottom Might Not Need
    public required double GantryHeight {get; set;}
    public required bool CarparkBasement {get; set;}

    [NotMapped]
    public double Distance { get; set; }
}