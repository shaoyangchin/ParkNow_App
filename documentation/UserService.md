
# UserService

> See complete implementation at `ParkNow.Services.UserService`

The `UserService` class provides methods to manage user operations within the ParkNow system. It includes user management operations such as registration, authentication, and profile updates.

## CRUD Operations

### Register
```csharp
Task<bool> Register(string username, string email, string password)
```
Creates a new user account in the system.

**Parameters:**
- `username` (string): The desired username for the new account
- `email` (string): The email address associated with the account
- `password` (string): The password for the new account

**Returns:**
- `Task<bool>`: True if registration is successful, false otherwise

### GetUser
```csharp
Task<User> GetUser(string username)
```
Retrieves user information for a specific username.

**Parameters:**
- `username` (string): The username to search for

**Returns:**
- `Task<User>`: The requested user entity, or null if not found

### GetUserRole
```csharp
Task<User.Roles> GetUserRole(string username)
```
Retrieves the role assigned to a specific user.

**Parameters:**
- `username` (string): The username to get the role for

**Returns:**
- `Task<User.Roles>`: The user's role, or null if user is not found

### GetAllUsers
```csharp
Task<List<User>> GetAllUsers()
```
Retrieves all users in the system.

**Returns:**
- `Task<List<User>>`: List of all user entities, or empty list if none found

### ChangePassword
```csharp
Task<bool> ChangePassword(string username, string oldPassword, string newPassword)
```
Updates a user's password after verifying their current password.

**Parameters:**
- `username` (string): The username of the account
- `oldPassword` (string): The current password for verification
- `newPassword` (string): The new password to set

**Returns:**
- `Task<bool>`: True if password change is successful, false otherwise

### ChangeEmail
```csharp
Task<bool> ChangeEmail(string username, string email)
```
Updates a user's email address.

**Parameters:**
- `username` (string): The username of the account
- `email` (string): The new email address

**Returns:**
- `Task<bool>`: True if email change is successful, false otherwise

## Authentication Functions

### HashPassword
```csharp
string HashPassword(string password)
```
Creates a secure hash of a password with salt.

**Parameters:**
- `password` (string): The password to hash

**Returns:**
- `string`: The hashed and salted password as a hexadecimal string

### VerifyCredentials
```csharp
Task<bool> VerifyCredentials(string username, string password)
```
Verifies a user's login credentials.

**Parameters:**
- `username` (string): The username to verify
- `password` (string): The password to verify

**Returns:**
- `Task<bool>`: True if credentials are valid, false otherwise

## Notes
- All operations except HashPassword are asynchronous
- Password hashing includes salting for additional security
- User roles are managed through an enumeration defined in the User model
- Email changes do not require password verification, but password changes do
