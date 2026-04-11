using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fixawy.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateApplicationuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_applicationuserOtps_AspNetUsers_ApplicationUserId",
                table: "applicationuserOtps");

            migrationBuilder.DropColumn(
                name: "ApplicationUserOtp",
                table: "applicationuserOtps");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "applicationuserOtps",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_applicationuserOtps_AspNetUsers_ApplicationUserId",
                table: "applicationuserOtps",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_applicationuserOtps_AspNetUsers_ApplicationUserId",
                table: "applicationuserOtps");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "applicationuserOtps",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserOtp",
                table: "applicationuserOtps",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_applicationuserOtps_AspNetUsers_ApplicationUserId",
                table: "applicationuserOtps",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
