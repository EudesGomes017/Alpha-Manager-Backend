using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alpha.Domain.Interfaces;
using MediatR;

namespace Alpha.Application.Products.Commands
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, int>
    {
        private readonly IProductRepository _repository;

        public UpdateProductCommandHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException("Produto não encontrado.");

            product.Update(request.Name, request.Price, request.Barcode, request.ImageBytes);
            await _repository.UpdateAsync(product);



            return product.Id;
        }
    }
}
