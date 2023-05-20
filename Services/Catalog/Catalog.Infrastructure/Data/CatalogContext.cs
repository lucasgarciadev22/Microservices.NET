using Catalog.Core.Entities;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace Catalog.Infrastructure.Data
{
    public class CatalogContext : ICatalogContext
    {
        public IMongoCollection<Product> Products { get; }

        public IMongoCollection<ProductBrand> Brands { get; }

        public IMongoCollection<ProductType> Types { get; }

        public CatalogContext(IConfiguration configuration)
        {
            //Getting repositories data contexts usin mongo...
            var client = new MongoClient(
                configuration.GetValue<string>("DatabaseSettings:ConnectionString")
            );
            var database = client.GetDatabase(
                configuration.GetValue<string>("DatabaseSettings:DatabaseName")
            );
            Brands = database.GetCollection<ProductBrand>(
                configuration.GetValue<string>("DatabaseSettings:BrandsCollection")
            );
            Types = database.GetCollection<ProductType>(
                configuration.GetValue<string>("DatabaseSettings:TypesCollection")
            );
            Products = database.GetCollection<Product>(
                configuration.GetValue<string>("DatabaseSettings:CollectionName") //name can varies
            );

            //Planting seeds...
            BrandContextSeed.SeedData(Brands);
            TypeContextSeed.SeedData(Types);
            CatalogContextSeed.SeedData(Products);
        }
    }
}
