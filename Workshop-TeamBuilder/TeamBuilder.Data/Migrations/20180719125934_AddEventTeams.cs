namespace TeamBuilder.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddEventTeams : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "EventTeams",
                table => new
                {
                    EventId = table.Column<int>(nullable: false),
                    TeamId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTeams", x => new {x.EventId, x.TeamId});
                    table.ForeignKey(
                        "FK_EventTeams_Events_EventId",
                        x => x.EventId,
                        "Events",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_EventTeams_Teams_TeamId",
                        x => x.TeamId,
                        "Teams",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                "IX_EventTeams_TeamId",
                "EventTeams",
                "TeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "EventTeams");
        }
    }
}