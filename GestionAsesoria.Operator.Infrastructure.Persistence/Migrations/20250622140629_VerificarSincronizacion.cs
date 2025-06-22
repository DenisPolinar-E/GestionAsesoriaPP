using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionAsesoria.Operator.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class VerificarSincronizacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.DropForeignKey(
                name: "PreProfessionalInternship_Request",
                table: "PreProfessionalInternship");

            migrationBuilder.DropForeignKey(
                name: "FK_PreProfessionalInternshipByAdvisoringContract_AdvisoringContract_AdvisoringContractId",
                table: "PreProfessionalInternshipByAdvisoringContract");

            migrationBuilder.DropForeignKey(
                name: "FK_PreProfessionalInternshipByAdvisoringContract_PreProfessionalInternship_PreProfessionalInternshipId",
                table: "PreProfessionalInternshipByAdvisoringContract");

            migrationBuilder.AlterColumn<int>(
                name: "AdvisoringContractId",
                table: "PreProfessionalInternshipByAdvisoringContract",
                type: "int",
                nullable: true,
                comment: "Identificador del contrato de asesoría asociado.",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Identificador del contrato de asesoría asociado.");

            migrationBuilder.AlterColumn<int>(
                name: "RequestPPPId",
                table: "PreProfessionalInternship",
                type: "int",
                nullable: false,
                comment: "ID de la solicitud de Prácticas Pre Profesionales asociada.",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_PreProfessionalInternship_RequestPPP",
                table: "PreProfessionalInternship",
                column: "RequestPPPId",
                principalTable: "RequestPPP",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PPPContract_AdvisoringContract",
                table: "PreProfessionalInternshipByAdvisoringContract",
                column: "AdvisoringContractId",
                principalTable: "AdvisoringContract",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PPPContract_Internship",
                table: "PreProfessionalInternshipByAdvisoringContract",
                column: "PreProfessionalInternshipId",
                principalTable: "PreProfessionalInternship",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);*/
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.DropForeignKey(
                name: "FK_PreProfessionalInternship_RequestPPP",
                table: "PreProfessionalInternship");

            migrationBuilder.DropForeignKey(
                name: "FK_PPPContract_AdvisoringContract",
                table: "PreProfessionalInternshipByAdvisoringContract");

            migrationBuilder.DropForeignKey(
                name: "FK_PPPContract_Internship",
                table: "PreProfessionalInternshipByAdvisoringContract");

            migrationBuilder.AlterColumn<int>(
                name: "AdvisoringContractId",
                table: "PreProfessionalInternshipByAdvisoringContract",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Identificador del contrato de asesoría asociado.",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Identificador del contrato de asesoría asociado.");

            migrationBuilder.AlterColumn<int>(
                name: "RequestPPPId",
                table: "PreProfessionalInternship",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "ID de la solicitud de Prácticas Pre Profesionales asociada.");

            migrationBuilder.AddForeignKey(
                name: "PreProfessionalInternship_Request",
                table: "PreProfessionalInternship",
                column: "RequestPPPId",
                principalTable: "RequestPPP",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PreProfessionalInternshipByAdvisoringContract_AdvisoringContract_AdvisoringContractId",
                table: "PreProfessionalInternshipByAdvisoringContract",
                column: "AdvisoringContractId",
                principalTable: "AdvisoringContract",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PreProfessionalInternshipByAdvisoringContract_PreProfessionalInternship_PreProfessionalInternshipId",
                table: "PreProfessionalInternshipByAdvisoringContract",
                column: "PreProfessionalInternshipId",
                principalTable: "PreProfessionalInternship",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);*/
        }
    }
}
