namespace Employees.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddManager : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                "ManagerId",
                "Employees",
                nullable: true);

            migrationBuilder.CreateIndex(
                "IX_Employees_ManagerId",
                "Employees",
                "ManagerId");

            migrationBuilder.AddForeignKey(
                "FK_Employees_Employees_ManagerId",
                "Employees",
                "ManagerId",
                "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_Employees_Employees_ManagerId",
                "Employees");

            migrationBuilder.DropIndex(
                "IX_Employees_ManagerId",
                "Employees");

            migrationBuilder.DropColumn(
                "ManagerId",
                "Employees");
        }
    }
}