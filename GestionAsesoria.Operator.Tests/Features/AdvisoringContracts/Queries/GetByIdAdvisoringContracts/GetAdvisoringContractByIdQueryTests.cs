using AutoMapper;
using GestionAsesoria.Operator.Application.DTOs.AdvisoringContracts.Response;
using GestionAsesoria.Operator.Application.Features.AdvisoringContracts.Queries.GetByIdAdvisoringContracts;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Domain.Entities;
using GestionAsesoria.Operator.Shared.Static;
using GestionAsesoria.Operator.Shared.Wrapper;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GestionAsesoria.Operator.Tests.Features.AdvisoringContracts.Queries.GetByIdAdvisoringContracts
{
    public class GetAdvisoringContractByIdQueryTests
    {
        private readonly Mock<IUnitOfWork<int>> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IGenericRepositoryAsync<AdvisoringContract, int>> _mockRepository;

        public GetAdvisoringContractByIdQueryTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork<int>>();
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<IGenericRepositoryAsync<AdvisoringContract, int>>();
            
            _mockUnitOfWork
                .Setup(uow => uow.Repository<AdvisoringContract>())
                .Returns(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_WhenContractExists_ReturnsSuccessResult()
        {
            // Arrange
            var contractId = 1;
            var contract = new AdvisoringContract
            {
                Id = contractId,
                Subject = "Test Subject",
                Description = "Test Description",
                IsActived = true
            };

            var expectedResponse = new AdvisoringContractByIdResponseDto
            {
                AdvisoringContractId = contractId,
                Subject = "Test Subject",
                Description = "Test Description",
                IsActive = true
            };

            _mockRepository
                .Setup(repo => repo.GetByIdAsync(contractId))
                .ReturnsAsync(contract);

            _mockMapper
                .Setup(m => m.Map<AdvisoringContractByIdResponseDto>(contract))
                .Returns(expectedResponse);

            var handler = new GetAdvisoringContractByIdQueryHandler(_mockUnitOfWork.Object, _mockMapper.Object);
            var query = new GetAdvisoringContractByIdQuery { advisoringContractId = contractId };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal(expectedResponse, result.Data);
            Assert.Equal(ReplyMessage.MESSAGE_QUERY, result.Messages[0]);
        }

        [Fact]
        public async Task Handle_WhenContractDoesNotExist_ReturnsFailResult()
        {
            // Arrange
            var contractId = 1;

            _mockRepository
                .Setup(repo => repo.GetByIdAsync(contractId))
                .ReturnsAsync((AdvisoringContract)null);

            var handler = new GetAdvisoringContractByIdQueryHandler(_mockUnitOfWork.Object, _mockMapper.Object);
            var query = new GetAdvisoringContractByIdQuery { advisoringContractId = contractId };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(ReplyMessage.MESSAGE_QUERY_EMPTY, result.Messages[0]);
        }

        [Fact]
        public async Task Handle_WhenExceptionOccurs_ReturnsFailResult()
        {
            // Arrange
            var contractId = 1;

            _mockRepository
                .Setup(repo => repo.GetByIdAsync(contractId))
                .ThrowsAsync(new Exception("Error de prueba"));

            var handler = new GetAdvisoringContractByIdQueryHandler(_mockUnitOfWork.Object, _mockMapper.Object);
            var query = new GetAdvisoringContractByIdQuery { advisoringContractId = contractId };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(ReplyMessage.MESSAGE_EXCEPTION, result.Messages[0]);
        }
    }
} 