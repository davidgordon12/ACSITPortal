using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ACSITPortal.Migrations
{
    /// <inheritdoc />
    public partial class SaltPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserSalt",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserSalt",
                table: "Users");
        }
    }
}
