using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingApp.BlazorUI.Migrations
{
    /// <inheritdoc />
    public partial class AddTradeEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FavouriteColor",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeliveryParity = table.Column<int>(type: "int", nullable: false),
                    LocationDetail = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductQualities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Protein = table.Column<float>(type: "real", nullable: false),
                    FallingNumber = table.Column<int>(type: "int", nullable: false),
                    TestWeight = table.Column<int>(type: "int", nullable: false),
                    Glassiness = table.Column<int>(type: "int", nullable: false),
                    GMP = table.Column<int>(type: "int", nullable: false),
                    ISCC = table.Column<int>(type: "int", nullable: false),
                    OilContent = table.Column<int>(type: "int", nullable: false),
                    DamagedKernels = table.Column<int>(type: "int", nullable: false),
                    Don = table.Column<int>(type: "int", nullable: false),
                    Afla = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductQualities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductQualityId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_ProductQualities_ProductQualityId",
                        column: x => x.ProductQualityId,
                        principalTable: "ProductQualities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TradeEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CatalogNumber = table.Column<int>(type: "int", nullable: false),
                    TradeDirection = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    DeliveryInfoId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ISCC = table.Column<int>(type: "int", nullable: false),
                    GMP = table.Column<int>(type: "int", nullable: false),
                    PrivateNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublicNotes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TradeEntries_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TradeEntries_DeliveryInfos_DeliveryInfoId",
                        column: x => x.DeliveryInfoId,
                        principalTable: "DeliveryInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TradeEntries_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductQualityId",
                table: "Products",
                column: "ProductQualityId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeEntries_CompanyId",
                table: "TradeEntries",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeEntries_DeliveryInfoId",
                table: "TradeEntries",
                column: "DeliveryInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeEntries_ProductId",
                table: "TradeEntries",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TradeEntries");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "DeliveryInfos");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ProductQualities");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FavouriteColor",
                table: "AspNetUsers");
        }
    }
}
