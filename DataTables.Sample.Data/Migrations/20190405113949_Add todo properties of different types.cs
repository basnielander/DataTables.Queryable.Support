using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataTables.Sample.Data.Migrations
{
    public partial class Addtodopropertiesofdifferenttypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var rand = new Random();

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ToDos",
                nullable: false,
                defaultValue: DateTime.Now.AddDays(-rand.Next(60)));

            migrationBuilder.AddColumn<DateTime>(
                name: "Deadline",
                table: "ToDos",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EstimatedDuration",
                table: "ToDos",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ExpectedCosts",
                table: "ToDos",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Finished",
                table: "ToDos",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ToDos");

            migrationBuilder.DropColumn(
                name: "Deadline",
                table: "ToDos");

            migrationBuilder.DropColumn(
                name: "EstimatedDuration",
                table: "ToDos");

            migrationBuilder.DropColumn(
                name: "ExpectedCosts",
                table: "ToDos");

            migrationBuilder.DropColumn(
                name: "Finished",
                table: "ToDos");
        }
    }
}
