namespace PhotoShare.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Albums",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    BackgroundColor = table.Column<int>(nullable: true),
                    IsPublic = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table => { table.PrimaryKey("PK_Albums", x => x.Id); });

            migrationBuilder.CreateTable(
                "Tags",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Tags", x => x.Id); });

            migrationBuilder.CreateTable(
                "Towns",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 60, nullable: false),
                    Country = table.Column<string>(maxLength: 60, nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Towns", x => x.Id); });

            migrationBuilder.CreateTable(
                "Pictures",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 50, nullable: false),
                    Caption = table.Column<string>(maxLength: 250, nullable: true),
                    Path = table.Column<string>(nullable: false),
                    AlbumId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures", x => x.Id);
                    table.ForeignKey(
                        "FK_Pictures_Albums_AlbumId",
                        x => x.AlbumId,
                        "Albums",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "AlbumTags",
                table => new
                {
                    AlbumId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumTags", x => new {x.AlbumId, x.TagId});
                    table.ForeignKey(
                        "FK_AlbumTags_Albums_AlbumId",
                        x => x.AlbumId,
                        "Albums",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_AlbumTags_Tags_TagId",
                        x => x.TagId,
                        "Tags",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Users",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(unicode: false, maxLength: 30, nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Email = table.Column<string>(maxLength: 80, nullable: false),
                    ProfilePictureId = table.Column<int>(nullable: true),
                    FirstName = table.Column<string>(maxLength: 60, nullable: true),
                    LastName = table.Column<string>(maxLength: 60, nullable: true),
                    BornTownId = table.Column<int>(nullable: true),
                    CurrentTownId = table.Column<int>(nullable: true),
                    RegisteredOn = table.Column<DateTime>(nullable: true, defaultValueSql: "GETDATE()"),
                    LastTimeLoggedIn = table.Column<DateTime>(nullable: true),
                    Age = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        "FK_Users_Towns_BornTownId",
                        x => x.BornTownId,
                        "Towns",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Users_Towns_CurrentTownId",
                        x => x.CurrentTownId,
                        "Towns",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Users_Pictures_ProfilePictureId",
                        x => x.ProfilePictureId,
                        "Pictures",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "AlbumRoles",
                table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    AlbumId = table.Column<int>(nullable: false),
                    Role = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumRoles", x => new {x.AlbumId, x.UserId});
                    table.ForeignKey(
                        "FK_AlbumRoles_Albums_AlbumId",
                        x => x.AlbumId,
                        "Albums",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_AlbumRoles_Users_UserId",
                        x => x.UserId,
                        "Users",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "Friendships",
                table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    FriendId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendships", x => new {x.UserId, x.FriendId});
                    table.ForeignKey(
                        "FK_Friendships_Users_FriendId",
                        x => x.FriendId,
                        "Users",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Friendships_Users_UserId",
                        x => x.UserId,
                        "Users",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                "IX_AlbumRoles_UserId",
                "AlbumRoles",
                "UserId");

            migrationBuilder.CreateIndex(
                "IX_AlbumTags_TagId",
                "AlbumTags",
                "TagId");

            migrationBuilder.CreateIndex(
                "IX_Friendships_FriendId",
                "Friendships",
                "FriendId");

            migrationBuilder.CreateIndex(
                "IX_Pictures_AlbumId",
                "Pictures",
                "AlbumId");

            migrationBuilder.CreateIndex(
                "IX_Users_BornTownId",
                "Users",
                "BornTownId");

            migrationBuilder.CreateIndex(
                "IX_Users_CurrentTownId",
                "Users",
                "CurrentTownId");

            migrationBuilder.CreateIndex(
                "IX_Users_ProfilePictureId",
                "Users",
                "ProfilePictureId",
                unique: true,
                filter: "[ProfilePictureId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                "IX_Users_Username",
                "Users",
                "Username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "AlbumRoles");

            migrationBuilder.DropTable(
                "AlbumTags");

            migrationBuilder.DropTable(
                "Friendships");

            migrationBuilder.DropTable(
                "Tags");

            migrationBuilder.DropTable(
                "Users");

            migrationBuilder.DropTable(
                "Towns");

            migrationBuilder.DropTable(
                "Pictures");

            migrationBuilder.DropTable(
                "Albums");
        }
    }
}