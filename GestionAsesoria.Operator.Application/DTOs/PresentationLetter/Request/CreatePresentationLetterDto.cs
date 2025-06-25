// GestionAsesoria.Operator.Application/DTOs/PresentationLetter/Request/CreatePresentationLetterDto.cs
namespace GestionAsesoria.Operator.Application.DTOs.PresentationLetter.Request
{
    public class CreatePresentationLetterDto
    {
        public string StudentCode { get; set; } = null!;
        public string StudentName { get; set; } = null!;
        public string StudentEmail { get; set; } = null!;
        public string Career { get; set; } = null!;
        public string CompanyName { get; set; } = null!;
        public string CompanyRuc { get; set; } = null!;
        public string CompanyAddress { get; set; } = null!;

        public int RequestPPPId { get; set; }
    }
}