using Microsoft.EntityFrameworkCore.Migrations;

namespace ElectronicsStore.Data.Migrations
{
    public partial class RedoingCartRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItem_AspNetUsers_ShoppingUserId",
                table: "CartItem");

            migrationBuilder.DropIndex(
                name: "IX_CartItem_ShoppingUserId",
                table: "CartItem");

            migrationBuilder.DropColumn(
                name: "ShoppingUserId",
                table: "CartItem");

            migrationBuilder.AddColumn<int>(
                name: "CartId",
                table: "CartItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShoppingUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Total = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cart_AspNetUsers_ShoppingUserId",
                        column: x => x.ShoppingUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_CartId",
                table: "CartItem",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_ShoppingUserId",
                table: "Cart",
                column: "ShoppingUserId",
                unique: true,
                filter: "[ShoppingUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItem_Cart_CartId",
                table: "CartItem",
                column: "CartId",
                principalTable: "Cart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItem_Cart_CartId",
                table: "CartItem");

            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropIndex(
                name: "IX_CartItem_CartId",
                table: "CartItem");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "CartItem");

            migrationBuilder.AddColumn<string>(
                name: "ShoppingUserId",
                table: "CartItem",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_ShoppingUserId",
                table: "CartItem",
                column: "ShoppingUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItem_AspNetUsers_ShoppingUserId",
                table: "CartItem",
                column: "ShoppingUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
