using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ACSITPortal.Migrations
{
    /// <inheritdoc />
    public partial class ResetTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Users",
                newName: "Email");

            migrationBuilder.AddColumn<string>(
                name: "ActionToken",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TokenExpires",
                table: "Users",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActionToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TokenExpires",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users",
                newName: "Phone");
        }
    }
}
