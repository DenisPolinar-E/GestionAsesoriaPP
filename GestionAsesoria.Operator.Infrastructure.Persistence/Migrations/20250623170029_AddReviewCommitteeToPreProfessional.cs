using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionAsesoria.Operator.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddReviewCommitteeToPreProfessional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReviewCommitteePrimaryId",
                table: "PreProfessionalInternship",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReviewCommitteeSecondaryId",
                table: "PreProfessionalInternship",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PreProfessionalInternship_ReviewCommitteePrimaryId",
                table: "PreProfessionalInternship",
                column: "ReviewCommitteePrimaryId");

            migrationBuilder.CreateIndex(
                name: "IX_PreProfessionalInternship_ReviewCommitteeSecondaryId",
                table: "PreProfessionalInternship",
                column: "ReviewCommitteeSecondaryId");

            migrationBuilder.AddForeignKey(
                name: "FK_PreProfessionalInternship_ReviewCommitteePrimary",
                table: "PreProfessionalInternship",
                column: "ReviewCommitteePrimaryId",
                principalTable: "ActorSecondaryRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PreProfessionalInternship_ReviewCommitteeSecondary",
                table: "PreProfessionalInternship",
                column: "ReviewCommitteeSecondaryId",
                principalTable: "ActorSecondaryRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreProfessionalInternship_ReviewCommitteePrimary",
                table: "PreProfessionalInternship");

            migrationBuilder.DropForeignKey(
                name: "FK_PreProfessionalInternship_ReviewCommitteeSecondary",
                table: "PreProfessionalInternship");

            migrationBuilder.DropIndex(
                name: "IX_PreProfessionalInternship_ReviewCommitteePrimaryId",
                table: "PreProfessionalInternship");

            migrationBuilder.DropIndex(
                name: "IX_PreProfessionalInternship_ReviewCommitteeSecondaryId",
                table: "PreProfessionalInternship");

            migrationBuilder.DropColumn(
                name: "ReviewCommitteePrimaryId",
                table: "PreProfessionalInternship");

            migrationBuilder.DropColumn(
                name: "ReviewCommitteeSecondaryId",
                table: "PreProfessionalInternship");
        }
    }
}
