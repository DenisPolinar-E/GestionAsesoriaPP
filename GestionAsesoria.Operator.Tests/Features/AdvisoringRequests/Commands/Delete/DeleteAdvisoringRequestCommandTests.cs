using GestionAsesoria.Operator.Application.Features.AdvisoringRequests.Commands.Delete;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Application.Interfaces.Services;
using GestionAsesoria.Operator.Domain.Entities;
using GestionAsesoria.Operator.Shared.Static;
using GestionAsesoria.Operator.Tests.Helpers;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GestionAsesoria.Operator.Tests.Features.AdvisoringRequests.Commands.Delete
{
    public class DeleteAdvisoringRequestCommandTests
    {
        private readonly Mock<IUnitOfWork<int>> _mockUnitOfWork;
        private readonly Mock<IMessageService> _mockMessageService;
        private readonly MockLogger<DeleteAdvisoringRequestCommandHandler> _mockLogger;
        private readonly Mock<IAdvisoringRequestRepositoryAsync> _mockAdvisoringRequestRepository;

        public DeleteAdvisoringRequestCommandTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork<int>>();
            _mockMessageService = new Mock<IMessageService>();
            _mockLogger = new MockLogger<DeleteAdvisoringRequestCommandHandler>();
            _mockAdvisoringRequestRepository = new Mock<IAdvisoringRequestRepositoryAsync>();

            _mockUnitOfWork.Setup(uow => uow.AdvisoringRequestRepository).Returns(_mockAdvisoringRequestRepository.Object);
        }

        [Fact]
        public async Task Handle_WhenRequestExists_ReturnsSuccessResult()
        {
            // Arrange
            var requestId = 1;
            var request = new AdvisoringRequest { Id = requestId };

            _mockAdvisoringRequestRepository
                .Setup(repo => repo.GetByIdAsync(requestId))
                .ReturnsAsync(request);

            _mockAdvisoringRequestRepository
                .Setup(repo => repo.DeleteAsync(request))
                .Returns(Task.CompletedTask);

            _mockMessageService
                .Setup(ms => ms.GetDynamicMessage("AdvisoringRequest", "Messages", "Delete_Success"))
                .Returns("Solicitud eliminada con éxito");

            var handler = new DeleteAdvisoringRequestCommandHandler(
                _mockUnitOfWork.Object,
                _mockMessageService.Object,
                _mockLogger);

            var command = new DeleteAdvisoringRequestCommand { AdvisoringRequestId = requestId };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal("Solicitud eliminada con éxito", result.Messages[0]);
            _mockUnitOfWork.Verify(uow => uow.Commit(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenRequestDoesNotExist_ReturnsFailResult()
        {
            // Arrange
            var requestId = 1;

            _mockAdvisoringRequestRepository
                .Setup(repo => repo.GetByIdAsync(requestId))
                .ReturnsAsync((AdvisoringRequest)null);

            _mockMessageService
                .Setup(ms => ms.GetDynamicMessage("AdvisoringRequest", "Messages", "NotFound"))
                .Returns("Solicitud no encontrada");

            var handler = new DeleteAdvisoringRequestCommandHandler(
                _mockUnitOfWork.Object,
                _mockMessageService.Object,
                _mockLogger);

            var command = new DeleteAdvisoringRequestCommand { AdvisoringRequestId = requestId };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("Solicitud no encontrada", result.Messages[0]);
            _mockUnitOfWork.Verify(uow => uow.Commit(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenExceptionOccurs_ReturnsFailResult()
        {
            // Arrange
            var requestId = 1;

            _mockAdvisoringRequestRepository
                .Setup(repo => repo.GetByIdAsync(requestId))
                .ThrowsAsync(new Exception("Error de prueba"));

            _mockMessageService
                .Setup(ms => ms.GetDynamicMessage("General", "Error", "Exception"))
                .Returns("Ha ocurrido un error");

            var handler = new DeleteAdvisoringRequestCommandHandler(
                _mockUnitOfWork.Object,
                _mockMessageService.Object,
                _mockLogger);

            var command = new DeleteAdvisoringRequestCommand { AdvisoringRequestId = requestId };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("Ha ocurrido un error", result.Messages[0]);
            Assert.Contains(_mockLogger.LogLevels, level => level == LogLevel.Error);
            Assert.NotEmpty(_mockLogger.LogExceptions);
        }
    }
} 