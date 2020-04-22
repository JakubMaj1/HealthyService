using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthyService.Core.Migrations
{
    public partial class FirtMigrationCreateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Meals",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    Protein = table.Column<float>(nullable: false),
                    Carbo = table.Column<float>(nullable: false),
                    Fat = table.Column<float>(nullable: false),
                    ProductMeasure = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(maxLength: 20, nullable: false),
                    SureName = table.Column<string>(maxLength: 25, nullable: false),
                    Email = table.Column<string>(maxLength: 30, nullable: false),
                    Password = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersDetails",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    Weight = table.Column<long>(nullable: true),
                    Height = table.Column<long>(nullable: true),
                    Age = table.Column<long>(nullable: true),
                    ActivityLevel = table.Column<string>(nullable: false),
                    Gender = table.Column<string>(nullable: false),
                    UserProteinLevel = table.Column<long>(nullable: true),
                    UserCarboLevel = table.Column<long>(nullable: true),
                    UserFatLevel = table.Column<long>(nullable: true),
                    UserDemendLevel = table.Column<long>(nullable: true),
                    WaistCircumference = table.Column<long>(nullable: true),
                    HipCircumference = table.Column<long>(nullable: true),
                    ChestCircumference = table.Column<long>(nullable: true),
                    CalfCircumference = table.Column<long>(nullable: true),
                    ThighCircumference = table.Column<long>(nullable: true),
                    ArmCircumference = table.Column<long>(nullable: true),
                    ForearmCircumference = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersDetails", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Meals");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UsersDetails");
        }
    }
}
