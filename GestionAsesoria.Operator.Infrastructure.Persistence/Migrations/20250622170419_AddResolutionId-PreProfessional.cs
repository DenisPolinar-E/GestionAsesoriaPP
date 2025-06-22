using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionAsesoria.Operator.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddResolutionIdPreProfessional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartRequest",
                table: "RequestPPP",
                newName: "StartPreProfessionalPractice");

            migrationBuilder.RenameColumn(
                name: "EndRequest",
                table: "RequestPPP",
                newName: "EndPreProfessionalPractice");

            migrationBuilder.AddColumn<int>(
                name: "ResolutionId",
                table: "PreProfessionalInternship",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PreProfessionalInternship_ResolutionId",
                table: "PreProfessionalInternship",
                column: "ResolutionId");

            migrationBuilder.AddForeignKey(
                name: "FK_PreProfessionalInternship_DocumentResolution",
                table: "PreProfessionalInternship",
                column: "ResolutionId",
                principalTable: "DocumentCollection",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreProfessionalInternship_DocumentResolution",
                table: "PreProfessionalInternship");

            migrationBuilder.DropIndex(
                name: "IX_PreProfessionalInternship_ResolutionId",
                table: "PreProfessionalInternship");

            migrationBuilder.DropColumn(
                name: "ResolutionId",
                table: "PreProfessionalInternship");

            migrationBuilder.RenameColumn(
                name: "StartPreProfessionalPractice",
                table: "RequestPPP",
                newName: "StartRequest");

            migrationBuilder.RenameColumn(
                name: "EndPreProfessionalPractice",
                table: "RequestPPP",
                newName: "EndRequest");
        }
    }
}
