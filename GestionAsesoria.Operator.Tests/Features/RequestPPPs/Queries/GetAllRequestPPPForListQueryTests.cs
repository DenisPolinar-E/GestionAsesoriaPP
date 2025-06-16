using GestionAsesoria.Operator.Application.DTOs.RequestPPP.Response;
using GestionAsesoria.Operator.Application.Features.RequestPPPs.Queries;
using GestionAsesoria.Operator.Application.Interfaces;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GestionAsesoria.Operator.Tests.Features.RequestPPPs.Queries
{
    public class GetAllRequestPPPForListQueryTests
    {
        private readonly Mock<IRequestPPPRepositoryAsync> _requestPPPRepoMock;
        private readonly Mock<IUnitOfWork<int>> _unitOfWorkMock;
        private readonly GetAllRequestPPPForListQuery.Handler _handler;

        public GetAllRequestPPPForListQueryTests()
        {
            _requestPPPRepoMock = new Mock<IRequestPPPRepositoryAsync>();
            _unitOfWorkMock = new Mock<IUnitOfWork<int>>();

            _unitOfWorkMock
                .Setup(u => u.RequestPPPRepository)
                .Returns(_requestPPPRepoMock.Object);

            _handler = new GetAllRequestPPPForListQuery.Handler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsList_WhenRequestsExist()
        {
            // Arrange
            var mockData = new List<ListRequestPPPDto>
            {
                new ListRequestPPPDto
                {
                    Id = 1,
                    Title = "Solicitud 1",
                    Status = "Aprobado",
                    StudentName = "Juan Pérez",
                    CompanyName = "Empresa SAC",
                    AcademicAreaName = "Área 1",
                    DocumentUrl = "http://url.com/doc.pdf"
                }
            };

            _requestPPPRepoMock
                .Setup(r => r.GetAllForListAsync())
                .ReturnsAsync(mockData);

            // Act
            var result = await _handler.Handle(new GetAllRequestPPPForListQuery(), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Solicitud 1", result[0].Title);
        }

        [Fact]
        public async Task Handle_ReturnsEmptyList_WhenNoRequestsExist()
        {
            // Arrange
            _requestPPPRepoMock
                .Setup(r => r.GetAllForListAsync())
                .ReturnsAsync(new List<ListRequestPPPDto>());

            // Act
            var result = await _handler.Handle(new GetAllRequestPPPForListQuery(), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
