// Ya definido por ti (solo se refactoriza si es necesario)
using Microsoft.AspNetCore.Http;

public class CreateRequestPPPRequestDto
{
    // Student
    public string StudentCode { get; set; }
    public string StudentFirstName { get; set; }
    public string StudentLastName { get; set; }
    public string StudentEmail { get; set; }
    public string StudentPhone { get; set; }
    public string StudentDni { get; set; }

    // Company
    public string CompanyName { get; set; }
    public string CompanyRuc { get; set; }
    public string CompanyRepresentativeName { get; set; }
    public string CompanyType { get; set; } // Público / Privado
    public string CompanyAddress { get; set; }

    // Representative
    public string RepresentativeFirstName { get; set; }
    public string RepresentativeLastName { get; set; }
    public string RepresentativeDni { get; set; }
    public string RepresentativeEmail { get; set; }
    public string RepresentativePhone { get; set; }
    public string RepresentativePosition { get; set; }

    // PPP Info
    public string Title { get; set; }
    public string? Plan { get; set; }
    public string? Functions { get; set; }
    public string? Modality { get; set; }
    public string? AssignedArea { get; set; }
    public string? Observations { get; set; }
    public int ResearchAreaId { get; set; }

    // Document
    public IFormFile? Document { get; set; }
}
