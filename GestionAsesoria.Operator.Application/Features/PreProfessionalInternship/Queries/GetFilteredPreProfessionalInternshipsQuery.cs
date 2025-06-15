using GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternship.Request;
using GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternship.Response;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace GestionAsesoria.Operator.Application.Features.PreProfessionalInternships.Queries
{
    public class FilterPreProfessionalInternshipQuery : IRequest<List<FilterPreProfessionalInternshipDto>>
    {
        public FilterPreProfessionalInternshipDto Filter { get; set; }

        public class Handler : IRequestHandler<FilterPreProfessionalInternshipQuery, List<FilterPreProfessionalInternshipDto>>
        {
            private readonly IUnitOfWork<int> _unitOfWork;

            public Handler(IUnitOfWork<int> unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<FilterPreProfessionalInternshipDto>> Handle(FilterPreProfessionalInternshipQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.PreProfessionalInternshipRepository.GetFilteredPppAsync(request.Filter);

                if (result == null || result.Count == 0)
                {
                    throw new KeyNotFoundException("No se encontraron prácticas preprofesionales que coincidan con los filtros proporcionados.");
                }

                return result;
            }
        }
    }

}