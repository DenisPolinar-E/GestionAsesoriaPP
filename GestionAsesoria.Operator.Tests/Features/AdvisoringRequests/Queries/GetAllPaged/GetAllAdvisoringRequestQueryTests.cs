using AutoMapper;
using GestionAsesoria.Operator.Application.DTOs.AdvisoringRequests.Response;
using GestionAsesoria.Operator.Application.Features.AdvisoringRequests.Queries.GetAllPaged;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Shared.Static;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GestionAsesoria.Operator.Tests.Features.AdvisoringRequests.Queries.GetAllPaged
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
        public async Task Handle_WithPagination_ReturnsCorrectPage()
        {
            // Arrange
            var allRequests = new List<GetAllAdvisoringRequestResponse>();
            for (int i = 1; i <= 20; i++)
            {
                allRequests.Add(new GetAllAdvisoringRequestResponse
                {
                    Id = i,
                    UserSubject = $"Test Subject {i}",
                    UserMessage = $"Test Message {i}"
                });
            }

            _mockAdvisoringRequestRepository
                .Setup(repo => repo.GetAllAdvisoringRequestsAsync(
                    It.IsAny<string>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<DateTime?>()))
                .ReturnsAsync(allRequests);

            var handler = new GetAllAdvisoringRequestQueryHandler(_mockUnitOfWork.Object, _mockMapper.Object);
            var query = new GetAllAdvisoringRequestQuery 
            { 
                PageNumber = 2,
                PageSize = 5,
                SearchString = ""
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal(5, result.Data.Count);
            Assert.Equal(6, result.Data[0].Id);
            Assert.Equal(ReplyMessage.MESSAGE_QUERY, result.Messages[0]);
        }

        [Fact]
        public async Task Handle_WithSearch_ReturnsFilteredResults()
        {
            // Arrange
            var searchString = "specific";
            var allRequests = new List<GetAllAdvisoringRequestResponse>
            {
                new GetAllAdvisoringRequestResponse
                {
                    Id = 1,
                    UserSubject = "specific test subject",
                    UserMessage = "Test Message 1"
                },
                new GetAllAdvisoringRequestResponse
                {
                    Id = 2,
                    UserSubject = "other subject",
                    UserMessage = "Test Message 2"
                }
            };

            _mockAdvisoringRequestRepository
                .Setup(repo => repo.GetAllAdvisoringRequestsAsync(
                    searchString,
                    It.IsAny<DateTime?>(),
                    It.IsAny<DateTime?>()))
                .ReturnsAsync(allRequests.FindAll(r => r.UserSubject.Contains(searchString)));

            var handler = new GetAllAdvisoringRequestQueryHandler(_mockUnitOfWork.Object, _mockMapper.Object);
            var query = new GetAllAdvisoringRequestQuery 
            { 
                PageNumber = 1,
                PageSize = 10,
                SearchString = searchString
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Single(result.Data);
            Assert.Contains(result.Data, r => r.UserSubject.Contains(searchString));
            Assert.Equal(ReplyMessage.MESSAGE_QUERY, result.Messages[0]);
        }

        [Fact]
        public async Task Handle_WithEmptyResults_ReturnsFailResult()
        {
            // Arrange
            var emptyList = new List<GetAllAdvisoringRequestResponse>();

            _mockAdvisoringRequestRepository
                .Setup(repo => repo.GetAllAdvisoringRequestsAsync(
                    It.IsAny<string>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<DateTime?>()))
                .ReturnsAsync(emptyList);

            var handler = new GetAllAdvisoringRequestQueryHandler(_mockUnitOfWork.Object, _mockMapper.Object);
            var query = new GetAllAdvisoringRequestQuery 
            { 
                PageNumber = 1,
                PageSize = 10
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Empty(result.Data);
            Assert.Equal(ReplyMessage.MESSAGE_QUERY_EMPTY, result.Messages[0]);
        }

        [Fact]
        public async Task Handle_WithPageBeyondResults_ReturnsFailResult()
        {
            // Arrange
            var allRequests = new List<GetAllAdvisoringRequestResponse>
            {
                new GetAllAdvisoringRequestResponse { Id = 1 },
                new GetAllAdvisoringRequestResponse { Id = 2 }
            };

            _mockAdvisoringRequestRepository
                .Setup(repo => repo.GetAllAdvisoringRequestsAsync(
                    It.IsAny<string>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<DateTime?>()))
                .ReturnsAsync(allRequests);

            var handler = new GetAllAdvisoringRequestQueryHandler(_mockUnitOfWork.Object, _mockMapper.Object);
            var query = new GetAllAdvisoringRequestQuery 
            { 
                PageNumber = 3,
                PageSize = 1
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Empty(result.Data);
            Assert.Equal(ReplyMessage.MESSAGE_QUERY_EMPTY, result.Messages[0]);
        }
    }
} 