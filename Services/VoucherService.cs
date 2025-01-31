using Microsoft.EntityFrameworkCore;
using ParkNow.Models;
using ParkNow.Data;

namespace ParkNow.Services;

public class VoucherService : IVoucherService
{
    // Get Database Context and Constructor to include context
    private readonly ILogger<VoucherService> _logger;
    private readonly AppDbContext _context;
    public VoucherService(AppDbContext context, ILogger<VoucherService> logger) {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> CreateVoucher(Voucher voucher) {
        try {
            // Add Voucher
            await _context.Vouchers.AddAsync(voucher);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e){
                _logger.LogInformation(e.Message);
                _logger.LogInformation(e.InnerException.Message);
                return false;
        }
    }

    public async Task<List<Voucher>> GetAllVouchers() {
        return await _context.Vouchers.Where(v => v.Deleted == false).ToListAsync();
    }
    public async Task<Voucher> GetVoucher(string voucherId) {
        return await _context.Vouchers.Where(v => v.VoucherId == voucherId).FirstOrDefaultAsync();
    }

    public async Task<bool> DeleteVoucher(string voucherId) {
        try {
            Voucher? db_Voucher = await _context.Vouchers.Where(v => v.VoucherId == voucherId).FirstOrDefaultAsync();
            if (db_Voucher == null) {
                return false;
            }
            db_Voucher.Deleted = true;
            await _context.SaveChangesAsync();
       }
       catch {
            return false;
       }
        return true;
    }

    public async Task<(bool success,decimal amount)> VerifyVoucher(string username, string voucherId) {
        var voucher = await _context.Vouchers.Where(v => v.VoucherId == voucherId && (v.Username == username || v.Username == null)).SingleOrDefaultAsync();
        if (voucher == null) {
            return (false,-1.0M);
        }
        return (true,voucher.Amount);
    }
}
