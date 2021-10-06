using Microsoft.EntityFrameworkCore.Migrations;

namespace ElectronicsStore.Data.Migrations
{
    public partial class FixingOrderStoreItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoreItem_AspNetUsers_ShoppingUserId",
                table: "StoreItem");

            migrationBuilder.DropIndex(
                name: "IX_StoreItem_ShoppingUserId",
                table: "StoreItem");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "StoreItem");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "StoreItem");

            migrationBuilder.DropColumn(
                name: "ShoppingUserId",
                table: "StoreItem");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderStoreItem");

            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "StoreItem",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "StoreItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShoppingUserId",
                table: "StoreItem",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StoreItem_ShoppingUserId",
                table: "StoreItem",
                column: "ShoppingUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreItem_AspNetUsers_ShoppingUserId",
                table: "StoreItem",
                column: "ShoppingUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
