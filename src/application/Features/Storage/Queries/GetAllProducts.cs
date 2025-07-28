using MediatR;
using AutoMapper;
using Application.Features.Storage.DTOs;
using Application.Features.GeneralPropose.Specifications;
using Domain.Models;
using Domain.Repositories;
using Microsoft.Extensions.Logging;


namespace Application.Features.Storage.Queries
{
    /// <summary>
    /// Query to get all products.
    /// </summary>
    public record GetAllProductsQuery(int PageNumber = 1, int PageSize = 10) : 
        IRequest<List<ProductDto>> { }

    /// <summary>
    /// Handler for retrieving all products from the repository.
    /// </summary>
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<ProductDto>>
    {
        private readonly IEntityReadRepository<Product> _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllProductsQueryHandler> _logger;

        public GetAllProductsQueryHandler(
            IEntityReadRepository<Product> productRepository,
            IMapper mapper,
            ILogger<GetAllProductsQueryHandler> logger)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllProductsQuery...");

            var spec = new PaginateSpecification<Product>(request.PageNumber, request.PageSize);
            var products = await _productRepository.ListAsync(spec, cancellationToken);

            _logger.LogInformation("Fetched {Count} products from repository.", products.Count);

            var productDtos = _mapper.Map<List<ProductDto>>(products);

            _logger.LogInformation("Mapped products to DTOs successfully.");

            return productDtos;
        }
    }
}