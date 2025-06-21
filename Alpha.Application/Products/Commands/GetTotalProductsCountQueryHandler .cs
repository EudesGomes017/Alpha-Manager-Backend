
using Alpha.Domain.Interfaces;
using MediatR;

namespace Alpha.Application.Products.Commands
{

    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, GetProductsResponse>
    {
        private readonly IProductRepository _repository;

        public GetProductsQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetProductsResponse> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var items = await _repository.SearchAsync(
                request.Name,
                request.Barcode,
                request.SortBy,
                request.ImageBytes,
                request.Page,
                request.PageSize);

            var totalCount = await _repository.GetTotalCountAsync(
                request.Name,
                request.Barcode);

            return new GetProductsResponse
            {
                Items = items,
                TotalCount = totalCount
            };
        }
    }
}
