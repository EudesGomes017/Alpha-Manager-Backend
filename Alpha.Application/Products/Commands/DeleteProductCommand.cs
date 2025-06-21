using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Alpha.Application.Products.Commands
{
    public class DeleteProductCommand : IRequest<int>
    {
        public int Id { get; }

        public DeleteProductCommand(int id) => Id = id;
    }
}
