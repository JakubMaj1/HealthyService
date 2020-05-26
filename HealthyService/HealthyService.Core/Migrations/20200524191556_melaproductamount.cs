using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthyService.Core.Migrations
{
    public partial class melaproductamount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Amount",
                table: "ProductMeals",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "ProductMeals");
        }
    }
}
