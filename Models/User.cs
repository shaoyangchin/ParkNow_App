/*
This class implements the User entity with the attributes
Username, Password, Email, Role
*/
using System.ComponentModel.DataAnnotations;

namespace ParkNow.Models;
public class User
{
    public enum Roles
    {
        User,
        Admin
    }
    [Key]
    public required string Username {get; set;}
    public required string Password {get; set;}
    public required string Email {get; set;}
    public required Roles Role {get; set;}
}
