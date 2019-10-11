using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DncAdmin.Api.Migrations
{
    public partial class add_menu_and_role : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "menu",
                schema: "dnc",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "newid()"),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Alias = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    ParentId = table.Column<Guid>(nullable: true),
                    Level = table.Column<int>(nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Sort = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Component = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    IsHide = table.Column<int>(nullable: false),
                    IsCache = table.Column<int>(nullable: false),
                    CreateOn = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_menu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_menu_menu_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "dnc",
                        principalTable: "menu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "role",
                schema: "dnc",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "newid()"),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    status = table.Column<int>(nullable: false),
                    IsSuperAdministrator = table.Column<int>(nullable: false),
                    IsBuiltin = table.Column<int>(nullable: false),
                    CreateOn = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User_Role_R",
                schema: "dnc",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Role_R", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_User_Role_R_role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dnc",
                        principalTable: "role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Role_R_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "dnc",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_menu_ParentId",
                schema: "dnc",
                table: "menu",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Role_R_RoleId",
                schema: "dnc",
                table: "User_Role_R",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "menu",
                schema: "dnc");

            migrationBuilder.DropTable(
                name: "User_Role_R",
                schema: "dnc");

            migrationBuilder.DropTable(
                name: "role",
                schema: "dnc");
        }
    }
}
