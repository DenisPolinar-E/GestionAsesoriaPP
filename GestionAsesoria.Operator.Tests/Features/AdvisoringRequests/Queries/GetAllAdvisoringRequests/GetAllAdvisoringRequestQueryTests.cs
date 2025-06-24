using AutoMapper;
using GestionAsesoria.Operator.Application.DTOs.AdvisoringRequests.Response;
using GestionAsesoria.Operator.Application.Features.AdvisoringRequests.Queries.GetAllAdvisoringRequests;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Shared.Static;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GestionAsesoria.Operator.Tests.Features.AdvisoringRequests.Queries.GetAllAdvisoringRequests
{
    public class GetAllAdvisoringRequestQueryTests
    {
        private readonly Mock<IUnitOfWork<int>> _mockUnitOfWork;
        private readonly Mock<IAdvisoringRequestRepositoryAsync> _mockAdvisoringRequestRepository;
        private readonly Mock<IMapper> _mockMapper;

        public GetAllAdvisoringRequestQueryTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork<int>>();
            _mockAdvisoringRequestRepository = new Mock<IAdvisoringRequestRepositoryAsync>();
            _mockMapper = new Mock<IMapper>();

            _mockUnitOfWork
                .Setup(uow => uow.AdvisoringRequestRepository)
                .Returns(_mockAdvisoringRequestRepository.Object);
        }

        [Fact]
        public async Task Handle_WhenRequestsExist_ReturnsSuccessResult()
        {
            // Arrange
            var searchString = "test";
            var expectedRequests = new List<GetAllAdvisoringRequestResponse>
            {
                new GetAllAdvisoringRequestResponse
                {
                    Id = 1,
                    UserSubject = "Test Subject 1",
                    UserMessage = "Test Message 1"
                },
                new GetAllAdvisoringRequestResponse
                {
                    Id = 2,
                    UserSubject = "Test Subject 2",
                    UserMessage = "Test Message 2"
                }
            };

            _mockAdvisoringRequestRepository
                .Setup(repo => repo.GetAllAdvisoringRequestsAsync(
                    It.IsAny<string>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<DateTime?>()))
                .ReturnsAsync(expectedRequests);

            var handler = new GetAllAdvisoringRequestQueryHandler(_mockUnitOfWork.Object, _mockMapper.Object);
            var query = new GetAllAdvisoringRequestQuery 
            { 
                parameters = new GetAllAdvisoringRequestParameters 
                { 
                    SearchString = searchString 
                } 
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal(expectedRequests, result.Data);
        }

        [Fact]
        public async Task Handle_WhenNoRequestsExist_ReturnsEmptyList()
        {
            // Arrange
            var emptyRequests = new List<GetAllAdvisoringRequestResponse>();

            _mockAdvisoringRequestRepository
                .Setup(repo => repo.GetAllAdvisoringRequestsAsync(
                    It.IsAny<string>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<DateTime?>()))
                .ReturnsAsync(emptyRequests);

            var handler = new GetAllAdvisoringRequestQueryHandler(_mockUnitOfWork.Object, _mockMapper.Object);
            var query = new GetAllAdvisoringRequestQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task Handle_WithDateRange_ReturnsFilteredResults()
        {
            // Arrange
            var fromDate = DateTime.Today.AddDays(-7);
            var toDate = DateTime.Today;
            var expectedRequests = new List<GetAllAdvisoringRequestResponse>
            {
                new GetAllAdvisoringRequestResponse
                {
                    Id = 1,
                    DateRequest = DateTime.Today.AddDays(-5),
                    UserSubject = "Test Subject 1"
                }
            };

            _mockAdvisoringRequestRepository
                .Setup(repo => repo.GetAllAdvisoringRequestsAsync(
                    It.IsAny<string>(),
                    fromDate,
                    toDate))
                .ReturnsAsync(expectedRequests);

            var handler = new GetAllAdvisoringRequestQueryHandler(_mockUnitOfWork.Object, _mockMapper.Object);
            var query = new GetAllAdvisoringRequestQuery
            {
                parameters = new GetAllAdvisoringRequestParameters
                {
                    FromDate = fromDate,
                    ToDate = toDate
                }
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Single(result.Data);
            Assert.Equal(expectedRequests, result.Data);
        }
    }
} 