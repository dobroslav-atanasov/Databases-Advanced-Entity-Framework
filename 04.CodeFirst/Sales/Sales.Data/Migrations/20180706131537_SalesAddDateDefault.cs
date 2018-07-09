namespace P03_SalesDatabase.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class SalesAddDateDefault : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                "Date",
                "Sales",
                "DATETIME2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "GETDATE()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                "Date",
                "Sales",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "DATETIME2",
                oldDefaultValueSql: "GETDATE()");
        }
    }
}