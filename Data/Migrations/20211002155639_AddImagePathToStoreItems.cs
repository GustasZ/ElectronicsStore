using Microsoft.EntityFrameworkCore.Migrations;

namespace ElectronicsStore.Data.Migrations
{
    public partial class AddImagePathToStoreItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "StoreItem",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "StoreItem");
        }
    }
}
