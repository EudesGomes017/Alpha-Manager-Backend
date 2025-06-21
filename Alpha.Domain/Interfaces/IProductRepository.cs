using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alpha.Domain.Interfaces
{
    public interface IProductRepository
    {
      
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<Product?> GetByIdAsync(int id);
        Task<Product?> GetByBarcodeAsync(string barcode); 
        Task<bool> ExistsByBarcodeAsync(string barcode);

        Task<IEnumerable<Product>> SearchAsync(
               string? name,
               string? barcode,
               string? sortBy,
               string? imageBytes,
               int page,
               int pageSize);

        Task<int> GetTotalCountAsync(string? nameFilter, string? barcodeFilter);
        Task<IEnumerable<Product>> SearchAsync(string? name, string? barcode, string? sortBy, int page, int pageSize, string? imageBytes);
    }
}
