using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DncAdmin.Api.Migrations
{
    public partial class Add_User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dnc");

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "dnc",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "newid()"),
                    Account = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    NiName = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(32)", nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    Status = table.Column<int>(nullable: false, defaultValue: 0),
                    Remark = table.Column<string>(nullable: true),
                    CreateOn = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Account",
                schema: "dnc",
                table: "Users",
                column: "Account",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users",
                schema: "dnc");
        }
    }
}
