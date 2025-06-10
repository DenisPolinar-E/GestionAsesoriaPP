using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionAsesoria.Operator.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyRepresentativeToRequestPPP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Actor_AdvisorActorId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Actor_StudentActorId",
                table: "Appointment");

            migrationBuilder.AddColumn<int>(
                name: "CompanyRepresentativeId",
                table: "RequestPPP",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "ID del representante asignado por la empresa.");

            migrationBuilder.CreateIndex(
                name: "IX_RequestPPP_CompanyRepresentativeId",
                table: "RequestPPP",
                column: "CompanyRepresentativeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Actor_AdvisorActorId",
                table: "Appointment",
                column: "AdvisorActorId",
                principalTable: "Actor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Actor_StudentActorId",
                table: "Appointment",
                column: "StudentActorId",
                principalTable: "Actor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestPPP_CompanyRepresentative",
                table: "RequestPPP",
                column: "CompanyRepresentativeId",
                principalTable: "Actor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
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
                name: "FK_RequestPPP_CompanyRepresentative",
                table: "RequestPPP");

            migrationBuilder.DropIndex(
                name: "IX_RequestPPP_CompanyRepresentativeId",
                table: "RequestPPP");

            migrationBuilder.DropColumn(
                name: "CompanyRepresentativeId",
                table: "RequestPPP");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Actor_AdvisorActorId",
                table: "Appointment",
                column: "AdvisorActorId",
                principalTable: "Actor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Actor_StudentActorId",
                table: "Appointment",
                column: "StudentActorId",
                principalTable: "Actor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
