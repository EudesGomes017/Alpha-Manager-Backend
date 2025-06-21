using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alpha.Application.Interfaces;
using Alpha.Domain.Interfaces;
using Alpha.Domain;
using MediatR;

namespace Alpha.Application.Products.Commands
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IProductRepository _repository;
        private readonly IFakeStoreService _fakeStore;

        public CreateProductCommandHandler(IProductRepository repository, IFakeStoreService fakeStore)
        {
            _repository = repository;
            _fakeStore = fakeStore;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product(request.Name, request.Price, request.Barcode, request.ImageBytes, DateTime.UtcNow);

            await _repository.AddAsync(product);
            await _fakeStore.ReplicateProductAsync(product); // integração externa

            return product.Id;
        }
    }
}
