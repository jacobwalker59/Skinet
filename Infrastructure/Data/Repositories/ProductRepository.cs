using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _context;
        public ProductRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<ProductBrand>> getProductBrandsAsync()
        {
            return await _context.ProductBrands.ToListAsync();
        }

        public async Task<Product> getProductByIdAsync(int id)
        {
            return await _context.Products
            .Include(x => x.ProductBrand)
            .Include(x => x.ProductType)
            .FirstOrDefaultAsync(p => p.Id == id);

        }

        public async Task<IReadOnlyList<Product>> getProductsAsync()
        {
            return await _context.Products
            .Include(x => x.ProductType)
            .Include(x => x.ProductBrand)
            .ToListAsync();
        }

        public async Task<IReadOnlyList<ProductType>> getProductTypesAsync()
        {
            return await _context.ProductTypes.ToListAsync();
        }
    }
}