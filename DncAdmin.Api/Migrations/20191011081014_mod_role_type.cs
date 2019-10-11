using Microsoft.EntityFrameworkCore.Migrations;

namespace DncAdmin.Api.Migrations
{
    public partial class mod_role_type : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsSuperAdministrator",
                schema: "dnc",
                table: "role",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<bool>(
                name: "IsBuiltin",
                schema: "dnc",
                table: "role",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IsSuperAdministrator",
                schema: "dnc",
                table: "role",
                nullable: false,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<int>(
                name: "IsBuiltin",
                schema: "dnc",
                table: "role",
                nullable: false,
                oldClrType: typeof(bool));
        }
    }
}
