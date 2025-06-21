using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alpha.Application.DTOs;
using MediatR;

namespace Alpha.Application.Queries
{
    public class GetProductsQuery : IRequest<IEnumerable<ProductDto>>
    {
        public string? Name { get; set; }
        public string? Barcode { get; set; }
        public string? SortByPrice { get; set; } // asc ou desc
        public string? ImageBytes { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
