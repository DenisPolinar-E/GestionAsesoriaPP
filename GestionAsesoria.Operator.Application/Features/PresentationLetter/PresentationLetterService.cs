using AutoMapper;
using GestionAsesoria.Operator.Application.DTOs.PresentationLetter.Request;
using GestionAsesoria.Operator.Application.DTOs.PresentationLetter.Response;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Application.Interfaces.Services;
using GestionAsesoria.Operator.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.Services.PresentationLetters
{
    public class PresentationLetterService : IPresentationLetterService
    {
        private readonly IPresentationLetterRepositoryAsync _repository;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public PresentationLetterService(
            IPresentationLetterRepositoryAsync repository,
            IUnitOfWork<int> unitOfWork,
            IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> CreatePresentationLetterAsync(CreatePresentationLetterDto dto, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<PresentationLetter>(dto);
            var addedEntity = await _repository.AddAsync(entity, cancellationToken);
            return addedEntity.Id;
        }

        public async Task<PresentationLetterDto> GetByRequestPPPIdAsync(int requestPPPId, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.GetByRequestPPPIdAsync(requestPPPId, cancellationToken);
            return _mapper.Map<PresentationLetterDto>(entity);
        }
    }
}