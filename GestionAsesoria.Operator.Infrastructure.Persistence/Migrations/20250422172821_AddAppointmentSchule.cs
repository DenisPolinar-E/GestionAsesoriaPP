using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionAsesoria.Operator.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAppointmentSchule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentCollection_AdvisoringContract_ContractId",
                table: "DocumentCollection");

            migrationBuilder.DropIndex(
                name: "IX_DocumentCollection_ContractId",
                table: "DocumentCollection");

            migrationBuilder.DropColumn(
                name: "ContractId",
                table: "DocumentCollection");

            migrationBuilder.AlterColumn<int>(
                name: "PlanDocumentCollectionId",
                table: "Project",
                type: "int",
                nullable: true,
                comment: "Identificador de la colección de documentos del plan (DocumentCollection).",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Identificador de la colección de documentos del plan (DocumentCollection).");

            migrationBuilder.CreateTable(
                name: "AppointmentSchedule",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AdvisorId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeetingLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeZone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Attendees = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppointmentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentSchedule", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppointmentSchedule");

            migrationBuilder.AlterColumn<int>(
                name: "PlanDocumentCollectionId",
                table: "Project",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Identificador de la colección de documentos del plan (DocumentCollection).",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Identificador de la colección de documentos del plan (DocumentCollection).");

            migrationBuilder.AddColumn<int>(
                name: "ContractId",
                table: "DocumentCollection",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentCollection_ContractId",
                table: "DocumentCollection",
                column: "ContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentCollection_AdvisoringContract_ContractId",
                table: "DocumentCollection",
                column: "ContractId",
                principalTable: "AdvisoringContract",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
