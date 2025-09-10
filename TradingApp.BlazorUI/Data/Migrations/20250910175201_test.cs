using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingApp.BlazorUI.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GMP",
                table: "ProductQualities");

            migrationBuilder.DropColumn(
                name: "ISCC",
                table: "ProductQualities");

            migrationBuilder.RenameColumn(
                name: "PublicNotes",
                table: "TradeEntries",
                newName: "Records");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Records",
                table: "TradeEntries",
                newName: "PublicNotes");

            migrationBuilder.AddColumn<int>(
                name: "GMP",
                table: "ProductQualities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ISCC",
                table: "ProductQualities",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
