using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alpha.Domain;

namespace Alpha.Application.DTOs
{
    public class GetProductsResponse
    {
        public IEnumerable<Product> Items { get; set; }
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

}
