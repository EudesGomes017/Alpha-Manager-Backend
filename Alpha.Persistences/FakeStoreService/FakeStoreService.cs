using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Alpha.Application.Interfaces;
using Alpha.Domain;
using Alpha.Domain.Interfaces;

namespace Alpha.Persistences.FakeStoreService
{
    public class FakeStoreService : IFakeStoreService
    {
        private readonly HttpClient _httpClient;

        public FakeStoreService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task ReplicateProductAsync(Product product)
        {
            var payload = new
            {
                id = product.Id,
                title = product.Name,
                price = product.Price,
                description = $"Produto {product.Barcode}",
                category = "geral",
                image = "https://via.placeholder.com/150" 
            };

            var response = await _httpClient.PostAsJsonAsync("/products", payload);

            response.EnsureSuccessStatusCode();
        }

        public async Task<List<FakeStoreProduct>> GetProductsFromFakeStoreAsync()
        {
            var products = await _httpClient.GetFromJsonAsync<List<FakeStoreProduct>>("/products");

            if (products == null)
                throw new Exception("Não foi possível buscar produtos da Fake Store.");

            return products;
        }
    }
}
