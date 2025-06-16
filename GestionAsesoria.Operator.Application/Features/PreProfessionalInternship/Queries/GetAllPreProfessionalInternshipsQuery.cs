using GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternship.Response;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace GestionAsesoria.Operator.Application.Features.PreProfessionalInternships.Queries
{
    public class GetAllPreProfessionalInternshipQuery : IRequest<List<PreProfessionalInternshipDto>>
    {
        public class Handler : IRequestHandler<GetAllPreProfessionalInternshipQuery, List<PreProfessionalInternshipDto>>
        {
            private readonly IUnitOfWork<int> _unitOfWork;

            public Handler(IUnitOfWork<int> unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<PreProfessionalInternshipDto>> Handle(GetAllPreProfessionalInternshipQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.PreProfessionalInternshipRepository.GetAllPppAsync();
                if (result == null || result.Count == 0)
                {
                    throw new KeyNotFoundException("No existen prácticas preprofesionales registradas.");
                }

                return result;
            }
        }
    }
}