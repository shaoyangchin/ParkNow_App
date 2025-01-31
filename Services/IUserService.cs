using ParkNow.Models;
namespace ParkNow.Services;
/*
This interface implements the methods of the User entity
Register, GetUser, GetUserRole, GetAllUsers, ChangePassword, ChangeEmail
HashPassword, VerifyPassword, VerifyCredentials

This entity has CRUD operations
*/

public interface IUserService
{
    // CRUD
    /*
    Creates a new user
    Returns true on successful creation, false if not
    */ 
    Task<bool> Register(string username, string email, string password);
    /*
    Get a specific user by username
    Returns the found User entity or null if not found
    */ 
    Task<User> GetUser (string username);
    /*
    Get a specific user's role by username
    Returns the found User role attribute or null if User is not found
    */ 
    Task<User.Roles> GetUserRole (string username);
    /*
    Return all users
    Returns a list of User entities or an empty list if not found
    */ 
    Task<List<User>> GetAllUsers ();
    /*
    Changes the password attribute of a User associated with the username
    Returns true on successful password change or false if unsuccessful
    */ 
    Task<bool> ChangePassword(string username, string oldPassword, string newPassword);
    /*
    Changes the email attribute of a User associated with the username
    Returns true on successful email change or false if unsuccessful
    */ 
    Task<bool> ChangeEmail(string username, string email);

    // Non CRUD
    /*
    Takes in a password and hashes and salts the password
    Returns the hashed and salted password in a hex string
    */ 
    string HashPassword(string password);
    /*
    Verifies that a password belongs to a user
    Returns true if password string matches hash, false otherwise
    */ 
    Task<bool> VerifyCredentials(string username, string password);

}
