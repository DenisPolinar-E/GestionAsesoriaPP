using AutoMapper;
using GestionAsesoria.Operator.Application.DTOs.Actor.Response;
using GestionAsesoria.Operator.Application.Features.Actors.Queries.GetActorsByMainRole;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Application.Interfaces.Services;
using Moq;

namespace GestionAsesoria.Operator.Tests.Features.Actors.Queries.GetChildActorsByParentAndRole
{
    public class GetChildActorsByParentAndRoleQueryTests
    {
        private readonly Mock<IUnitOfWork<int>> _mockUnitOfWork;
        private readonly Mock<IMessageService> _mockMessageService;
        private readonly Mock<IActorRepositoryAsync> _mockActorRepository;
        private readonly Mock<IMapper> _mockMapper;

        public GetChildActorsByParentAndRoleQueryTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork<int>>();
            _mockMessageService = new Mock<IMessageService>();
            _mockActorRepository = new Mock<IActorRepositoryAsync>();
            _mockMapper = new Mock<IMapper>();

            _mockUnitOfWork.Setup(uow => uow.ActorRepository).Returns(_mockActorRepository.Object);
        }

        [Fact]
        public async Task Handle_WhenChildActorsExist_ReturnsSuccessResult()
        {
            // Arrange
            var parentId = 1;
            var roleId = 2;
            var expectedActors = new List<ActorResponseDto>
            {
                new ActorResponseDto
                {
                    Id = 1,
                    FirstName = "Actor1",
                    MainRoleId = roleId,
                    RoleName = "Rol1",
                    IsActived = true
                },
                new ActorResponseDto
                {
                    Id = 2,
                    FirstName = "Actor2",
                    MainRoleId = roleId,
                    RoleName = "Rol1",
                    IsActived = true
                }
            };

            _mockActorRepository
                .Setup(repo => repo.GetChildActorsByParentAndRoleAsync(parentId, roleId))
                .ReturnsAsync(expectedActors);

            _mockMessageService
                .Setup(ms => ms.GetDynamicMessage("General", "Success", "Successful"))
                .Returns("Operación exitosa");

            // Configuración del mapper para que retorne la misma lista
            _mockMapper
                .Setup(m => m.Map<IEnumerable<ActorResponseDto>>(It.IsAny<IEnumerable<ActorResponseDto>>()))
                .Returns(expectedActors);

            var handler = new GetChildActorsByParentAndRoleQueryHandler(
                _mockUnitOfWork.Object,
                _mockMapper.Object,
                _mockMessageService.Object);

            var query = new GetChildActorsByParentAndRoleQuery
            {
                ParentId = parentId,
                RoleId = roleId
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal(expectedActors, result.Data);
            Assert.Equal("Operación exitosa", result.Messages[0]);
        }

        [Fact]
        public async Task Handle_WhenNoChildActorsExist_ReturnsFailResult()
        {
            // Arrange
            var parentId = 1;
            var roleId = 2;
            var emptyActors = new List<ActorResponseDto>();

            _mockActorRepository
                .Setup(repo => repo.GetChildActorsByParentAndRoleAsync(parentId, roleId))
                .ReturnsAsync(emptyActors);

            _mockMessageService
                .Setup(ms => ms.GetDynamicMessage("General", "Validation", "QueryEmpty"))
                .Returns("No se encontraron resultados");

            var handler = new GetChildActorsByParentAndRoleQueryHandler(
                _mockUnitOfWork.Object,
                _mockMapper.Object,
                _mockMessageService.Object);

            var query = new GetChildActorsByParentAndRoleQuery
            {
                ParentId = parentId,
                RoleId = roleId
            };

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
            var parentId = 1;
            var roleId = 2;

            _mockActorRepository
                .Setup(repo => repo.GetChildActorsByParentAndRoleAsync(parentId, roleId))
                .ThrowsAsync(new System.Exception("Error de prueba"));

            _mockMessageService
                .Setup(ms => ms.GetDynamicMessage("General", "Error", "Exception"))
                .Returns("Error en la operación");

            var handler = new GetChildActorsByParentAndRoleQueryHandler(
                _mockUnitOfWork.Object,
                _mockMapper.Object,
                _mockMessageService.Object);

            var query = new GetChildActorsByParentAndRoleQuery
            {
                ParentId = parentId,
                RoleId = roleId
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("Error en la operación", result.Messages[0]);
        }
    }
}