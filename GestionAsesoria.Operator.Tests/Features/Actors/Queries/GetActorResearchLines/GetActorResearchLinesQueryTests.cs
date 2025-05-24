using GestionAsesoria.Operator.Application.DTOs.Actor.Response.ActorResearchLine;
using GestionAsesoria.Operator.Application.Features.Actors.Queries.GetActorResearchLines;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Application.Interfaces.Services;
using GestionAsesoria.Operator.Application.Interfaces.Services.ExternalRequest;
using GestionAsesoria.Operator.Shared.Wrapper;
using AutoMapper;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GestionAsesoria.Operator.Tests.Features.Actors.Queries.GetActorResearchLines
{
    public class GetActorResearchLinesQueryTests
    {
        private readonly Mock<IUnitOfWork<int>> _mockUnitOfWork;
        private readonly Mock<IMessageService> _mockMessageService;
        private readonly Mock<IActorRepositoryAsync> _mockActorRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IEmailService> _mockEmailService;

        public GetActorResearchLinesQueryTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork<int>>();
            _mockMessageService = new Mock<IMessageService>();
            _mockActorRepository = new Mock<IActorRepositoryAsync>();
            _mockMapper = new Mock<IMapper>();
            _mockEmailService = new Mock<IEmailService>();
            
            _mockUnitOfWork.Setup(uow => uow.ActorRepository).Returns(_mockActorRepository.Object);
        }

        [Fact]
        public async Task Handle_WhenResearchLinesExist_ReturnsSuccessResult()
        {
            // Arrange
            int? groupId = 1;
            var expectedLines = new List<GetActorResearchLineDto>
            {
                new GetActorResearchLineDto { Id = 1, FirstName = "Línea 1" },
                new GetActorResearchLineDto { Id = 2, FirstName = "Línea 2" }
            };

            _mockActorRepository
                .Setup(repo => repo.GetResearchLinesAsync(groupId))
                .ReturnsAsync(expectedLines);

            _mockMessageService
                .Setup(ms => ms.GetDynamicMessage("General", "Success", "Successful"))
                .Returns("Operación exitosa");

            var handler = new GetActorResearchLinesQueryHandler(
                _mockMapper.Object,
                _mockUnitOfWork.Object,
                _mockEmailService.Object,
                _mockMessageService.Object);

            var query = new GetActorResearchLinesQuery { GroupId = groupId };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal(expectedLines, result.Data);
            Assert.Equal("Operación exitosa", result.Messages[0]);
        }

        [Fact]
        public async Task Handle_WhenNoResearchLinesExist_ReturnsFailResult()
        {
            // Arrange
            int? groupId = 1;
            var emptyLines = new List<GetActorResearchLineDto>();

            _mockActorRepository
                .Setup(repo => repo.GetResearchLinesAsync(groupId))
                .ReturnsAsync(emptyLines);

            _mockMessageService
                .Setup(ms => ms.GetDynamicMessage("General", "Validation", "QueryEmpty"))
                .Returns("No se encontraron resultados");

            var handler = new GetActorResearchLinesQueryHandler(
                _mockMapper.Object,
                _mockUnitOfWork.Object,
                _mockEmailService.Object,
                _mockMessageService.Object);

            var query = new GetActorResearchLinesQuery { GroupId = groupId };

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
            int? groupId = 1;

            _mockActorRepository
                .Setup(repo => repo.GetResearchLinesAsync(groupId))
                .ThrowsAsync(new System.Exception("Error de prueba"));

            _mockMessageService
                .Setup(ms => ms.GetDynamicMessage("General", "Error", "Exception"))
                .Returns("Error en la operación");

            var handler = new GetActorResearchLinesQueryHandler(
                _mockMapper.Object,
                _mockUnitOfWork.Object,
                _mockEmailService.Object,
                _mockMessageService.Object);

            var query = new GetActorResearchLinesQuery { GroupId = groupId };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("Error en la operación", result.Messages[0]);
        }

        [Fact]
        public async Task Handle_WithNullGroupId_ReturnsAllResearchLines()
        {
            // Arrange
            int? groupId = null;
            var expectedLines = new List<GetActorResearchLineDto>
            {
                new GetActorResearchLineDto { Id = 1, FirstName = "Línea 1" },
                new GetActorResearchLineDto { Id = 2, FirstName = "Línea 2" }
            };

            _mockActorRepository
                .Setup(repo => repo.GetResearchLinesAsync(groupId))
                .ReturnsAsync(expectedLines);

            _mockMessageService
                .Setup(ms => ms.GetDynamicMessage("General", "Success", "Successful"))
                .Returns("Operación exitosa");

            var handler = new GetActorResearchLinesQueryHandler(
                _mockMapper.Object,
                _mockUnitOfWork.Object,
                _mockEmailService.Object,
                _mockMessageService.Object);

            var query = new GetActorResearchLinesQuery { GroupId = null };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal(expectedLines, result.Data);
            Assert.Equal("Operación exitosa", result.Messages[0]);
        }
    }
} 