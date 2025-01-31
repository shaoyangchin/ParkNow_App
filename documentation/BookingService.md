# BookingService

> See complete implementation at `ParkNow.Services.BookingService`

The `BookingService` class provides methods to manage booking operations within the ParkNow system. It includes CRUD operations for bookings as well as additional utility functions like price calculation.

## CRUD Operations

### CreateBooking

#### Without Voucher
```csharp
Task<bool> CreateBooking(Booking booking)
```
Creates a new booking in the system.

**Parameters:**
- `booking` (Booking): The booking entity to be created

**Returns:**
- `Task<bool>`: True if creation is successful, false otherwise

#### With Voucher
```csharp
Task<bool> CreateBooking(Booking booking, Voucher voucher)
```
Creates a new booking with an applied voucher discount.

**Parameters:**
- `booking` (Booking): The booking entity to be created
- `voucher` (Voucher): The voucher to be applied to the booking

**Returns:**
- `Task<bool>`: True if creation is successful, false otherwise

### GetUserBookings
```csharp
Task<List<Booking>> GetUserBookings(string username)
```
Retrieves all active (non-deleted) bookings for a specific user.

**Parameters:**
- `username` (string): The username to search bookings for

**Returns:**
- `Task<List<Booking>>`: List of bookings associated with the user, or empty list if none found

### GetBooking
```csharp
Task<Booking> GetBooking(int bookingId)
```
Retrieves a specific booking by its ID.

**Parameters:**
- `bookingId` (int): The ID of the booking to retrieve

**Returns:**
- `Task<Booking>`: The requested booking entity, or null if not found

### UpdateBooking
```csharp
Task<bool> UpdateBooking(Booking booking)
```
Updates an existing booking's information (start time, end time, cost, and status).

**Parameters:**
- `booking` (Booking): The booking entity with updated information

**Returns:**
- `Task<bool>`: True if update is successful, false otherwise

### DeleteBooking
```csharp
Task<bool> DeleteBooking(int bookingId)
```
Marks a booking as deleted in the system (soft delete).

**Parameters:**
- `bookingId` (int): The ID of the booking to mark as deleted

**Returns:**
- `Task<bool>`: True if deletion is successful, false otherwise

## Utility Functions

### CalculatePrice
```csharp
decimal CalculatePrice(DateTime start, DateTime end, Carpark carpark)
```
Calculates the parking fee for a specific time range at a given carpark.

**Parameters:**
- `start` (DateTime): Start time of the parking period
- `end` (DateTime): End time of the parking period
- `carpark` (Carpark): The carpark entity to calculate price for

**Returns:**
- `decimal`: Calculated price for the parking duration, or 0 if parking is not available for the specified time range
