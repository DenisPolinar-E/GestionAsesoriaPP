using GestionAsesoria.Operator.Application.Features.AdvisoringContracts.Commands.Delete;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Application.Interfaces.Services;
using GestionAsesoria.Operator.Domain.Entities;
using GestionAsesoria.Operator.Shared.Static;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GestionAsesoria.Operator.Tests.Features.AdvisoringContracts.Commands.Delete
{
    public class DeleteAdvisoringContractCommandTests
    {
        private readonly Mock<IUnitOfWork<int>> _mockUnitOfWork;
        private readonly Mock<ICurrentUserService> _mockCurrentUserService;
        private readonly Mock<IAdvisoringContractRepositoryAsync> _mockRepository;

        public DeleteAdvisoringContractCommandTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork<int>>();
            _mockCurrentUserService = new Mock<ICurrentUserService>();
            _mockRepository = new Mock<IAdvisoringContractRepositoryAsync>();

            _mockUnitOfWork
                .Setup(uow => uow.AdvisoringContractRepository)
                .Returns(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_WhenContractExists_ReturnsSuccessResult()
        {
            // Arrange
            var contractId = 1;
            var contract = new AdvisoringContract { Id = contractId };

            _mockRepository
                .Setup(repo => repo.GetByIdAsync(contractId))
                .ReturnsAsync(contract);

            _mockRepository
                .Setup(repo => repo.DeleteAsync(contract))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork
                .Setup(uow => uow.Commit(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var handler = new DeleteAdvisoringContractCommandHandler(_mockUnitOfWork.Object, _mockCurrentUserService.Object);
            var command = new DeleteAdvisoringContractCommand { AdvisoringContractId = contractId };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal(contractId, result.Data);
            Assert.Equal(ReplyMessage.MESSAGE_DELETE, result.Messages[0]);
            _mockRepository.Verify(repo => repo.DeleteAsync(contract), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.Commit(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenContractDoesNotExist_ReturnsFailResult()
        {
            // Arrange
            var contractId = 1;

            _mockRepository
                .Setup(repo => repo.GetByIdAsync(contractId))
                .ReturnsAsync((AdvisoringContract)null);

            var handler = new DeleteAdvisoringContractCommandHandler(_mockUnitOfWork.Object, _mockCurrentUserService.Object);
            var command = new DeleteAdvisoringContractCommand { AdvisoringContractId = contractId };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(ReplyMessage.MESSAGE_QUERY_EMPTY, result.Messages[0]);
            _mockRepository.Verify(repo => repo.DeleteAsync(It.IsAny<AdvisoringContract>()), Times.Never);
            _mockUnitOfWork.Verify(uow => uow.Commit(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenExceptionOccurs_ReturnsFailResult()
        {
            // Arrange
            var contractId = 1;
            var contract = new AdvisoringContract { Id = contractId };

            _mockRepository
                .Setup(repo => repo.GetByIdAsync(contractId))
                .ReturnsAsync(contract);

            _mockRepository
                .Setup(repo => repo.DeleteAsync(contract))
                .ThrowsAsync(new Exception("Error al eliminar"));

            var handler = new DeleteAdvisoringContractCommandHandler(_mockUnitOfWork.Object, _mockCurrentUserService.Object);
            var command = new DeleteAdvisoringContractCommand { AdvisoringContractId = contractId };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(ReplyMessage.MESSAGE_EXCEPTION, result.Messages[0]);
            _mockRepository.Verify(repo => repo.DeleteAsync(contract), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.Commit(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
} 