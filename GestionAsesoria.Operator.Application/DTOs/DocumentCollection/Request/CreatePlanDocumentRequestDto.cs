using System;

namespace GestionAsesoria.Operator.Application.DTOs.DocumentCollection.Request
{
    /// <summary>
    /// DTO simplificado para crear un documento de plan
    /// Omite campos que serán asignados automáticamente por el handler
    /// </summary>
    public class CreatePlanDocumentRequestDto
    {
        // Solo campos necesarios para un plan de documento
        public string Title { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }
        public string OnlineUrl { get; set; }
    }
} 