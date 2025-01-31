# CarparkService

> See complete implementation at `ParkNow.Services.CarparkService`
> 
The `CarparkService` class provides read-only methods to access carpark information within the ParkNow system. This service specifically handles retrieval operations for carpark data.

## CRUD Operations

### GetAllCarparks
```csharp
Task<List<Carpark>> GetAllCarparks()
```
Retrieves all available carparks that allow short-term parking.

**Returns:**
- `Task<List<Carpark>>`: List of all carparks with ShortTermParking enabled
- Returns an empty list if no carparks are found

### GetCarpark
```csharp
Task<Carpark> GetCarpark(string carparkId)
```
Retrieves a specific carpark by its identifier.

**Parameters:**
- `carparkId` (string): The unique identifier of the carpark to retrieve

**Returns:**
- `Task<Carpark>`: The requested carpark entity
- Returns null if no carpark is found with the specified ID

## Notes
- This service is read-only and does not provide methods for creating, updating, or deleting carparks
- The GetAllCarparks method specifically excludes carparks where ShortTermParking is set to NO
- All operations are asynchronous and return Task objects
