using Microsoft.EntityFrameworkCore.Migrations;

namespace ElectronicsStore.Data.Migrations
{
    public partial class FixingCartRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CartItem");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "CartItem",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
