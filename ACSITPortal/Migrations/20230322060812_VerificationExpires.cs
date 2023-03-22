using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ACSITPortal.Migrations
{
    /// <inheritdoc />
    public partial class VerificationExpires : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "VerificationExpires",
                table: "Users",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerificationExpires",
                table: "Users");
        }
    }
}
