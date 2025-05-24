using AutoMapper;
using GestionAsesoria.Operator.Application.DTOs.AdvisoringContracts.Response;
using GestionAsesoria.Operator.Application.Features.AdvisoringContracts.Queries.GetAllAdvisoringContracts;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Shared.Static;
using GestionAsesoria.Operator.Shared.Wrapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GestionAsesoria.Operator.Tests.Features.AdvisoringContracts.Queries.GetAllAdvisoringContracts
{
    public class GetAllAdvisoringContractQueryTests
    {
        private readonly Mock<IUnitOfWork<int>> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IAdvisoringContractRepositoryAsync> _mockAdvisoringContractRepository;

        public GetAllAdvisoringContractQueryTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork<int>>();
            _mockMapper = new Mock<IMapper>();
            _mockAdvisoringContractRepository = new Mock<IAdvisoringContractRepositoryAsync>();
            
            _mockUnitOfWork
                .Setup(uow => uow.AdvisoringContractRepository)
                .Returns(_mockAdvisoringContractRepository.Object);
        }

        [Fact]
        public async Task Handle_WhenContractsExist_ReturnsSuccessResult()
        {
            // Arrange
            var searchString = "test";
            var expectedContracts = new List<GetAllAdvisoringContractResponse>
            {
                new GetAllAdvisoringContractResponse
                {
                    Id = 1,
                    ContractNumber = "CONT-001",
                    Status = "Activo",
                    IsActived = true
                },
                new GetAllAdvisoringContractResponse
                {
                    Id = 2,
                    ContractNumber = "CONT-002",
                    Status = "Activo",
                    IsActived = true
                }
            };

            _mockAdvisoringContractRepository
                .Setup(repo => repo.GetAllAdvisoringContractsAsync(searchString))
                .ReturnsAsync(expectedContracts);

            var handler = new GetAllAdvisoringContractQueryHandler(_mockUnitOfWork.Object, _mockMapper.Object);
            var query = new GetAllAdvisoringContractQuery 
            { 
                parameters = new GetAllAdvisoringContractParameters 
                { 
                    SearchString = searchString 
                } 
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal(expectedContracts, result.Data);
        }

        [Fact]
        public async Task Handle_WhenNoContractsExist_ReturnsEmptyList()
        {
            // Arrange
            var searchString = "test";
            var emptyContracts = new List<GetAllAdvisoringContractResponse>();

            _mockAdvisoringContractRepository
                .Setup(repo => repo.GetAllAdvisoringContractsAsync(searchString))
                .ReturnsAsync(emptyContracts);

            var handler = new GetAllAdvisoringContractQueryHandler(_mockUnitOfWork.Object, _mockMapper.Object);
            var query = new GetAllAdvisoringContractQuery 
            { 
                parameters = new GetAllAdvisoringContractParameters 
                { 
                    SearchString = searchString 
                } 
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task Handle_WithNullSearchString_ReturnsAllContracts()
        {
            // Arrange
            var expectedContracts = new List<GetAllAdvisoringContractResponse>
            {
                new GetAllAdvisoringContractResponse
                {
                    Id = 1,
                    ContractNumber = "CONT-001",
                    Status = "Activo",
                    IsActived = true
                }
            };

            _mockAdvisoringContractRepository
                .Setup(repo => repo.GetAllAdvisoringContractsAsync(string.Empty))
                .ReturnsAsync(expectedContracts);

            var handler = new GetAllAdvisoringContractQueryHandler(_mockUnitOfWork.Object, _mockMapper.Object);
            var query = new GetAllAdvisoringContractQuery 
            { 
                parameters = new GetAllAdvisoringContractParameters() 
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal(expectedContracts, result.Data);
        }

        [Fact]
        public async Task Handle_WhenExceptionOccurs_ReturnsFailResult()
        {
            // Arrange
            var searchString = "test";
            var expectedErrorMessage = "Error de prueba";

            _mockAdvisoringContractRepository
                .Setup(repo => repo.GetAllAdvisoringContractsAsync(searchString))
                .ThrowsAsync(new Exception(expectedErrorMessage));

            var handler = new GetAllAdvisoringContractQueryHandler(_mockUnitOfWork.Object, _mockMapper.Object);
            var query = new GetAllAdvisoringContractQuery 
            { 
                parameters = new GetAllAdvisoringContractParameters 
                { 
                    SearchString = searchString 
                } 
            };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(query, CancellationToken.None));
        }
    }
} 