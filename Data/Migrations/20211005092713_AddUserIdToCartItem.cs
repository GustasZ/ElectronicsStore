using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ElectronicsStore.Data.Migrations
{
    public partial class AddUserIdToCartItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CartId",
                table: "CartItem");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "CartItem",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CartItem");

            migrationBuilder.AddColumn<int>(
                name: "CartId",
                table: "CartItem",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
