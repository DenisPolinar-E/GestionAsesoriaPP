using AutoMapper;
using GestionAsesoria.Operator.Application.DTOs.DocumentCollection.Request;
using GestionAsesoria.Operator.Application.Features.RequestPPPs.Commands;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Application.Interfaces.Repositories.Identity;
using GestionAsesoria.Operator.Domain.Entities;
using GestionAsesoria.Operator.Shared.Wrapper;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Tsp.Sigescom.Config;
using Xunit;

namespace GestionAsesoria.Operator.Tests.Features.RequestPPPs.Commands
{
    public class CreateRequestPPPCommandHandlerTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IUnitOfWork<int>> _mockUnitOfWork;
        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<IWebHostEnvironment> _mockEnv;
        private readonly Mock<IActorRepositoryAsync> _mockActorRepo;
        private readonly Mock<IRoleRepositoryAsync> _mockRoleRepo;
        private readonly Mock<IActorTypeRepositoryAsync> _mockActorTypeRepo;
        private readonly Mock<IMasterDataValueRepositoryAsync> _mockMDVRepo;
        private readonly Mock<IRequestPPPRepositoryAsync> _mockRequestPPPRepo;
        private readonly Mock<IDocumentCollectionRepositoryAsync> _mockDocumentRepo;

        public CreateRequestPPPCommandHandlerTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockUnitOfWork = new Mock<IUnitOfWork<int>>();
            _mockMediator = new Mock<IMediator>();
            _mockEnv = new Mock<IWebHostEnvironment>();

            _mockActorRepo = new Mock<IActorRepositoryAsync>();
            _mockRoleRepo = new Mock<IRoleRepositoryAsync>();
            _mockActorTypeRepo = new Mock<IActorTypeRepositoryAsync>();
            _mockMDVRepo = new Mock<IMasterDataValueRepositoryAsync>();
            _mockRequestPPPRepo = new Mock<IRequestPPPRepositoryAsync>();
            _mockDocumentRepo = new Mock<IDocumentCollectionRepositoryAsync>();

            _mockUnitOfWork.SetupGet(u => u.ActorRepository).Returns(_mockActorRepo.Object);
            _mockUnitOfWork.SetupGet(u => u.RoleRepository).Returns(_mockRoleRepo.Object);
            _mockUnitOfWork.SetupGet(u => u.ActorTypeRepository).Returns(_mockActorTypeRepo.Object);
            _mockUnitOfWork.SetupGet(u => u.MasterDataValueRepository).Returns(_mockMDVRepo.Object);
            _mockUnitOfWork.SetupGet(u => u.RequestPPPRepository).Returns(_mockRequestPPPRepo.Object);
            _mockUnitOfWork.SetupGet(u => u.DocumentCollectionRepository).Returns(_mockDocumentRepo.Object);
        }

        [Fact]
        public async Task Handle_WhenValidRequestAndFileUploaded_ReturnsSuccessResult()
        {
            // Arrange
            var requestDto = new CreateRequestPPPRequestDto
            {
                StudentDni = "12345678",
                StudentFirstName = "Ana",
                StudentLastName = "Torres",
                StudentCode = "A20210001",
                StudentEmail = "ana@example.com",
                StudentPhone = "987654321",
                StudentGender = "Femenino",
                CompanyRuc = "20123456789",
                CompanyName = "Tech S.A.",
                CompanyAddress = "Av. Perú 123",
                CompanyType = "Privada",
                CompanyRepresentativeDni = "87654321",
                CompanyRepresentativeFirstName = "Carlos",
                CompanyRepresentativeLastName = "Gomez",
                CompanyRepresentativeGender = "Masculino",
                RepresentativeDni = "11223344",
                RepresentativeFirstName = "María",
                RepresentativeLastName = "López",
                RepresentativeGender = "Femenino",
                RepresentativeEmail = "maria@example.com",
                RepresentativePhone = "999999999",
                RepresentativePosition = "Gerente",
                ResearchAreaId = 2,
                Title = "Plan de prácticas"
            };

            var documentDto = new CreateDocumentCollectionRequestDto
            {
                Title = "Plan de prácticas",
                Description = "Plan detallado",
                File = Mock.Of<IFormFile>(file =>
                    file.FileName == "plan.pdf" &&
                    file.Length == 1024 &&
                    file.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()) == Task.CompletedTask)
            };

            var command = new CreateRequestPPPCommand
            {
                Request = requestDto,
                PlanDocument = documentDto
            };

            _mockActorRepo.Setup(r => r.GetByIdentificationNumberAsync(It.IsAny<string>()))
                .ReturnsAsync((Actor)null);
            _mockActorRepo.Setup(r => r.GetByCodeAsync("FAC001"))
                .ReturnsAsync(new Actor { Id = 100 });
            _mockActorRepo.Setup(r => r.AddAsync(It.IsAny<Actor>()))
                .ReturnsAsync((Actor actor) => { actor.Id = new Random().Next(1, 1000); return actor; });

            _mockRoleRepo.Setup(r => r.GetIdByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(1);
            _mockActorTypeRepo.Setup(r => r.GetIdByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(1);
            _mockMDVRepo.Setup(r => r.GetByCodeAsync(It.IsAny<string>()))
                .ReturnsAsync(new MasterDataValue { Id = 1 });

            _mockEnv.Setup(e => e.WebRootPath)
                .Returns(Directory.GetCurrentDirectory());

            _mockDocumentRepo.Setup(r => r.AddAsync(It.IsAny<DocumentCollection>()))
                .ReturnsAsync((DocumentCollection doc) => { doc.Id = 101; return doc; });

            _mockMDVRepo.Setup(r => r.GetByCodeAsync("REQ_PENDING"))
                .ReturnsAsync(new MasterDataValue { Id = 99 });

            _mockRequestPPPRepo.Setup(r => r.AddAsync(It.IsAny<RequestPPP>()))
                .ReturnsAsync((RequestPPP req) => { req.Id = 200; return req; });

            _mockUnitOfWork.Setup(u => u.Commit(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);
            
            var handler = new CreateRequestPPPCommandHandler(
                _mockMapper.Object,
                _mockUnitOfWork.Object,
                _mockMediator.Object,
                _mockEnv.Object
            );
            _mockMapper.Setup(m => m.Map<RequestPPP>(It.IsAny<CreateRequestPPPRequestDto>()))
                .Returns(new RequestPPP());

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal(200, result.Data);
        }
    }
}
