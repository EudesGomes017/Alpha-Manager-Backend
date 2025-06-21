using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alpha.Domain;
using Alpha.Domain.Interfaces;

namespace Alpha.Application.Interfaces
{
    public interface IFakeStoreService
    {
        Task ReplicateProductAsync(Product product);
        Task<List<FakeStoreProduct>> GetProductsFromFakeStoreAsync();
    }
}
