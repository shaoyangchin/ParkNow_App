# Other Important Methods

There are a few important methods that do not fit into the entities but are part of our business logic and it makes more sense
to place them on the Razor Components itself

## DynamicCarpark

> See complete implementations at `ParkNow.Pages.DynamicCarpark`

### Booking Validation
```csharp
private bool ValidateBooking()
```
Validates each booking with a few checks.

**Validation Checks:**
- Vehicle must be selected
- Start datetime must be present or in the future
- End datetime must be present and after Start
- Booking durati0on cannot be more than 24 hours and cannot book more than 7 days in advance
- Price returned by price calculation must be greater than 0, less than 0 means invalid booking

**Returns:**
- `bool`: True if validation is successful, false otherwise

### Payment Validation
```csharp
private bool ValidatePayment()
```
Validates each payment with a few checks.

**Validation Checks:**
- Credit Card Number presence
- CSV (Security Code) presence
- Expiry date presence and validity
- Card expiration status

**Returns:**
- `bool`: True if validation is successful, false otherwise

## Map

> See complete implementations at `ParkNow.Pages.Map`

### Distance Calculation
```csharp
private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
```
Used to calculate distance between geographical points, user location and carparks, as we only load carparks within 2km radius.

**Parameters:**
- `lat1` (double): Latitude of the first point in decimal degrees
- `lon1` (double): Longitude of the first point in decimal degrees
- `lat2` (double): Latitude of the second point in decimal degrees
- `lon2` (double): Longitude of the second point in decimal degrees

**Returns:**
- `double`: Distance between the points in meters

#### Notes
- Uses Haversine formula for great-circle distance calculation
- Earth's radius (R) is assumed to be 6371 kilometers
- Result is converted to meters (multiplied by 1000)
- Coordinates should be in decimal degrees
- Accounts for Earth's spherical shape

## ToRad
```csharp
private double ToRad(double degrees)
```
Converts angle measurements from degrees to radians.

**Parameters**
- `degrees` (double): Angle measurement in degrees

**Returns**
- `double`: Angle measurement in radians

#### Notes
- Helper method for CalculateDistance
- Uses formula: radians = degrees * π/180
- Used for converting coordinate values for trigonometric calculations
