using AutoMapper;
using GestionAsesoria.Operator.Application.DTOs.AdvisoringRequests.Response;
using GestionAsesoria.Operator.Application.Features.AdvisoringRequests.Queries.GetByIdAdvisoringRequests;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Domain.Entities;
using GestionAsesoria.Operator.Shared.Static;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GestionAsesoria.Operator.Tests.Features.AdvisoringRequests.Queries.GetByIdAdvisoringRequests
{
    public class GetAdvisoringRequestByIdQueryTests
    {
        private readonly Mock<IUnitOfWork<int>> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IAdvisoringRequestRepositoryAsync> _mockAdvisoringRequestRepository;

        public GetAdvisoringRequestByIdQueryTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork<int>>();
            _mockMapper = new Mock<IMapper>();
            _mockAdvisoringRequestRepository = new Mock<IAdvisoringRequestRepositoryAsync>();
            
            _mockUnitOfWork
                .Setup(uow => uow.AdvisoringRequestRepository)
                .Returns(_mockAdvisoringRequestRepository.Object);
        }

        [Fact]
        public async Task Handle_WhenRequestExists_ReturnsSuccessResult()
        {
            // Arrange
            var requestId = 1;
            var request = new AdvisoringRequest
            {
                Id = requestId,
                UserSubject = "Test Subject",
                UserMessage = "Test Message"
            };

            var expectedResponse = new AdvisoringRequestByIdResponseDto
            {
                AdvisoringRequestId = requestId,
                UserMessage = "Test Message",
                DateRequest = DateTime.Now,
                DateResponseAdvisor = null,
                ResponseAdvisor = null,
                AdvisoringRequestStatus = 0,
                ServiceTypeId = 0,
                AdvisorActorId = 0,
                UserActorId = 0,
                RequesterActorId = 0,
                IsActive = true
            };

            _mockAdvisoringRequestRepository
                .Setup(repo => repo.GetByIdAsync(requestId))
                .ReturnsAsync(request);

            _mockMapper
                .Setup(m => m.Map<AdvisoringRequestByIdResponseDto>(request))
                .Returns(expectedResponse);

            var handler = new GetAdvisoringRequestByIdQueryHandler(_mockUnitOfWork.Object, _mockMapper.Object);
            var query = new GetAdvisoringRequestByIdQuery { advisoringRequestId = requestId };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal(expectedResponse, result.Data);
            Assert.Equal(ReplyMessage.MESSAGE_QUERY, result.Messages[0]);
        }

        [Fact]
        public async Task Handle_WhenRequestDoesNotExist_ReturnsFailResult()
        {
            // Arrange
            var requestId = 1;

            _mockAdvisoringRequestRepository
                .Setup(repo => repo.GetByIdAsync(requestId))
                .ReturnsAsync((AdvisoringRequest)null);

            var handler = new GetAdvisoringRequestByIdQueryHandler(_mockUnitOfWork.Object, _mockMapper.Object);
            var query = new GetAdvisoringRequestByIdQuery { advisoringRequestId = requestId };

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
            var requestId = 1;

            _mockAdvisoringRequestRepository
                .Setup(repo => repo.GetByIdAsync(requestId))
                .ThrowsAsync(new Exception("Error de prueba"));

            var handler = new GetAdvisoringRequestByIdQueryHandler(_mockUnitOfWork.Object, _mockMapper.Object);
            var query = new GetAdvisoringRequestByIdQuery { advisoringRequestId = requestId };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(ReplyMessage.MESSAGE_EXCEPTION, result.Messages[0]);
        }
    }
} 