using System;

namespace GestionAsesoria.Operator.Application.DTOs.DocumentCollection.Request
{
    public class CreateDocumentCollectionRequestDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }
        public DateTime UploadDate { get; set; }
        public int DocumentTypeId { get; set; }
        public string OnlineUrl { get; set; }
        public int UploadedByActorId { get; set; }
    }
} 