using GestionAsesoria.Operator.Application.DTOs.Actor.Response;
using GestionAsesoria.Operator.Application.DTOs.MasterDataValues;
using System;
using System.Collections.Generic;

namespace GestionAsesoria.Operator.Application.DTOs.DocumentCollection.Response
{
    public class DocumentCollectionResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }
        public DateTime UploadDate { get; set; }
        public string OnlineUrl { get; set; }
        public int DocumentTypeId { get; set; }
        public int UploadedByActorId { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public string LastModifiedBy { get; set; }

        // Propiedades de navegación
        public MasterDataValueResponseDto DocumentType { get; set; }
        public ActorResponseDto UploadedByActor { get; set; }
        //public ICollection<DocumentVersionResponseDto>? DocumentVersions { get; set; }
    }
} 