using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Vending.Application.Contracts.Persistence;

namespace Vending.Application.Features.Catalog.Queries.GetProductList
{
    /// <summary>
    /// CQRS pattern: GetProductListQuery query handler
    /// </summary>
    public class GetProductListQueryHandler : IRequestHandler<GetProductListQuery, List<ProductDTO>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetProductListQueryHandler> _logger;

        public GetProductListQueryHandler(IProductRepository productRepository, IMapper mapper, ILogger<GetProductListQueryHandler> logger)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Handler whose responsibility is to obtain the product catalog of the vending machine
        /// </summary>
        /// <param name="request">Query parameters: vending machine identifier</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<ProductDTO>> Handle(GetProductListQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start handler - GetProductListQueryHandler");

            var productList = await _productRepository.GetAllProductsByVendingMachine(request.VendigMachineId);
           return _mapper.Map<List<ProductDTO>>(productList);
        }
    }
}
