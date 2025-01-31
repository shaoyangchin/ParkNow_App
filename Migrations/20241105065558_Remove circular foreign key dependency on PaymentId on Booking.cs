using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkNow.Migrations
{
    /// <inheritdoc />
    public partial class RemovecircularforeignkeydependencyonPaymentIdonBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Payments_PaymentId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_PaymentId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "Bookings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_PaymentId",
                table: "Bookings",
                column: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Payments_PaymentId",
                table: "Bookings",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "PaymentId");
        }
    }
}
