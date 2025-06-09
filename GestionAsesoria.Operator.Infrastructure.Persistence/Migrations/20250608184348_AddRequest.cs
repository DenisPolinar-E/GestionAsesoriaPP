using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionAsesoria.Operator.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.CreateTable(
                name: "RequestPPP",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Título del plan de prácticas pre profesionales."),
                    Plan = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Plan detallado de las prácticas."),
                    Functions = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Funciones asignadas al estudiante."),
                    Modality = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Modalidad de ejecución de las prácticas."),
                    AssignedArea = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Área a la que fue asignado el estudiante."),
                    Observations = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Observaciones generales sobre la solicitud."),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Fecha prevista de inicio de prácticas."),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Fecha prevista de fin de prácticas."),
                    StudentId = table.Column<int>(type: "int", nullable: false, comment: "ID del estudiante solicitante."),
                    CompanyId = table.Column<int>(type: "int", nullable: false, comment: "ID de la empresa donde se realizarán las prácticas."),
                    RepresentativeId = table.Column<int>(type: "int", nullable: false, comment: "ID del representante legal de la empresa."),
                    ResearchAreaId = table.Column<int>(type: "int", nullable: false, comment: "ID del área académica asignada."),
                    DocumentCollectionId = table.Column<int>(type: "int", nullable: false, comment: "ID de la colección de documentos asociados."),
                    StatusId = table.Column<int>(type: "int", nullable: false, comment: "ID del estado actual de la solicitud.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestPPP", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestPPP_Company",
                        column: x => x.CompanyId,
                        principalTable: "Actor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestPPP_DocumentCollection",
                        column: x => x.DocumentCollectionId,
                        principalTable: "DocumentCollection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestPPP_Representative",
                        column: x => x.RepresentativeId,
                        principalTable: "Actor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestPPP_ResearchArea",
                        column: x => x.ResearchAreaId,
                        principalTable: "Actor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestPPP_Status",
                        column: x => x.StatusId,
                        principalTable: "MasterDataValue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestPPP_Student",
                        column: x => x.StudentId,
                        principalTable: "Actor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Representa una solicitud de prácticas pre profesionales realizada por un estudiante.");

            
            migrationBuilder.CreateIndex(
                name: "IX_RequestPPP_CompanyId",
                table: "RequestPPP",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestPPP_DocumentCollectionId",
                table: "RequestPPP",
                column: "DocumentCollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestPPP_RepresentativeId",
                table: "RequestPPP",
                column: "RepresentativeId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestPPP_ResearchAreaId",
                table: "RequestPPP",
                column: "ResearchAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestPPP_StatusId",
                table: "RequestPPP",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestPPP_StudentId",
                table: "RequestPPP",
                column: "StudentId");

            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Actor_AdvisorActorId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Actor_StudentActorId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_MasterDataValue_AppointmentTypeId",
                table: "Appointment");

            migrationBuilder.DropTable(
                name: "RequestPPP");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_AdvisorActorId",
                table: "Appointment");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_AppointmentTypeId",
                table: "Appointment");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_StudentActorId",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "AdvisorActorId",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "AppointmentTypeId",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "GoogleEventId",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "MeetingLink",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "StudentActorId",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "TimeZone",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Appointment");

            migrationBuilder.AlterColumn<int>(
                name: "DocumentCollectionId",
                table: "ScientificProductionEvaluation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Identificador de la colección de documentos asociados a la evaluación (DocumentCollection).",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Identificador de la colección de documentos asociados a la evaluación (DocumentCollection).");

            migrationBuilder.AlterColumn<int>(
                name: "DocumentCollectionId",
                table: "ProjectEvaluation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Identificador de la colección de documentos asociados a la evaluación (DocumentCollection).",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Identificador de la colección de documentos asociados a la evaluación (DocumentCollection).");

            migrationBuilder.AlterColumn<int>(
                name: "PlanDocumentCollectionId",
                table: "Project",
                type: "int",
                nullable: true,
                comment: "Identificador de la colección de documentos del plan (DocumentCollection).",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Identificador de la colección de documentos del plan (DocumentCollection).");

            migrationBuilder.AlterColumn<int>(
                name: "OdsObjectiveId",
                table: "Project",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Identificador del obejtivo ODS para el proyecto (MasterDataValue).",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Identificador del obejtivo ODS para el proyecto (MasterDataValue).");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Appointment",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Ubicación de la cita.",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "Ubicación de la cita (física o virtual)");

            migrationBuilder.AlterColumn<int>(
                name: "CurrentAppointmentStatusId",
                table: "Appointment",
                type: "int",
                nullable: false,
                comment: "Identificador del estado actual de la cita.",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Estado actual de la cita");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Appointment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "Fecha de la cita.");

            migrationBuilder.AlterColumn<int>(
                name: "DocumentCollectionId",
                table: "AdvanceEvaluation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Identificador de la colección de documentos asociados a la evaluación (DocumentCollection).",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Identificador de la colección de documentos asociados a la evaluación (DocumentCollection).");

            migrationBuilder.CreateTable(
                name: "AppointmentSchedule",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AdvisorId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppointmentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Attendees = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeetingLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeZone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentSchedule", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoogleCalendarEvent",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeZone = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoogleCalendarEvent", x => x.Id);
                });
        }
    }
}
