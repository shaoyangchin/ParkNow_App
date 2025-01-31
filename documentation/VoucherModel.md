# Voucher Model

> See complete implementation at `ParkNow.Models.Voucher`

The `Voucher` class represents a discount voucher in the ParkNow system. It tracks voucher details, ownership, and validity period.

## Properties

### Primary Key
```csharp
[DatabaseGenerated(DatabaseGeneratedOption.None)]
public required string VoucherId { get; set; }
```
- Primary key for voucher identification
- Custom voucher ID (not auto-generated)
- Required field
- Annotated to prevent automatic ID generation

### Navigation Properties
```csharp
public string? Username { get; set; }

[ForeignKey(nameof(Username))]
public User? User { get; set; }
```
- Links voucher to a user (optional)
- Foreign key relationship to User entity
- Nullable to allow unassigned vouchers
- Can be assigned to a user later

### Voucher Details
```csharp
[Precision(18, 2)]
public decimal Amount { get; set; }
```
- Discount amount of the voucher
- Stored with 18 digits total, 2 decimal places
- Represents the monetary value of the voucher

```csharp
public DateTime Issue { get; set; }
```
- Date and time when the voucher was issued
- Used for tracking voucher creation

```csharp
public DateTime Expiry { get; set; }
```
- Date and time when the voucher expires
- Used for validating voucher usage

### Soft Delete
```csharp
public required bool Deleted { get; set; }
```
- Indicates if the voucher is marked as deleted
- Required field
- Used for soft delete functionality
- Defaults to false for new vouchers

## Database Configuration
- Uses Entity Framework Core
- Decimal precision configured using `[Precision(18, 2)]` attribute
- Custom ID generation strategy specified
- Foreign key relationship defined using `[ForeignKey]` attribute
- Soft delete implemented through Deleted flag

## Relationships
- Optional one-to-many relationship with User (voucher can be unassigned)
- Referenced by Payment entity for tracking voucher usage

## Validation Rules
- VoucherId must be unique
- Amount must be a positive decimal value
- Issue date must be before Expiry date
- Expiry date must be in the future when issued

## Notes
- VoucherId is manually generated (not auto-incremented)
- Vouchers can exist without being assigned to a user
- Soft delete allows tracking of used/expired vouchers
- Amount is stored with high precision for accurate calculations
- Validity period defined by Issue and Expiry dates

## Usage Considerations
- Check Deleted flag when querying vouchers
- Validate expiry date before allowing voucher use
- Ensure voucher assignment is tracked properly
- Handle soft delete appropriately in queries
- Consider implementing voucher usage limits
- Track voucher redemption through Payment entity