using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
  public class ProductRepository : IProductRepository, IBrandRepository, ITypeRepository
  {
    private readonly ICatalogContext _context;

    public ProductRepository(ICatalogContext context)
    {
      _context = context;
    }

    async Task<IEnumerable<Product>> IProductRepository.GetProducts()
    {
      return await _context.Products.Find(p => true).ToListAsync();
    }

    async Task<Product> IProductRepository.GetProduct(string id)
    {
      return await _context
      .Products
      .Find(p => p.Id == id).FirstOrDefaultAsync();
    }
    async Task<IEnumerable<Product>> IProductRepository.GetProductsByName(string name)
    {
      FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);

      return await _context
      .Products
      .Find(filter)
      .ToListAsync();
    }

    async Task<IEnumerable<Product>> IProductRepository.GetProductsByBrand(string brand)
    {
      FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Brand.Name, brand);

      return await _context
      .Products
      .Find(filter)
      .ToListAsync();
    }

    async Task<Product> IProductRepository.CreateProduct(Product product)
    {
      await _context
      .Products
      .InsertOneAsync(product);

      return product;
    }

    async Task<bool> IProductRepository.UpdateProduct(Product product)
    {
      ReplaceOneResult result = await _context
      .Products
      .ReplaceOneAsync(p => p.Id == product.Id, product);

      return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    async Task<bool> IProductRepository.DeleteProduct(string id)
    {
      FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);

      DeleteResult result = await _context
      .Products
      .DeleteOneAsync(filter);

      return result.IsAcknowledged && result.DeletedCount > 0;
    }

    async Task<IEnumerable<ProductType>> ITypeRepository.GetAllTypes()
    {
      return await _context
      .Types
      .Find(t => true).ToListAsync();
    }

    async Task<IEnumerable<ProductBrand>> IBrandRepository.GetAllBrands()
    {
      return await _context
      .Brands
      .Find(b => true).ToListAsync();
    }
  }
}
