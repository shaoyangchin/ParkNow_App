using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkNow.Migrations
{
    /// <inheritdoc />
    public partial class Makevoucherusernamenullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the existing foreign key constraint
            migrationBuilder.DropForeignKey(
                name: "FK_Vouchers_Users_Username",
                table: "Vouchers");

            // Alter the column to be nullable
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Vouchers",
                type: "nvarchar(450)",
                nullable: true);

            // Add back the foreign key constraint with nullable allowed
            migrationBuilder.AddForeignKey(
                name: "FK_Vouchers_Users_Username",
                table: "Vouchers",
                column: "Username",
                principalTable: "Users",
                principalColumn: "Username",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the foreign key constraint
            migrationBuilder.DropForeignKey(
                name: "FK_Vouchers_Users_Username",
                table: "Vouchers");

            // Make the column non-nullable again
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Vouchers",
                type: "nvarchar(450)",
                nullable: false);

            // Add back the original foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "FK_Vouchers_Users_Username",
                table: "Vouchers",
                column: "Username",
                principalTable: "Users",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
