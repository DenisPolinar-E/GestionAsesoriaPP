using AutoMapper;
using GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternship.Response;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Application.Interfaces.Services;
using GestionAsesoria.Operator.Application.Services;
using GestionAsesoria.Operator.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace GestionAsesoria.Operator.Tests.Services
{
    public class PreProfessionalInternshipServiceTests_GetAll
    {
        private readonly Mock<IPreProfessionalInternshipRepositoryAsync> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IPreProfessionalInternshipService _service;

        public PreProfessionalInternshipServiceTests_GetAll()
        {
            _repositoryMock = new Mock<IPreProfessionalInternshipRepositoryAsync>();
            _mapperMock = new Mock<IMapper>();
            _service = new PreProfessionalInternshipService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllInternships()
        {
            // Arrange
            var internships = new List<PreProfessionalInternship>
            {
                new PreProfessionalInternship
                {
                    Id = 1,
                    CodeStudentId = "STU001",
                    FullNameInterId = "John Doe",
                    FullNameAdviserId = "Jane Smith",
                    FullNameCompanyId = "Acme Corp",
                    Career = "Computer Science",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(30),
                    State = "Pendiente",
                    Note = 15.5m
                },
                new PreProfessionalInternship
                {
                    Id = 2,
                    CodeStudentId = "STU002",
                    FullNameInterId = "Alice Brown",
                    FullNameAdviserId = "Bob Wilson",
                    FullNameCompanyId = "Tech Ltd",
                    Career = "Engineering",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(60),
                    State = "EnCurso",
                    Note = 18.0m
                }
            };

            var expectedDtos = new List<PreProfessionalInternshipDto>
            {
                new PreProfessionalInternshipDto
                {
                    CodeStudentId = "STU001",
                    FullNameInterId = "John Doe",
                    FullNameAdviserId = "Jane Smith",
                    FullNameCompanyId = "Acme Corp",
                    Career = "Computer Science",
                    StartDate = internships[0].StartDate,
                    EndDate = internships[0].EndDate,
                    State = "Pendiente",
                    Note = "15.5"
                },
                new PreProfessionalInternshipDto
                {
                    CodeStudentId = "STU002",
                    FullNameInterId = "Alice Brown",
                    FullNameAdviserId = "Bob Wilson",
                    FullNameCompanyId = "Tech Ltd",
                    Career = "Engineering",
                    StartDate = internships[1].StartDate,
                    EndDate = internships[1].EndDate,
                    State = "EnCurso",
                    Note = "18"
                }
            };

            _repositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(expectedDtos);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(expectedDtos[0].CodeStudentId, result[0].CodeStudentId);
            Assert.Equal(expectedDtos[0].FullNameInterId, result[0].FullNameInterId);
            Assert.Equal(expectedDtos[0].FullNameAdviserId, result[0].FullNameAdviserId);
            Assert.Equal(expectedDtos[0].FullNameCompanyId, result[0].FullNameCompanyId);
            Assert.Equal(expectedDtos[0].Career, result[0].Career);
            Assert.Equal(expectedDtos[0].State, result[0].State);
            Assert.Equal(expectedDtos[0].Note, result[0].Note);
            Assert.Equal(expectedDtos[1].CodeStudentId, result[1].CodeStudentId);
            _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once());
        }

        [Fact]
        public async Task GetAllAsync_EmptyList_ReturnsEmptyList()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<PreProfessionalInternshipDto>());

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once());
        }

        [Fact]
        public async Task GetAllAsync_RepositoryThrowsException_ThrowsException()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAllAsync())
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.GetAllAsync());
            _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once());
        }
    }
}