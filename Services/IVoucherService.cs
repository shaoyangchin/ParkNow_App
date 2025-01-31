using ParkNow.Models;
namespace ParkNow.Services;
/*
This interface implements the methods of the Vehicle entity
CreateVoucher, GetAllVouchers, GetVoucher, DeleteVoucher, VerifyVoucher

This entity has CRD operations
*/
public interface IVoucherService
{
    // CRUD
    /*
    Create a new voucher
    Returns true on successful creation, false if not
    */ 
    Task<bool> CreateVoucher(Voucher voucher);
    /*
    Return all vouchers that are not marked deleted
    Returns a list of voucher entities or an empty list if not found
    */ 
    Task<List<Voucher>> GetAllVouchers();
    /*
    Get a specific voucher by its id
    Returns the found voucher entity or null if not found
    */ 
    Task<Voucher> GetVoucher(string voucherId);
    /*
    Sets a voucher to deleted (DOES NOT DELETE FROM DATABASE)
    Returns true if deleted attributed is successfully set or false if not
    */ 
    Task<bool> DeleteVoucher(string voucherId);
    /*
    Verifies that a voucher can be redeemed by a user
    Returns true and the discount amount in a tuple if voucher can be redeemed, false and -1.00M otherwise
    */ 
    Task<(bool success, decimal amount)> VerifyVoucher(string username, string voucherId);
}
