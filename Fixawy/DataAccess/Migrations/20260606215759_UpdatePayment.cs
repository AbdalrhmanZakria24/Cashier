using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fixawy.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_payments_Orders_OrderId",
                table: "payments");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "payments",
                newName: "cartId");

            migrationBuilder.RenameIndex(
                name: "IX_payments_OrderId",
                table: "payments",
                newName: "IX_payments_cartId");

            migrationBuilder.AddColumn<long>(
                name: "PaymentId",
                table: "Orders",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentId",
                table: "Orders",
                column: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_payments_PaymentId",
                table: "Orders",
                column: "PaymentId",
                principalTable: "payments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_payments_carts_cartId",
                table: "payments",
                column: "cartId",
                principalTable: "carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_payments_PaymentId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_payments_carts_cartId",
                table: "payments");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PaymentId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "cartId",
                table: "payments",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_payments_cartId",
                table: "payments",
                newName: "IX_payments_OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_payments_Orders_OrderId",
                table: "payments",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
