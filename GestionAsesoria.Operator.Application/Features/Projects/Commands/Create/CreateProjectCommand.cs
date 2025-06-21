using AutoMapper;
using GestionAsesoria.Operator.Application.DTOs.Project.Request;
using GestionAsesoria.Operator.Application.DTOs.DocumentCollection.Request;
using GestionAsesoria.Operator.Application.Features.DocumentCollections.Commands.Create;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Domain.Entities.ProjectIDI;
using GestionAsesoria.Operator.Shared.Wrapper;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GestionAsesoria.Operator.Domain.ConfigParameters.Container;
using Tsp.Sigescom.Config;

namespace GestionAsesoria.Operator.Application.Features.Projects.Commands.Create
{
    public class CreateProjectCommand : IRequest<Result<int>>
    {
        public CreateProjectRequestDto Request { get; set; }
        
        // Usar el DTO simplificado en lugar del completo
        public CreatePlanDocumentRequestDto PlanDocument { get; set; }
    }
    
    internal class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMediator _mediator;
        private readonly SettingsContainer _settingsContainer;

        public CreateProjectCommandHandler(
            IMapper mapper,
            IUnitOfWork<int> unitOfWork,
            IMediator mediator)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _settingsContainer = LocalSettingContainer.Get();
        }

        public async Task<Result<int>> Handle(CreateProjectCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Asignar título y descripción si no existen
                if (string.IsNullOrWhiteSpace(command.PlanDocument.Title))
                {
                    command.PlanDocument.Title = $"Plan de proyecto: {command.Request.Title}";
                }

                if (string.IsNullOrWhiteSpace(command.PlanDocument.Description))
                {
                    command.PlanDocument.Description = "Documentación del plan de proyecto";
                }
                
                // Mapear el DTO simplificado al DTO completo
                var documentCollectionRequest = _mapper.Map<CreateDocumentCollectionRequestDto>(command.PlanDocument);
                
                // Crear la colección de documentos usando las propiedades del comando
                var createDocumentCollectionCommand = new CreateDocumentCollectionCommand
                {
                    Request = documentCollectionRequest,
                    DocumentTypeId = _settingsContainer.LocalDocumentTypeSettings.DocumentTypeId,
                    UploadedByActorId = command.Request.AuthorProjectId
                };

                // Crear la colección de documentos
                var documentCollectionResult = await _mediator.Send(createDocumentCollectionCommand, cancellationToken);
                if (!documentCollectionResult.Succeeded)
                {
                    return await Result<int>.FailAsync($"Error al crear la colección de documentos: {documentCollectionResult.Messages.FirstOrDefault()}");
                }

                int planDocumentCollectionId = documentCollectionResult.Data;

                // Crear el proyecto
                var project = _mapper.Map<Project>(command.Request);
                
                // Asignar el ID de la colección de documentos al proyecto
                project.PlanDocumentCollectionId = planDocumentCollectionId;
                
                // Establecer el estado inicial del proyecto
                project.StateProjectId = _settingsContainer.LocalStateProjectTypeSettings.StateProjectTypeId;
                
                // Persistir el proyecto
                await _unitOfWork.ProjectRepository.AddAsync(project);
                await _unitOfWork.Commit(cancellationToken);

                return await Result<int>.SuccessAsync(project.Id, "Proyecto creado exitosamente.");
            }
            catch (Exception ex)
            {
                return await Result<int>.FailAsync($"Error al crear el proyecto: {ex.Message}");
            }
        }
    }
}
