# Carpark Model

> See complete implementation at `ParkNow.Models.Carpark`

The `Carpark` class represents a carpark facility in the ParkNow system. It combines static data from the government dataset and real-time availability data from the carpark availability API.

## Data Sources
- Static data: [Government Dataset](https://data.gov.sg/datasets/d_23f946fa557947f93a8043bbef41dd09/view)
- Real-time data: [Carpark Availability API](https://api.data.gov.sg/v1/transport/carpark-availability)

## Properties

### Identification
```csharp
public required string CarparkId { get; set; }
```
- Primary key
- Unique identifier for the carpark
- Matches government dataset ID

### Location Information
```csharp
public required string Address { get; set; }
```
- Physical address of the carpark

```csharp
public required string XCord { get; set; }
public required string YCord { get; set; }
```
- Geographical coordinates of the carpark
- Stored as strings to maintain original format from dataset

```csharp
[NotMapped]
public double Distance { get; set; }
```
- Calculated distance from a reference point
- Not stored in database
- Used for distance-based queries and sorting

### Carpark Characteristics
```csharp
public required string Type { get; set; }
```
- Type of carpark facility

```csharp
public required string SystemType { get; set; }
```
- Payment/management system used

```csharp
public required string ShortTermParkingType { get; set; }
```
- Type of short-term parking available

```csharp
public required string FreeParking { get; set; }
```
- Free parking availability information

```csharp
public required bool NightParking { get; set; }
```
- Indicates if night parking is allowed

```csharp
public required bool CentralCharge { get; set; }
```
- Indicates if central charging system is used

```csharp
public required double GantryHeight { get; set; }
```
- Height of the entrance gantry
- Used for vehicle height restrictions

```csharp
public required bool CarparkBasement { get; set; }
```
- Indicates if carpark has basement levels

### Real-time Availability
```csharp
public int? TotalLots { get; set; }
public int? LotsAvailable { get; set; }
```
- Updated by background service from API
- Nullable as data may not always be available
- Represents current parking capacity and availability

## Data Updates
- Static properties are populated from the government dataset
- `TotalLots` and `LotsAvailable` are updated periodically by a background service
- Real-time updates are fetched from the carpark availability API

## Notes
- Most properties are required and non-nullable
- Availability data (TotalLots, LotsAvailable) is nullable as it depends on API updates
- Distance property is used for calculations but not stored in database
- Coordinates are stored as strings to maintain compatibility with government dataset format