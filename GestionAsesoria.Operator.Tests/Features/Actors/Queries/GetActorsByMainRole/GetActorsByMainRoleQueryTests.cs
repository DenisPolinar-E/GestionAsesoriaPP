using GestionAsesoria.Operator.Application.DTOs.Actor.Response.ActorResearchGroup;
using GestionAsesoria.Operator.Application.Features.Actors.Queries.GetSelect;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Application.Interfaces.Services;
using GestionAsesoria.Operator.Domain.ConfigParameters.Container;
using GestionAsesoria.Operator.Domain.Entities;
using GestionAsesoria.Operator.Shared.Wrapper;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GestionAsesoria.Operator.Tests.Features.Actors.Queries.GetActorsByMainRole
{
    public class GetActorsByMainRoleQueryTests
    {
        private readonly Mock<IUnitOfWork<int>> _mockUnitOfWork;
        private readonly Mock<IMessageService> _mockMessageService;
        private readonly Mock<IActorRepositoryAsync> _mockActorRepository;

        public GetActorsByMainRoleQueryTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork<int>>();
            _mockMessageService = new Mock<IMessageService>();
            _mockActorRepository = new Mock<IActorRepositoryAsync>();
            
            // Configurar el UnitOfWork para devolver nuestro repositorio mock
            _mockUnitOfWork.Setup(uow => uow.ActorRepository).Returns(_mockActorRepository.Object);
        }

        [Fact]
        public async Task Handle_WhenResearchGroupsExist_ReturnsSuccessResult()
        {
            // Arrange
            var expectedGroups = new List<GetActorResearchGroupDto>
            {
                new GetActorResearchGroupDto { Id = 1, SecondName = "Grupo1" },
                new GetActorResearchGroupDto { Id = 2, SecondName = "Grupo2" }
            };

            _mockActorRepository
                .Setup(repo => repo.GetResearchGroupsAsync())
                .ReturnsAsync(expectedGroups);

            _mockMessageService
                .Setup(ms => ms.GetDynamicMessage("General", "Success", "Successful"))
                .Returns("Operación exitosa");

            var handler = new GetActorsByResearchGroupQueryHandler(
                _mockUnitOfWork.Object,
                null, // IMapper no es necesario para esta prueba
                _mockMessageService.Object);

            var query = new GetActorsByMainRoleQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal(expectedGroups, result.Data);
            Assert.Equal("Operación exitosa", result.Messages[0]);
        }

        [Fact]
        public async Task Handle_WhenNoResearchGroupsExist_ReturnsFailResult()
        {
            // Arrange
            var emptyGroups = new List<GetActorResearchGroupDto>();

            _mockActorRepository
                .Setup(repo => repo.GetResearchGroupsAsync())
                .ReturnsAsync(emptyGroups);

            _mockMessageService
                .Setup(ms => ms.GetDynamicMessage("General", "Validation", "QueryEmpty"))
                .Returns("No se encontraron resultados");

            var handler = new GetActorsByResearchGroupQueryHandler(
                _mockUnitOfWork.Object,
                null, // IMapper no es necesario para esta prueba
                _mockMessageService.Object);

            var query = new GetActorsByMainRoleQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("No se encontraron resultados", result.Messages[0]);
        }

        [Fact]
        public async Task Handle_WhenExceptionOccurs_ReturnsFailResult()
        {
            // Arrange
            _mockActorRepository
                .Setup(repo => repo.GetResearchGroupsAsync())
                .ThrowsAsync(new System.Exception("Error de prueba"));

            _mockMessageService
                .Setup(ms => ms.GetDynamicMessage("General", "Error", "Exception"))
                .Returns("Error en la operación");

            var handler = new GetActorsByResearchGroupQueryHandler(
                _mockUnitOfWork.Object,
                null, // IMapper no es necesario para esta prueba
                _mockMessageService.Object);

            var query = new GetActorsByMainRoleQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("Error en la operación", result.Messages[0]);
        }
    }
} 