using ParkNow.Models;
using ParkNow.Data;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ParkNow.Services;

public class UserService : IUserService
{
    // Get Database Context and Constructor to include context
    private readonly AppDbContext _context;
    public UserService(AppDbContext context) {
        _context = context;
    }
    public async Task<User> GetUser (string username) {
        return await _context.Users.Where(u => u.Username == username).FirstOrDefaultAsync();
    }
    public async Task<User.Roles> GetUserRole (string username) {
        return (await _context.Users.Where(u => u.Username == username).FirstOrDefaultAsync()).Role;
    }
    public async Task<List<User>> GetAllUsers () {
        return await _context.Users.ToListAsync();
    }
    
    public bool VerifyPassword(string password, string hash, string salt) {
        var byte_salt = Encoding.UTF8.GetBytes(salt);
        var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, byte_salt, 350000, HashAlgorithmName.SHA512, 64);
        return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
    }

    public string HashPassword(string password) {
        var byte_salt = Encoding.UTF8.GetBytes("35cdd9db5a7eb3bf27ecb65e351dd6d4088f82bbdedcc800ca2a44e4b34df82e946972ab434762f87cd56fe09e7e8b2d83c33f101874d7f1e66303c510256525");
        var hashBytes = Rfc2898DeriveBytes.Pbkdf2(password, byte_salt, 350000, HashAlgorithmName.SHA512, 64);
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant(); // Convert to hex string
    }

    public async Task<bool> VerifyCredentials(string username, string password) {
        var user = await _context.Users.Where(e => e.Username == username).SingleOrDefaultAsync();
        if (user == null) {
            return false;
        }
        bool valid = VerifyPassword(password, user.Password, "35cdd9db5a7eb3bf27ecb65e351dd6d4088f82bbdedcc800ca2a44e4b34df82e946972ab434762f87cd56fe09e7e8b2d83c33f101874d7f1e66303c510256525");
        if (user == null || !valid) {
            return false;
        }
        return true;
    }

    public async Task<bool> Register(string username, string email, string password) {
        bool exists = await _context.Users.AnyAsync(e => e.Username == username);
        if (exists){
            return false;
        }
        else {
            User temp = new User{Username=username,Password=password,Email=email, Role=User.Roles.User};
            _context.Users.Add(temp);
            await _context.SaveChangesAsync();
        }
        return true;
    }

   public async Task<bool> ChangePassword(string username, string oldPassword, string newPassword) {
        if (await VerifyCredentials(username, oldPassword)) {
            try {
                // Add User
                User? db_user = await _context.Users.Where(u => u.Username == username).FirstOrDefaultAsync();
                if (db_user == null) {
                    return false;
                }
                db_user.Password = HashPassword(newPassword);
                await _context.SaveChangesAsync();
            }
            catch {
                return false;
            }
            return true;
        }
        return false;
    }

    public async Task<bool> ChangeEmail(string username, string email) {
        User db_user = await GetUser(username);
        db_user.Email = email;
        await _context.SaveChangesAsync();
        return true;
    }

}
