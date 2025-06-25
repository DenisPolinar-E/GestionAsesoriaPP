// GestionAsesoria.Operator.Application/DTOs/PresentationLetter/Response/PresentationLetterDto.cs
namespace GestionAsesoria.Operator.Application.DTOs.PresentationLetter.Response
{
    public class PresentationLetterDto
    {
        public int Id { get; set; }
        public string StudentCode { get; set; } = null!;
        public string StudentName { get; set; } = null!;
        public string StudentEmail { get; set; } = null!;
        public string Career { get; set; } = null!;
        public string CompanyName { get; set; } = null!;
        public string CompanyRuc { get; set; } = null!;
        public string CompanyAddress { get; set; } = null!;

        public string Status { get; set; } = null!;
    }
}