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
        private readonly IVendingMachineRepository _vendingMachineRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetProductListQueryHandler> _logger;

        public GetProductListQueryHandler(IVendingMachineRepository vendingMachineRepository, IMapper mapper, ILogger<GetProductListQueryHandler> logger)
        {
            _vendingMachineRepository = vendingMachineRepository ?? throw new ArgumentNullException(nameof(vendingMachineRepository));
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

            var vendingMachine = await _vendingMachineRepository.GetVendingMachineWithProduct(request.SerialNumber);
            return vendingMachine != null && vendingMachine.Products != null ? _mapper.Map<List<ProductDTO>>(vendingMachine.Products) : new List<ProductDTO>();
        }
    }
}
