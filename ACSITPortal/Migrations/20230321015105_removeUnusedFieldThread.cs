using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ACSITPortal.Migrations
{
    /// <inheritdoc />
    public partial class removeUnusedFieldThread : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThreadTitle",
                table: "Threads");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThreadTitle",
                table: "Threads",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
