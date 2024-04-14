using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data;

public static class BrandContextSeed
{
    public static void SeedData(IMongoCollection<ProductBrand> brandCollection)
    {
        bool checkBrands = brandCollection.Find(b => true).Any();
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        string path = Path.Combine(basePath, "Data", "SeedData", "brands.json");

        if (checkBrands)
            return;

        string brandsData = File.ReadAllText(path);
        List<ProductBrand>? brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
        if (brands != null)
        {
            foreach (ProductBrand item in brands)
            {
                brandCollection.InsertOneAsync(item);
            }
        }
    }
}
