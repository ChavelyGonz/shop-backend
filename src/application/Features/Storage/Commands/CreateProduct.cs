
using Domain.Enums;
using Domain.Models;
using Domain.Exceptions;
using Domain.Repositories;

using Application.Features.Storage.DTOs;
using Application.Features.Storage.Queries;
using Application.Features.GeneralPropose.DTOs;

using MediatR;
using AutoMapper;

namespace Application.Features.Storage.Commands
{
    public record CreateProductCommand() : IRequest<ProductDto>
    {
        public ProductDto DTO { get; set; }
        public IEnumerable<int> Stores { get; set; }
    }
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
    {
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        private readonly IEntityRepository<Product> _productRepository;
        private readonly IEntityRepository<ProductStore> _productStoreRepository;

        public CreateProductCommandHandler
        (
            IEntityRepository<Product> productRepository,
            IEntityRepository<ProductStore> productStoreRepository,
            IMapper mapper,
            ISender sender
        ) 
        {
            _productRepository = productRepository; 
            _productStoreRepository = productStoreRepository;
            _mapper = mapper; 
            _sender = sender;
        }

        public async Task<ProductDto> Handle
            (CreateProductCommand request, CancellationToken cancellationToken)
        {
            await CheckIds(request.Stores, cancellationToken);
            await CreateProduct(request.DTO, cancellationToken);
            await CreateProductStores(request.Stores, request.DTO.Id, cancellationToken);
            return request.DTO;
        }

        private async Task CheckIds(IEnumerable<int> stores, CancellationToken cancellationToken)
        {
            CheckedIds storesCheckedIds = 
                await _sender.Send(new ExistStoresIdQuery() {Stores = stores});
            if (!storesCheckedIds.AllExist)
                throw new ApiException
                (
                    ApiExceptionType.KeyNotFound, 
                    $"Store with id {storesCheckedIds.FirstMissedId} not found.", 
                    new KeyNotFoundException($"{storesCheckedIds.FirstMissedId}")
                );
        }
        private async Task CreateProduct(ProductDto dto, CancellationToken cancellationToken)
        {
            var newProduct = _mapper.Map<ProductDto, Product>(dto);
            await _productRepository.AddAsync(newProduct);
            dto.Id = newProduct.Id;
        }
        private async Task CreateProductStores
            (IEnumerable<int> storesId, int productId, CancellationToken cancellationToken)
        {
            var productStores = storesId.Select(storeId => new ProductStore
            {
                ProductId = productId,
                StoreId = storeId
            }).ToList();

            await _productStoreRepository.AddRangeAsync(productStores);
        }
    }
}


