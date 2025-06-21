using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Alpha.Application.Products.Commands
{
    public class GetTotalProductsCountQuery : IRequest<int>
    {
      
        public string? NameFilter { get; set; }
        public string? BarcodeFilter { get; set; }
    }
}
