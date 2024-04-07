using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data;

public class CatalogContextSeed
{
    public static void SeedData(IMongoCollection<Product> productCollection)
    {
        bool checkProducts = productCollection.Find(b => true).Any();
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        string path = Path.Combine(basePath, "Data", "SeedData", "products.json");

        if (!checkProducts)
        {
            string productsData = File.ReadAllText(path);
            List<Product>? products = JsonSerializer.Deserialize<List<Product>>(productsData);
            if (products != null)
            {
                foreach (Product item in products)
                {
                    productCollection.InsertOneAsync(item);
                }
            }
        }
    }
}
