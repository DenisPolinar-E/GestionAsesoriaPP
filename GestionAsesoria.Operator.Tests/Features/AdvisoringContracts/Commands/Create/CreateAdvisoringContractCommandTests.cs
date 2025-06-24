using AutoMapper;
using GestionAsesoria.Operator.Application.DTOs.AdvisoringContracts.Request;
using GestionAsesoria.Operator.Application.Features.AdvisoringContracts.Commands.Create;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Application.Interfaces.Services;
using GestionAsesoria.Operator.Application.Interfaces.Services.ExternalRequest;
using GestionAsesoria.Operator.Domain.Entities;
using GestionAsesoria.Operator.Domain.Enums;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GestionAsesoria.Operator.Tests.Features.AdvisoringContracts.Commands.Create
{
    public class CreateAdvisoringContractCommandTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IUnitOfWork<int>> _mockUnitOfWork;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly Mock<IMessageService> _mockMessageService;
        private readonly Mock<IActorRepositoryAsync> _mockActorRepository;
        private readonly Mock<IMasterDataValueRepositoryAsync> _mockMasterDataValueRepository;
        private readonly Mock<IAdvisoringContractRepositoryAsync> _mockAdvisoringContractRepository;
        private readonly Mock<IAdvisoringRequestRepositoryAsync> _mockAdvisoringRequestRepository;

        public CreateAdvisoringContractCommandTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockUnitOfWork = new Mock<IUnitOfWork<int>>();
            _mockEmailService = new Mock<IEmailService>();
            _mockMessageService = new Mock<IMessageService>();
            _mockActorRepository = new Mock<IActorRepositoryAsync>();
            _mockMasterDataValueRepository = new Mock<IMasterDataValueRepositoryAsync>();
            _mockAdvisoringContractRepository = new Mock<IAdvisoringContractRepositoryAsync>();
            _mockAdvisoringRequestRepository = new Mock<IAdvisoringRequestRepositoryAsync>();

            _mockUnitOfWork.Setup(uow => uow.ActorRepository).Returns(_mockActorRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.MasterDataValueRepository).Returns(_mockMasterDataValueRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.AdvisoringContractRepository).Returns(_mockAdvisoringContractRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.AdvisoringRequestRepository).Returns(_mockAdvisoringRequestRepository.Object);
        }

        [Fact]
        public async Task Handle_WhenValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var request = new CreateAdvisoringContractRequestDto
            {
                EstudianteId = 1,
                DocenteId = 2,
                ResearchGroupId = 3,
                ResearchLineId = 4,
                ResearchAreaId = 5,
                Subject = "Test Subject",
                Description = "Test Description"
            };

            var student = new Actor { Id = 1, MainRoleId = 12, FirstName = "Student", Email = "student@test.com" };
            var advisor = new Actor { Id = 2, MainRoleId = 15, FirstName = "Advisor", Email = "advisor@test.com" };
            var researchGroup = new Actor { Id = 3 };
            var serviceType = new MasterDataValue { Id = 1, Code = "TESIS" };

            _mockActorRepository.Setup(r => r.GetActorWithDetailsAsync(1))
                .ReturnsAsync(student);
            _mockActorRepository.Setup(r => r.GetActorWithDetailsAsync(2))
                .ReturnsAsync(advisor);
            _mockActorRepository.Setup(r => r.GetResearchGroupWithDetailsAsync(3, 4, 5, 2))
                .ReturnsAsync(researchGroup);
            _mockMasterDataValueRepository.Setup(r => r.GetByCodeAsync("TESIS"))
                .ReturnsAsync(serviceType);
            _mockAdvisoringContractRepository.Setup(r => r.GetContractsByActorIdAsync(1))
                .ReturnsAsync(new List<AdvisoringContract>());

            var addedRequest = new AdvisoringRequest { Id = 1 };
            _mockAdvisoringRequestRepository.Setup(r => r.AddAsync(It.IsAny<AdvisoringRequest>()))
                .ReturnsAsync(addedRequest);

            _mockUnitOfWork.Setup(uow => uow.ExecuteInTransactionAsync(
                It.IsAny<Func<Task<AdvisoringRequest>>>(),
                It.IsAny<System.Data.IsolationLevel>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(addedRequest);

            _mockMessageService.Setup(m => m.GetMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("test@test.com");

            var handler = new CreateAdvisoringContractCommandHandler(
                _mockMapper.Object,
                _mockUnitOfWork.Object,
                _mockEmailService.Object,
                _mockMessageService.Object);

            var command = new CreateAdvisoringContractCommand { Request = request };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal(1, result.Data);
        }

        [Fact]
        public async Task Handle_WhenStudentHasActiveContract_ReturnsFailResult()
        {
            // Arrange
            var request = new CreateAdvisoringContractRequestDto
            {
                EstudianteId = 1,
                DocenteId = 2,
                ResearchGroupId = 3,
                ResearchLineId = 4,
                ResearchAreaId = 5
            };

            var activeContract = new AdvisoringContract { Id = 1, IsActived = true };
            _mockAdvisoringContractRepository.Setup(r => r.GetContractsByActorIdAsync(1))
                .ReturnsAsync(new List<AdvisoringContract> { activeContract });

            _mockMessageService.Setup(m => m.GetDynamicMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("Error: Contrato activo existente");

            var handler = new CreateAdvisoringContractCommandHandler(
                _mockMapper.Object,
                _mockUnitOfWork.Object,
                _mockEmailService.Object,
                _mockMessageService.Object);

            var command = new CreateAdvisoringContractCommand { Request = request };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Contains("Error: Contrato activo existente", result.Messages[0]);
        }

        [Fact]
        public async Task Handle_WhenStudentNotFound_ReturnsFailResult()
        {
            // Arrange
            var request = new CreateAdvisoringContractRequestDto
            {
                EstudianteId = 1,
                DocenteId = 2,
                ResearchGroupId = 3,
                ResearchLineId = 4,
                ResearchAreaId = 5
            };

            _mockActorRepository.Setup(r => r.GetActorWithDetailsAsync(1))
                .ReturnsAsync((Actor)null);

            _mockAdvisoringContractRepository.Setup(r => r.GetContractsByActorIdAsync(1))
                .ReturnsAsync(new List<AdvisoringContract>());

            var handler = new CreateAdvisoringContractCommandHandler(
                _mockMapper.Object,
                _mockUnitOfWork.Object,
                _mockEmailService.Object,
                _mockMessageService.Object);

            var command = new CreateAdvisoringContractCommand { Request = request };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Contains("Estudiante con ID 1 no encontrado", result.Messages[0]);
        }

        [Fact]
        public async Task Handle_WhenAdvisorNotFound_ReturnsFailResult()
        {
            // Arrange
            var request = new CreateAdvisoringContractRequestDto
            {
                EstudianteId = 1,
                DocenteId = 2,
                ResearchGroupId = 3,
                ResearchLineId = 4,
                ResearchAreaId = 5
            };

            var student = new Actor { Id = 1, MainRoleId = 12 };
            _mockActorRepository.Setup(r => r.GetActorWithDetailsAsync(1))
                .ReturnsAsync(student);
            _mockActorRepository.Setup(r => r.GetActorWithDetailsAsync(2))
                .ReturnsAsync((Actor)null);

            _mockAdvisoringContractRepository.Setup(r => r.GetContractsByActorIdAsync(1))
                .ReturnsAsync(new List<AdvisoringContract>());

            var handler = new CreateAdvisoringContractCommandHandler(
                _mockMapper.Object,
                _mockUnitOfWork.Object,
                _mockEmailService.Object,
                _mockMessageService.Object);

            var command = new CreateAdvisoringContractCommand { Request = request };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Contains("Docente con ID 2 no encontrado", result.Messages[0]);
        }
    }
} 