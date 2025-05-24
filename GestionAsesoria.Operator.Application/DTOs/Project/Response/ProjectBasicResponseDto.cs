using GestionAsesoria.Operator.Application.DTOs.Actor.Response;
using GestionAsesoria.Operator.Application.DTOs.DocumentCollection.Response;
using GestionAsesoria.Operator.Application.DTOs.MasterData.Response;

namespace GestionAsesoria.Operator.Application.DTOs.Project.Response
{
    public class ProjectBasicResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        
        // Información del autor principal
        public int AuthorProjectId { get; set; }
        public ActorResponseDto AuthorProject { get; set; }
        
        // Clasificación del proyecto
        public int ClassificationProjectId { get; set; }
        public MasterDataValueResponseDto ClassificationProject { get; set; }
        
        // Plan del proyecto
        public int? PlanDocumentCollectionId { get; set; }
        public DocumentCollectionResponseDto? PlanDocumentCollection { get; set; }
    }
} 