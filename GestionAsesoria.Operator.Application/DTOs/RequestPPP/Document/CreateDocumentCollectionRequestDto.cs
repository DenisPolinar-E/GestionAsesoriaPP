using Microsoft.AspNetCore.Http;

public class CreateDocumentCollectionRequestDto
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public IFormFile File { get; set; }
}
