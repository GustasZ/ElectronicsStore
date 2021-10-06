using Microsoft.EntityFrameworkCore.Migrations;

namespace ElectronicsStore.Data.Migrations
{
    public partial class AddDescriptionToStoreItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "StoreItem",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "StoreItem");
        }
    }
}
