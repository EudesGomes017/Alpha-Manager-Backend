using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alpha.Application.DTOs;
using Alpha.Domain;
using MediatR;

namespace Alpha.Application.Products.Commands
{
    public class GetProductsQuery : IRequest<GetProductsResponse>
    {
        public string? Name { get; set; }
        public string? Barcode { get; set; }
        public string? SortBy { get; set; }
        public string ImageBytes { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetProductsResponse
    {
        public IEnumerable<Product> Items { get; set; }
        public int TotalCount { get; set; }
    }

}
