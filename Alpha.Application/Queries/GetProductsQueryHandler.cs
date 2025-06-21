using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alpha.Application.DTOs;
using Alpha.Domain.Interfaces;
using MediatR;

namespace Alpha.Application.Queries
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IProductRepository _repository;

        public GetProductsQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.SearchAsync(
                request.Name,
                request.Barcode,
                request.SortByPrice,
                request.Page,
                request.PageSize,
                request.ImageBytes
            );

            return result.Select(p => new ProductDto(p.Id, p.Name, p.Price, p.Barcode, p.ImageBytes));
        }
    }
}
