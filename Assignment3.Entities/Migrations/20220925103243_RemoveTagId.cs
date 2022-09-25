using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Assignment3.Entities.Migrations
{
    public partial class RemoveTagId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TagId",
                table: "Tags");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TagId",
                table: "Tags",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
