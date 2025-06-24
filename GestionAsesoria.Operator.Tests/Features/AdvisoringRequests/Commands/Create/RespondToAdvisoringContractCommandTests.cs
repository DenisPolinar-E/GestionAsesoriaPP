using AutoMapper;
using GestionAsesoria.Operator.Application.DTOs.AdvisoringRequest.Request;
using GestionAsesoria.Operator.Application.DTOs.Mail.Request;
using GestionAsesoria.Operator.Application.Features.AdvisoringRequests.Commands.Create;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Application.Interfaces.Services;
using GestionAsesoria.Operator.Application.Interfaces.Services.ExternalRequest;
using GestionAsesoria.Operator.Domain.Entities;
using GestionAsesoria.Operator.Domain.Enums;
using GestionAsesoria.Operator.Tests.Helpers;
using Microsoft.Extensions.Logging;
using Moq;

namespace GestionAsesoria.Operator.Tests.Features.AdvisoringRequests.Commands.Create
{
    public class RespondToAdvisoringContractCommandTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IUnitOfWork<int>> _mockUnitOfWork;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly Mock<IMessageService> _mockMessageService;
        private readonly MockLogger<RespondToAdvisoringContractCommandHandler> _mockLogger;
        private readonly Mock<IAdvisoringRequestRepositoryAsync> _mockAdvisoringRequestRepository;
        private readonly Mock<IActorRepositoryAsync> _mockActorRepository;
        private readonly Mock<IMasterDataValueRepositoryAsync> _mockMasterDataValueRepository;
        private readonly Mock<IAdvisoringContractRepositoryAsync> _mockAdvisoringContractRepository;

        public RespondToAdvisoringContractCommandTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockUnitOfWork = new Mock<IUnitOfWork<int>>();
            _mockEmailService = new Mock<IEmailService>();
            _mockMessageService = new Mock<IMessageService>();
            _mockLogger = new MockLogger<RespondToAdvisoringContractCommandHandler>();
            _mockAdvisoringRequestRepository = new Mock<IAdvisoringRequestRepositoryAsync>();
            _mockActorRepository = new Mock<IActorRepositoryAsync>();
            _mockMasterDataValueRepository = new Mock<IMasterDataValueRepositoryAsync>();
            _mockAdvisoringContractRepository = new Mock<IAdvisoringContractRepositoryAsync>();

