using AutoMapper;
using GestionAsesoria.Operator.Application.DTOs.DocumentCollection.Request;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Application.Mappings.DocumentCollections;
using GestionAsesoria.Operator.Domain.Entities;
using GestionAsesoria.Operator.Shared.Wrapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Tsp.Sigescom.Config;
using GestionAsesoria.Operator.Domain.ConfigParameters.Container;

namespace GestionAsesoria.Operator.Application.Features.DocumentCollections.Commands.Create
{
    public class CreateDocumentCollectionCommand : IRequest<Result<int>>
    {
        public CreateDocumentCollectionRequestDto Request { get; set; }
        
        // Propiedades opcionales que pueden ser establecidas por el comando padre
        public int? DocumentTypeId { get; set; }
        public int? UploadedByActorId { get; set; }
    }

    internal class CreateDocumentCollectionCommandHandler : IRequestHandler<CreateDocumentCollectionCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;

        public CreateDocumentCollectionCommandHandler(IMapper mapper, IUnitOfWork<int> unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(CreateDocumentCollectionCommand command, CancellationToken cancellationToken)
        {
            try
            {     
                // Mapear el DTO a la entidad de dominio
                var documentCollection = _mapper.Map<DocumentCollection>(command.Request);
                
                // Establecer valores desde el comando si están presentes
                if (command.DocumentTypeId.HasValue && command.DocumentTypeId.Value > 0)
                {
                    documentCollection.DocumentTypeId = command.DocumentTypeId.Value;
                }
                
                if (command.UploadedByActorId.HasValue && command.UploadedByActorId.Value > 0)
                {
                    documentCollection.UploadedByActorId = command.UploadedByActorId.Value;
                }
                
                // Establecer la fecha de subida si no está definida
                if (documentCollection.UploadDate == default)
                {
                    documentCollection.UploadDate = DateTime.UtcNow;
                }

                // Usar el repositorio para persistir la entidad
                await _unitOfWork.DocumentCollectionRepository.AddAsync(documentCollection);
                await _unitOfWork.Commit(cancellationToken);

                return await Result<int>.SuccessAsync(documentCollection.Id, "Colección de documentos creada exitosamente.");
            }
            catch (Exception ex)
            {
                return await Result<int>.FailAsync($"Error al crear la colección de documentos: {ex.Message}");
            }
        }
    }
} 