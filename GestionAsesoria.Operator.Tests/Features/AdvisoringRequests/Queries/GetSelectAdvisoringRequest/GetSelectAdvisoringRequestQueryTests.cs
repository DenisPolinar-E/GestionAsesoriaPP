using AutoMapper;
using GestionAsesoria.Operator.Application.DTOs.AdvisoringRequest.Response;
using GestionAsesoria.Operator.Application.Features.AdvisoringRequests.Queries.GetSelectAdvisoringRequest;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Domain.Entities;
using GestionAsesoria.Operator.Domain.Enums;
using GestionAsesoria.Operator.Shared.Static;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GestionAsesoria.Operator.Tests.Features.AdvisoringRequests.Queries.GetSelectAdvisoringRequest
{
    public class GetSelectAdvisoringRequestQueryTests
    {
        private readonly Mock<IUnitOfWork<int>> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IAdvisoringRequestRepositoryAsync> _mockAdvisoringRequestRepository;

        public GetSelectAdvisoringRequestQueryTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork<int>>();
            _mockMapper = new Mock<IMapper>();
            _mockAdvisoringRequestRepository = new Mock<IAdvisoringRequestRepositoryAsync>();
            
            _mockUnitOfWork
                .Setup(uow => uow.AdvisoringRequestRepository)
                .Returns(_mockAdvisoringRequestRepository.Object);
        }

        [Fact]
        public async Task Handle_WhenPendingRequestsExist_ReturnsSuccessResult()
        {
            // Arrange
            var pendingRequests = new List<AdvisoringRequest>
            {
                new AdvisoringRequest
                {
                    Id = 1,
                    UserSubject = "Test Subject 1",
                    UserMessage = "Test Message 1",
                    AdvisoringRequestStatus = AdvisoringRequestStatus.PendingRequest
                },
                new AdvisoringRequest
                {
                    Id = 2,
                    UserSubject = "Test Subject 2",
                    UserMessage = "Test Message 2",
                    AdvisoringRequestStatus = AdvisoringRequestStatus.PendingRequest
                }
            };

            var expectedResponse = new List<AdvisoringRequestSelectResponseDto>
            {
                new AdvisoringRequestSelectResponseDto
                {
                    AdvisoringRequestId = 1,
                    UserMessage = "Test Message 1",
                    AdvisoringRequestStatus = (int)AdvisoringRequestStatus.PendingRequest
                },
                new AdvisoringRequestSelectResponseDto
                {
                    AdvisoringRequestId = 2,
                    UserMessage = "Test Message 2",
                    AdvisoringRequestStatus = (int)AdvisoringRequestStatus.PendingRequest
                }
            };

            _mockAdvisoringRequestRepository
                .Setup(repo => repo.GetPendingAdvisoringRequestsAsync())
                .ReturnsAsync(pendingRequests);

            _mockMapper
                .Setup(m => m.Map<IEnumerable<AdvisoringRequestSelectResponseDto>>(pendingRequests))
                .Returns(expectedResponse);

            var handler = new GetSelectAdvisoringRequestQueryHandler(_mockUnitOfWork.Object, _mockMapper.Object);
            var query = new GetSelectAdvisoringRequestQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal(expectedResponse, result.Data);
            Assert.Equal(ReplyMessage.MESSAGE_QUERY, result.Messages[0]);
        }

        [Fact]
        public async Task Handle_WhenNoPendingRequestsExist_ReturnsFailResult()
        {
            // Arrange
            var emptyRequests = new List<AdvisoringRequest>();
            var emptyResponse = new List<AdvisoringRequestSelectResponseDto>();

            _mockAdvisoringRequestRepository
                .Setup(repo => repo.GetPendingAdvisoringRequestsAsync())
                .ReturnsAsync(emptyRequests);

            _mockMapper
                .Setup(m => m.Map<IEnumerable<AdvisoringRequestSelectResponseDto>>(emptyRequests))
                .Returns(emptyResponse);

            var handler = new GetSelectAdvisoringRequestQueryHandler(_mockUnitOfWork.Object, _mockMapper.Object);
            var query = new GetSelectAdvisoringRequestQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Empty(result.Data);
            Assert.Equal(ReplyMessage.MESSAGE_QUERY_EMPTY, result.Messages[0]);
        }

        [Fact]
        public async Task Handle_WhenExceptionOccurs_ReturnsFailResult()
        {
            // Arrange
            _mockAdvisoringRequestRepository
                .Setup(repo => repo.GetPendingAdvisoringRequestsAsync())
                .ThrowsAsync(new Exception("Error de prueba"));

            var handler = new GetSelectAdvisoringRequestQueryHandler(_mockUnitOfWork.Object, _mockMapper.Object);
            var query = new GetSelectAdvisoringRequestQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Empty(result.Data);
            Assert.Equal(ReplyMessage.MESSAGE_EXCEPTION, result.Messages[0]);
        }
    }
} 