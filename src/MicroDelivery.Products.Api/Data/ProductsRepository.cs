using MicroDelivery.Products.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroDelivery.Products.Api.Data
{
    public interface IProductsRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product?> GetProductAsync(int id);
        Task<long> CountAllProductsAsync();

        Task<Product> CreateProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int id);
    }

    public class ProductsRepository : IProductsRepository
    {
        private readonly ProductsContext productContext;

        public ProductsRepository(ProductsContext productContext)
        {
            this.productContext = productContext;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await this.productContext.Products.ToListAsync();
        }

        public async Task<Product?> GetProductAsync(int id)
        {
            return await this.productContext.Products.FindAsync(id);
        }

        public async Task<long> CountAllProductsAsync()
        {
            return await this.productContext.Products.LongCountAsync();
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            await productContext.Products.AddAsync(product);
            await productContext.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            productContext.Products.Update(product);
            await productContext.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var rowsAffected = await productContext.Products.Where(c => c.Id == id).ExecuteDeleteAsync();
            return rowsAffected > 0; ;
        }
    }
}
