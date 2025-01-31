# VoucherService

> See complete implementation at `ParkNow.Services.VoucherService`

The `VoucherService` interface provides methods to manage voucher operations within the ParkNow system. It handles creation, retrieval, deletion, and verification of voucher records. Note that this service only supports Create, Read, and Delete operations (no updates).

## CRD Operations

### CreateVoucher
```csharp
Task<bool> CreateVoucher(Voucher voucher)
```
Creates a new voucher in the system.

**Parameters:**
- `voucher` (Voucher): The voucher entity to be created

**Returns:**
- `Task<bool>`: True if creation is successful, false otherwise

### GetAllVouchers
```csharp
Task<List<Voucher>> GetAllVouchers()
```
Retrieves all active (non-deleted) vouchers from the system.

**Returns:**
- `Task<List<Voucher>>`: List of all active vouchers, or empty list if none found

### GetVoucher
```csharp
Task<Voucher> GetVoucher(string voucherId)
```
Retrieves a specific voucher by its identifier.

**Parameters:**
- `voucherId` (string): The unique identifier of the voucher to retrieve

**Returns:**
- `Task<Voucher>`: The requested voucher entity, or null if not found

### DeleteVoucher
```csharp
Task<bool> DeleteVoucher(string voucherId)
```
Marks a voucher as deleted in the system (soft delete).

**Parameters:**
- `voucherId` (string): The ID of the voucher to mark as deleted

**Returns:**
- `Task<bool>`: True if deletion is successful, false otherwise

## Verification Functions

### VerifyVoucher
```csharp
Task<(bool success, decimal amount)> VerifyVoucher(string username, string voucherId)
```
Checks if a voucher can be redeemed by a specific user.

**Parameters:**
- `username` (string): The username of the user attempting to redeem
- `voucherId` (string): The ID of the voucher to verify

**Returns:**
- `Task<(bool success, decimal amount)>`: A tuple containing:
  - `success`: True if voucher can be redeemed, false otherwise
  - `amount`: The discount amount if successful, -1.00M if verification fails

## Notes
- All operations are asynchronous and return Task objects
- Voucher deletion is implemented as a soft delete (marked as deleted but not removed from database)
- GetAllVouchers only returns vouchers that haven't been marked as deleted
- The service does not support updating vouchers once created
- Voucher verification includes both validity checks and user eligibility
- Failed voucher verifications return a standard -1.00M amount
