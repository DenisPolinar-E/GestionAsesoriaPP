using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionAsesoria.Operator.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddStarRequestToRequestPPP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "RequestPPP",
                newName: "EndRequest");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "RequestPPP",
                type: "datetime2",
                nullable: true,
                comment: "Fecha en la que se hizo la solicitud.",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "Fecha prevista de inicio de prácticas.");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartRequest",
                table: "RequestPPP",
                type: "datetime2",
                nullable: true,
                comment: "Fecha prevista de inicio de prácticas.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartRequest",
                table: "RequestPPP");

            migrationBuilder.RenameColumn(
                name: "EndRequest",
                table: "RequestPPP",
                newName: "EndDate");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "RequestPPP",
                type: "datetime2",
                nullable: true,
                comment: "Fecha prevista de inicio de prácticas.",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "Fecha en la que se hizo la solicitud.");
        }
    }
}
