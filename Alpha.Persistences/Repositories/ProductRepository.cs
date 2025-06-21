using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alpha.Application.DTOs;
using Alpha.Domain;
using Alpha.Domain.Interfaces;
using Alpha.Persistences.Context;
using Microsoft.EntityFrameworkCore;

namespace Alpha.Persistences.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;

        public ProductRepository(DataContext context)
        {
            _context = context;
        }

        // ✔️ Adicionar Produto com proteção contra duplicatas
        public async Task AddAsync(Product product)
        {
            var exists = await ExistsByBarcodeAsync(product.Barcode);

            if (!exists)
            {
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Produto com este código de barras já existe.");
            }
        }

        // ✔️ Atualizar Produto
        public async Task UpdateAsync(Product product)
        {

            var existing = await _context.Products
             .FirstOrDefaultAsync(p => p.Id == product.Id);

            var exists = await ExistsByBarcodeAsync(product.Barcode);

           /* if (exists)
                throw new Exception("Produto com este código de barras já existe.");*/

            if (string.IsNullOrEmpty(product.ImageBytes))
                product.ImageBytes = "https://via.placeholder.com/150";

            if (existing == null)
                throw new Exception("Produto não encontrado.");

            // Verifica se o novo Barcode já pertence a outro produto
            var duplicateBarcode = await _context.Products
                .AnyAsync(p => p.Barcode == product.Barcode && p.Id != product.Id);

            if (duplicateBarcode)
                throw new Exception("Já existe um produto com este código de barras.");

            // Atualiza os campos
            existing.Name = product.Name;
            existing.Price = product.Price;
            existing.Barcode = product.Barcode;
            existing.ImageBytes = product.ImageBytes;
            existing.CreatedAt = product.CreatedAt;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        // ✔️ Deletar Produto
        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Products.FindAsync(id);
            if (entity is not null)
            {
                _context.Products.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        // ✔️ Buscar por ID
        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        // ✔️ Buscar por Código de Barras
        public async Task<Product?> GetByBarcodeAsync(string barcode)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Barcode == barcode);
        }

        // ✔️ Verificar se Barcode existe
        public async Task<bool> ExistsByBarcodeAsync(string barcode)
        {
            return await _context.Products
                .AnyAsync(p => p.Barcode == barcode);
        }

        // ✔️ Buscar com Filtros, Ordenação e Paginação
        public async  Task<IEnumerable<Product>> SearchAsync(
            string? name,
            string? barcode,
            string? sortBy,
            int page,
            int pageSize,
            string? imageBytes = null)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(p => p.Name.Contains(name));

            if (!string.IsNullOrWhiteSpace(barcode))
                query = query.Where(p => p.Barcode.Contains(barcode));

            if (!string.IsNullOrWhiteSpace(imageBytes))
                query = query.Where(p => p.ImageBytes.Contains(imageBytes));

            // Ordenação
            query = sortBy switch
            {
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                _ => query.OrderBy(p => p.Name) // padrão: nome
            };

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        // ✔️ Contagem total com filtros aplicados
        public async Task<int> CountAsync(string? name, string? barcode)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(p => p.Name.Contains(name));

            if (!string.IsNullOrWhiteSpace(barcode))
                query = query.Where(p => p.Barcode.Contains(barcode));

            return await query.CountAsync();
        }

        // ✔️ Paginação simplificada (para dashboards, por exemplo)
        public async Task<GetProductsResponse> GetPagedProductsAsync(int page, int pageSize)
        {
            var query = _context.Products.AsQueryable();

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(p => p.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new GetProductsResponse
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<IEnumerable<Product>> SearchAsync(string? name, string? barcode, string? sortBy, string? imageBytes, int page, int pageSize)
        {
            return await SearchAsync(name, barcode, sortBy, page, pageSize, imageBytes);
        }

        public async Task<int> GetTotalCountAsync(string? nameFilter, string? barcodeFilter)
        {
            return await _context.Products.CountAsync();
        }
    }

}
