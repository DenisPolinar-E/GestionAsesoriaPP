using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionAsesoria.Operator.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRequestRelationToPreProfessionalInternship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequestPPPId",
                table: "PreProfessionalInternship",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PreProfessionalInternship_RequestPPPId",
                table: "PreProfessionalInternship",
                column: "RequestPPPId");

            migrationBuilder.AddForeignKey(
                name: "PreProfessionalInternship_Request",
                table: "PreProfessionalInternship",
                column: "RequestPPPId",
                principalTable: "RequestPPP",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "PreProfessionalInternship_Request",
                table: "PreProfessionalInternship");

            migrationBuilder.DropIndex(
                name: "IX_PreProfessionalInternship_RequestPPPId",
                table: "PreProfessionalInternship");

            migrationBuilder.DropColumn(
                name: "RequestPPPId",
                table: "PreProfessionalInternship");
        }
    }
}
