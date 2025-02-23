using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using Catalog.Infrastructure.Data;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories;

public class ProductRepository(ICatalogContext context)
    : IProductRepository,
        IBrandRepository,
        ITypesRepository
{
    private readonly ICatalogContext _context = context;

    public async Task<Pagination<Product>> GetAllProducts(CatalogSpecParams catalogSpecParams)
    {
        FilterDefinitionBuilder<Product> builder = Builders<Product>.Filter;
        FilterDefinition<Product> filter = builder.Empty;
        if (!string.IsNullOrEmpty(catalogSpecParams?.Search))
        {
            FilterDefinition<Product> searchFilter = builder.Regex(
                x => x.Name,
                new BsonRegularExpression(catalogSpecParams?.Search)
            );
            filter &= searchFilter;
        }
        if (!string.IsNullOrEmpty(catalogSpecParams?.BrandId))
        {
            FilterDefinition<Product> brandFilter = builder.Eq(
                x => x.Brands.Id,
                catalogSpecParams.BrandId
            );
            filter &= brandFilter; //AND operator
        }
        if (!string.IsNullOrEmpty(catalogSpecParams?.TypeId))
        {
            FilterDefinition<Product> typeFilter = builder.Eq(
                x => x.Types.Id,
                catalogSpecParams.TypeId
            );
            filter &= typeFilter;
        }

        if (!string.IsNullOrEmpty(catalogSpecParams?.Sort))
        {
            return new Pagination<Product>
            {
                PageSize = catalogSpecParams.PageSize,
                PageIndex = catalogSpecParams.PageIndex,
                Data = await DataFilter(catalogSpecParams, filter),
                Count = await _context.Products.CountDocumentsAsync(p => true) //TODO: Need to check while applying with UI
            };
        }

        return new Pagination<Product>
        {
            PageSize = catalogSpecParams is null ? 1 : catalogSpecParams.PageSize,
            PageIndex = catalogSpecParams is null ? 0 : catalogSpecParams.PageIndex,
            Data = await _context.Products
                .Find(filter)
                .Sort(Builders<Product>.Sort.Ascending("Name"))
                .Skip(catalogSpecParams?.PageSize * (catalogSpecParams?.PageIndex - 1))
                .Limit(catalogSpecParams?.PageSize)
                .ToListAsync(),
            Count = await _context.Products.CountDocumentsAsync(p => true)
        };
    }

    private async Task<IReadOnlyList<Product>> DataFilter(
        CatalogSpecParams catalogSpecParams,
        FilterDefinition<Product> filter
    )
    {
        return catalogSpecParams?.Sort switch
        {
            "priceAsc"
                => await _context.Products
                    .Find(filter)
                    .Sort(Builders<Product>.Sort.Ascending("Price"))
                    .Skip(catalogSpecParams?.PageSize * (catalogSpecParams?.PageIndex - 1))
                    .Limit(catalogSpecParams?.PageSize)
                    .ToListAsync(),
            "priceDesc"
                => await _context.Products
                    .Find(filter)
                    .Sort(Builders<Product>.Sort.Descending("Price"))
                    .Skip(catalogSpecParams?.PageSize * (catalogSpecParams?.PageIndex - 1))
                    .Limit(catalogSpecParams?.PageSize)
                    .ToListAsync(),
            _
                => await _context.Products
                    .Find(filter)
                    .Sort(Builders<Product>.Sort.Ascending("Name"))
                    .Skip(catalogSpecParams?.PageSize * (catalogSpecParams?.PageIndex - 1))
                    .Limit(catalogSpecParams?.PageSize)
                    .ToListAsync(),
        };
    }

    public async Task<Product> GetProduct(string id)
    {
        return await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Product>> GetProductByName(string name)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);
        return await _context.Products.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductByBrand(string name)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Brands.Name, name);
        return await _context.Products.Find(filter).ToListAsync();
    }

    public async Task<Product> CreateProduct(Product product)
    {
        await _context.Products.InsertOneAsync(product);
        return product;
    }

    public async Task<bool> UpdateProduct(Product product)
    {
        ReplaceOneResult updateResult = await _context.Products.ReplaceOneAsync(
            p => p.Id == product.Id,
            product
        );
        return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
    }

    public async Task<bool> DeleteProduct(string id)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);
        DeleteResult deleteResult = await _context.Products.DeleteOneAsync(filter);
        return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
    }

    public async Task<IEnumerable<ProductBrand>> GetAllBrands()
    {
        return await _context.Brands.Find(b => true).ToListAsync();
    }

    public async Task<IEnumerable<ProductType>> GetAllTypes()
    {
        return await _context.Types.Find(t => true).ToListAsync();
    }
}