            _mockUnitOfWork.Setup(uow => uow.AdvisoringRequestRepository).Returns(_mockAdvisoringRequestRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.ActorRepository).Returns(_mockActorRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.MasterDataValueRepository).Returns(_mockMasterDataValueRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.AdvisoringContractRepository).Returns(_mockAdvisoringContractRepository.Object);
        }

        private void SetupMocks(AdvisoringRequest request, Actor advisor, Actor student, Actor researchGroup)
        {
            // Setup AdvisoringRequest repository
            _mockAdvisoringRequestRepository
                .Setup(repo => repo.GetByIdAsync(request.Id))
                .ReturnsAsync(request);

            _mockAdvisoringRequestRepository
                .Setup(repo => repo.UpdateAsync(It.IsAny<AdvisoringRequest>()))
                .Returns(Task.CompletedTask);

            // Setup Actor repository
            _mockActorRepository
                .Setup(repo => repo.GetByIdAsync(advisor.Id))
                .ReturnsAsync(advisor);

            _mockActorRepository
                .Setup(repo => repo.GetByIdAsync(student.Id))
                .ReturnsAsync(student);

            _mockActorRepository
                .Setup(repo => repo.GetActorByIdWithDetailsAsync(student.Id))
                .ReturnsAsync(student);

            _mockActorRepository
                .Setup(repo => repo.GetResearchGroupByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(researchGroup);

            _mockActorRepository
                .Setup(repo => repo.GetActorsByRoleAndStatusAsync(15, true))
                .ReturnsAsync(new List<Actor> { advisor });

            _mockActorRepository
                .Setup(repo => repo.GetActiveMembershipsByAdvisorAsync(advisor.Id))
                .ReturnsAsync(new List<Membership> { new Membership { OrganizationActorId = researchGroup.Id } });

            _mockActorRepository
                .Setup(repo => repo.UpdateAsync(It.IsAny<Actor>()))
                .Returns(Task.CompletedTask);

            // Setup MasterDataValue repository
            _mockMasterDataValueRepository
                .Setup(repo => repo.GetByCodeAsync("ACCEPTED"))
                .ReturnsAsync(new MasterDataValue { Id = 1 });

            _mockMasterDataValueRepository
                .Setup(repo => repo.GetByCodeAsync("REFUSED"))
                .ReturnsAsync(new MasterDataValue { Id = 3 });

            _mockMasterDataValueRepository
                .Setup(repo => repo.GetByCodeAsync("TESIS"))
                .ReturnsAsync(new MasterDataValue { Id = 2 });

            // Setup AdvisoringContract repository
            _mockAdvisoringContractRepository
                .Setup(repo => repo.AddAsync(It.IsAny<AdvisoringContract>()))
                .ReturnsAsync(new AdvisoringContract { Id = 1 });

            _mockAdvisoringContractRepository
                .Setup(repo => repo.GetByAdvisoringRequestIdAsync(It.IsAny<int>()))
                .ReturnsAsync((AdvisoringContract)null);

            // Setup UnitOfWork transaction and commit
            _mockUnitOfWork
                .Setup(uow => uow.Commit(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Setup MessageService
            _mockMessageService
                .Setup(ms => ms.GetMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("Test message template");

            // Setup EmailService
            _mockEmailService
                .Setup(es => es.SendEmailAsync(It.IsAny<MailRequest>()))
                .Returns(Task.CompletedTask);

            // Setup Mapper
            _mockMapper
                .Setup(m => m.Map(It.IsAny<RespondToAdvisoringContractRequestDto>(), It.IsAny<AdvisoringRequest>()))
                .Returns(request);
        }

        [Fact]
        public async Task Handle_WhenExceptionOccurs_ReturnsFailResult()
        {
            // Arrange
            var requestId = 1;
            var requestDto = new RespondToAdvisoringContractRequestDto
            {
                AdvisoringRequestId = requestId,
                ResponseStatus = AdvisoringRequestStatus.Accepted,
                ResponseAdvisor = "Accepted"
            };

            _mockAdvisoringRequestRepository
                .Setup(repo => repo.GetByIdAsync(requestId))
                .ThrowsAsync(new Exception("Error de prueba"));

            _mockUnitOfWork
                .Setup(uow => uow.ExecuteInTransactionAsync(
                    It.IsAny<Func<Task<bool>>>(),
                    It.IsAny<System.Data.IsolationLevel>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Error de prueba"));

            var handler = new RespondToAdvisoringContractCommandHandler(
                _mockMapper.Object,
                _mockUnitOfWork.Object,
                _mockEmailService.Object,
                _mockMessageService.Object,
                _mockLogger);

            var command = new RespondToAdvisoringContractCommand { Request = requestDto };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Contains(_mockLogger.LogLevels, level => level == LogLevel.Error);
            Assert.NotEmpty(_mockLogger.LogExceptions);
            _mockUnitOfWork.Verify(uow => uow.Commit(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenRequestIsAccepted_ReturnsSuccessResult()
        {
            // Arrange
            var requestId = 1;
            var advisorId = 2;
            var request = new AdvisoringRequest
            {
                Id = requestId,
                AdvisorActorId = advisorId,
                RequesterActorId = 3,
                UserSubject = "Asesoría académica",
                UserMessage = "Quisiera que fuera mi asesor",
                ResearchGroupId = 4,
                ResearchLineId = 5,
                ResearchAreaId = 6,
                ServiceTypeId = 2
            };

            var advisor = new Actor { Id = advisorId };
            var student = new Actor { Id = 3, Email = "student@test.com", MainRoleId = 12 };
            var researchGroup = new Actor { Id = 4 };

            var requestDto = new RespondToAdvisoringContractRequestDto
            {
                AdvisoringRequestId = requestId,
                ResponseStatus = AdvisoringRequestStatus.Accepted,
                ResponseAdvisor = "Accepted",
                DateResponseAdvisor = DateTime.Now
            };

            SetupMocks(request, advisor, student, researchGroup);

            _mockUnitOfWork
                .Setup(uow => uow.ExecuteInTransactionAsync(
                    It.IsAny<Func<Task<bool>>>(),
                    It.IsAny<System.Data.IsolationLevel>(),
                    It.IsAny<CancellationToken>()))
                .Returns<Func<Task<bool>>, System.Data.IsolationLevel, CancellationToken>(
                    async (func, level, token) =>
                    {
                        var result = await func();
                        if (result)
                        {
                            await _mockUnitOfWork.Object.Commit(token);
                        }
                        return result;
                    });

            var handler = new RespondToAdvisoringContractCommandHandler(
                _mockMapper.Object,
                _mockUnitOfWork.Object,
                _mockEmailService.Object,
                _mockMessageService.Object,
                _mockLogger);

            var command = new RespondToAdvisoringContractCommand { Request = requestDto };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            _mockUnitOfWork.Verify(uow => uow.Commit(It.IsAny<CancellationToken>()), Times.Once);
            _mockEmailService.Verify(es => es.SendEmailAsync(It.IsAny<MailRequest>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenRequestIsRefused_ReturnsSuccessResult()
        {
            // Arrange
            var requestId = 1;
            var advisorId = 2;
            var request = new AdvisoringRequest
            {
                Id = requestId,
                AdvisorActorId = advisorId,
                RequesterActorId = 3,
                UserSubject = "Test Subject",
                UserMessage = "Test Message",
                ResearchGroupId = 4,
                ResearchLineId = 5,
                ResearchAreaId = 6,
                ServiceTypeId = 2
            };

            var advisor = new Actor { Id = advisorId };
            var student = new Actor { Id = 3, Email = "student@test.com", MainRoleId = 12 };
            var researchGroup = new Actor { Id = 4 };

            var requestDto = new RespondToAdvisoringContractRequestDto
            {
                AdvisoringRequestId = requestId,
                ResponseStatus = AdvisoringRequestStatus.Refused,
                ResponseAdvisor = "Refused",
                DateResponseAdvisor = DateTime.Now
            };

            SetupMocks(request, advisor, student, researchGroup);

            _mockUnitOfWork
                .Setup(uow => uow.ExecuteInTransactionAsync(
                    It.IsAny<Func<Task<bool>>>(),
                    It.IsAny<System.Data.IsolationLevel>(),
                    It.IsAny<CancellationToken>()))
                .Returns<Func<Task<bool>>, System.Data.IsolationLevel, CancellationToken>(
                    async (func, level, token) =>
                    {
                        var result = await func();
                        if (result)
                        {
                            await _mockUnitOfWork.Object.Commit(token);
                        }
                        return result;
                    });

            var handler = new RespondToAdvisoringContractCommandHandler(
                _mockMapper.Object,
                _mockUnitOfWork.Object,
                _mockEmailService.Object,
                _mockMessageService.Object,
                _mockLogger);

            var command = new RespondToAdvisoringContractCommand { Request = requestDto };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            _mockUnitOfWork.Verify(uow => uow.Commit(It.IsAny<CancellationToken>()), Times.Once);
            _mockEmailService.Verify(es => es.SendEmailAsync(It.IsAny<MailRequest>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenRequestDoesNotExist_ReturnsFailResult()
        {
            // Arrange
            var requestId = 1;
            var requestDto = new RespondToAdvisoringContractRequestDto
            {
                AdvisoringRequestId = requestId,
                ResponseStatus = AdvisoringRequestStatus.Accepted,
                ResponseAdvisor = "Accepted"
            };

            _mockAdvisoringRequestRepository
                .Setup(repo => repo.GetByIdAsync(requestId))
                .ReturnsAsync((AdvisoringRequest)null);

            var handler = new RespondToAdvisoringContractCommandHandler(
                _mockMapper.Object,
                _mockUnitOfWork.Object,
                _mockEmailService.Object,
                _mockMessageService.Object,
                _mockLogger);

            var command = new RespondToAdvisoringContractCommand { Request = requestDto };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            _mockUnitOfWork.Verify(uow => uow.Commit(It.IsAny<CancellationToken>()), Times.Never);
            _mockEmailService.Verify(es => es.SendEmailAsync(It.IsAny<MailRequest>()), Times.Never);
        }
    }
}