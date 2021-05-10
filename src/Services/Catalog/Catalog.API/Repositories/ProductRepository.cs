using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;
        public ProductRepository(ICatalogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            FindOptions<Product> options = new FindOptions<Product> { Limit = 1 };
            IAsyncCursor<Product> task = await _context.Products.FindAsync(x => true);
            List<Product> list = await task.ToListAsync();
            return list;
        }


        public async Task<Product> GetProduct(string id)
        {
            FindOptions<Product> options = new FindOptions<Product> { Limit = 1 };
            IAsyncCursor<Product> task = await _context.Products.FindAsync(x => x.Id == id, options);
            Product product = await task.FirstOrDefaultAsync();
            return product;
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);
            IAsyncCursor<Product> task = await _context.Products.FindAsync(filter);
            List<Product> list = await task.ToListAsync();
            return list;
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, categoryName);
            IAsyncCursor<Product> task = await _context.Products.FindAsync(filter);
            List<Product> list = await task.ToListAsync();
            return list;
        }


        public async Task CreateProduct(Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }

        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);
            var deleteResult = await _context
                                 .Products
                                 .DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount> 0;
        }


        public async Task<bool> UpdateProduct(Product product)
        {
            var updateResult = await _context
                                  .Products
                                  .ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);
            return updateResult.IsAcknowledged
                && updateResult.ModifiedCount > 0;
        }
    }
}
