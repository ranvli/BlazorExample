using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FullApp.Server.Migrations
{
    public partial class ModificarCamposUsuarios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Apellido2",
                table: "Usuarios",
                type: "varchar(45)",
                maxLength: 45,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(45)",
                oldMaxLength: 45);

            migrationBuilder.AlterColumn<string>(
                name: "TelefonoMovil",
                table: "Usuarios",
                type: "varchar(45)",
                maxLength: 45,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(45)",
                oldMaxLength: 45);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Apellido2",
                table: "Usuarios",
                type: "varchar(45)",
                maxLength: 45,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(45)",
                oldMaxLength: 45,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TelefonoMovil",
                table: "Usuarios",
                type: "varchar(45)",
                maxLength: 45,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(45)",
                oldMaxLength: 45,
                oldNullable: true);
        }
    }
}

