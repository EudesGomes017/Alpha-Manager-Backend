using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alpha.Application.DTOs;
using MediatR;

namespace Alpha.Application.Products.Commands
{
    public class GetProductByIdQuery : IRequest<ProductDto?>
    {
        public int Id { get; }

        public GetProductByIdQuery(int id)
        {
            Id = id;
        }
    }
}
