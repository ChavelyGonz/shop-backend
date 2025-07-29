
using Domain.Enums;
using Domain.Interfaces;
using Domain.Exceptions;
using Domain.Repositories;

using AutoMapper;

namespace Application.Features.GeneralPropose.Helpers
{
    public partial class Helper<T> where T : class
    {
        private readonly IEntityReadRepository<T> _readRepository;
        private readonly IEntityRepository<T> _repository;
        private readonly IMapper _mapper;
        public Helper(
            IEntityReadRepository<T> readRepository,
            IEntityRepository<T> repository, 
            IMapper mapper)
        {
            _readRepository = readRepository;
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<int> TotalCount(CancellationToken cancellationToken = default) 
            => await _readRepository.CountAsync(cancellationToken);
        
        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _readRepository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
            {
                throw new ApiException
                (
                    ApiExceptionType.KeyNotFound, 
                    $"Entity of type {typeof(T)} with id {id} not found.", 
                    new KeyNotFoundException($"{id}")
                );
            }
            await _repository.DeleteAsync(entity, cancellationToken);
        }

        public async Task EditAsync<R>
            (R entityDto, CancellationToken cancellationToken = default)
            where R : IHasId
        {
            var entity = await _readRepository.GetByIdAsync(entityDto.Id, cancellationToken);
            if (entity == null)
            {
                throw new ApiException
                (
                    ApiExceptionType.KeyNotFound, 
                    $"Entity of type {typeof(T)} with id {entityDto.Id} not found.", 
                    new KeyNotFoundException($"{entityDto.Id}")
                );
            }
            Console.WriteLine($"I am here.");
            _mapper.Map<R, T>(entityDto, entity);
            await _repository.UpdateAsync(entity, cancellationToken);
        }
    }
}