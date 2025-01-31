# User Model

> See complete implementation at `ParkNow.Models.User`

The `User` class represents a user account in the ParkNow system. It contains essential user information and authentication details.

## Properties

### Primary Key
```csharp
[Key]
public required string Username { get; set; }
```
- Primary key for user identification
- Uses username as natural key instead of auto-generated ID
- Required and must be unique
- Annotated with `[Key]` attribute

### Authentication Properties
```csharp
public required string Password { get; set; }
```
- Stores the hashed password
- Required field
- Should never store plain text passwords
- Hashing handled by IUserService

```csharp
public required string Email { get; set; }
```
- User's email address
- Required field
- Used for communications and recovery

### Role Management
```csharp
public enum Roles
{
    User,   // Regular user with standard privileges
    Admin   // Administrator with elevated privileges
}

public required Roles Role { get; set; }
```
- Defines user access level
- Required field
- Two possible roles:
  - `User`: Standard user privileges
  - `Admin`: Administrative privileges

## Access Control
- Role-based access control implemented through the Roles enum
- Admin users have additional privileges over regular users
- Role is required and must be explicitly set during user creation

## Notes
- All properties are required and non-nullable
- Username serves as the primary key for the user
- Password should be stored in hashed format only
- Email uniqueness should be enforced at application level
- Role determines user's access level in the system

## Related Entities
The User entity is referenced by:
- Booking: for tracking who made the booking
- Vehicle: for vehicle ownership
- Voucher: for voucher redemption tracking

## Security Considerations
- Passwords must be properly hashed before storage
- Username and email uniqueness should be enforced
- Role changes should be restricted to administrative operations
- Email format validation should be implemented at application level