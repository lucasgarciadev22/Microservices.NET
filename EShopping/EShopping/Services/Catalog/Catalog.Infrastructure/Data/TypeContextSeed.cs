using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data;

public class TypeContextSeed
{
    public static void SeedData(IMongoCollection<ProductType> typeCollection)
    {
        bool checkTypes = typeCollection.Find(b => true).Any();
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        string path = Path.Combine(basePath, "Data", "SeedData", "types.json");

        if (!checkTypes)
        {
            string typesData = File.ReadAllText(path);
            List<ProductType>? types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
            if (types != null)
            {
                foreach (ProductType item in types)
                {
                    typeCollection.InsertOneAsync(item);
                }
            }
        }
    }
}
