using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ACSITPortal.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnusedField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReplyTitle",
                table: "Replies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReplyTitle",
                table: "Replies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
