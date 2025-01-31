# VehicleService

> See complete implementation at `ParkNow.Services.VehicleService`

The `VehicleService` interface provides methods to manage vehicle operations within the ParkNow system. It handles creation, retrieval, updating, and soft deletion of vehicle records.

## CRUD Operations

### CreateVehicle
```csharp
Task<bool> CreateVehicle(Vehicle vehicle)
```
Creates a new vehicle record associated with a user.

**Parameters:**
- `vehicle` (Vehicle): The vehicle entity to be created

**Returns:**
- `Task<bool>`: True if creation is successful, false otherwise

### GetVehicle
```csharp
Task<Vehicle> GetVehicle(string licenseplate)
```
Retrieves a specific vehicle by its license plate number.

**Parameters:**
- `licenseplate` (string): The license plate number to search for

**Returns:**
- `Task<Vehicle>`: The requested vehicle entity, or null if not found

### GetUserVehicles
```csharp
Task<List<Vehicle>> GetUserVehicles(string username)
```
Retrieves all active (non-deleted) vehicles associated with a specific user.

**Parameters:**
- `username` (string): The username to get vehicles for

**Returns:**
- `Task<List<Vehicle>>`: List of vehicles belonging to the user, or empty list if none found

### UpdateVehicle
```csharp
Task<bool> UpdateVehicle(Vehicle vehicle)
```
Updates an existing vehicle's information (license plate, model, car type).

**Parameters:**
- `vehicle` (Vehicle): The vehicle entity with updated information

**Returns:**
- `Task<bool>`: True if update is successful, false otherwise

### DeleteVehicle
```csharp
Task<bool> DeleteVehicle(int vehicleId)
```
Marks a vehicle as deleted in the system (soft delete).

**Parameters:**
- `vehicleId` (int): The ID of the vehicle to mark as deleted

**Returns:**
- `Task<bool>`: True if deletion is successful, false otherwise

## Notes
- All operations are asynchronous and return Task objects
- Vehicle deletion is implemented as a soft delete (marked as deleted but not removed from database)
- GetUserVehicles only returns vehicles that haven't been marked as deleted
- License plates are used as unique identifiers for vehicle lookup
- Updates to vehicles affect license plate, model, and car type attributes
