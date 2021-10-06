using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ElectronicsStore.Data.Migrations
{
    public partial class RevampCartItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderStoreItem");

            migrationBuilder.CreateTable(
                name: "CartItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StoreItemId = table.Column<int>(type: "int", nullable: false),
                    ShoppingUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItem_AspNetUsers_ShoppingUserId",
                        column: x => x.ShoppingUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CartItem_StoreItem_StoreItemId",
                        column: x => x.StoreItemId,
                        principalTable: "StoreItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_ShoppingUserId",
                table: "CartItem",
                column: "ShoppingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_StoreItemId",
                table: "CartItem",
                column: "StoreItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItem");

            migrationBuilder.CreateTable(
                name: "OrderStoreItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    ShoppingUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStoreItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderStoreItem_AspNetUsers_ShoppingUserId",
                        column: x => x.ShoppingUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderStoreItem_ShoppingUserId",
                table: "OrderStoreItem",
                column: "ShoppingUserId");
        }
    }
}
