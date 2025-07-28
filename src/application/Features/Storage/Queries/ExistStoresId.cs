
using Domain.Models;
using Domain.Repositories;
using Application.Features.GeneralPropose.DTOs;

using MediatR;
using AutoMapper;

namespace Application.Features.Storage.Queries
{
    public record ExistStoresIdQuery : IRequest<CheckedIds>
    {
        public IEnumerable<int> Stores { get; set; }
    }
    public record ExistStoresIdQueryHandler : IRequestHandler<ExistStoresIdQuery, CheckedIds>
    {
        private readonly IEntityReadRepository<Store> _storeRepository;

        public ExistStoresIdQueryHandler(IEntityReadRepository<Store> storeRepository) 
        {
            _storeRepository = storeRepository;
        }
        public async Task<CheckedIds> Handle
            (ExistStoresIdQuery request, CancellationToken cancellationToken)
        {
            var result = new CheckedIds
            {
                Ids = new List<int>(),
                AllExist = true
            };
            
            foreach (int id in request.Stores)
            {
                var store = await _storeRepository.GetByIdAsync(id);
                if (store == null && result.AllExist) 
                {
                    result.AllExist = false;
                    result.FirstMissedId = id;
                }
                else if (store != null) result.Ids.Add(id);
            }
            return result;
        }
    }
}