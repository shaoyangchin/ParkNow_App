using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkNow.Migrations
{
    /// <inheritdoc />
    public partial class Addeddeletedtovoucher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Vouchers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Vouchers");
        }
    }
}
