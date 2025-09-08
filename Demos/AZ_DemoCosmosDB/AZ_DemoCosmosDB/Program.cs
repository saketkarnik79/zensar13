using AZ_DemoCosmosDB.Models;
using Azure;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace AZ_DemoCosmosDB
{
    internal class Program
    {
        static string cs = "";
        static string containerName = "products";
        static string databaseName = "InventoryDB";

        static async Task Main(string[] args)
        {
            var cosmosClient = new CosmosClient(cs);
            await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseName);
            var database = cosmosClient.GetDatabase(databaseName);

            await database.CreateContainerIfNotExistsAsync(containerName, "/category");
            var container = database.GetContainer(containerName);

            var p1 = new Product
            {
                Id = "1",
                Category = "Electronics",
                Name = "Laptop",
                Quantity = 50,
                Price = 999.99m,
                Clearance = true
            };

            var p2 = new Product
            {
                Id = "2",
                Category = "Electronics",
                Name = "Smartphone",
                Quantity = 200,
                Price = 699.99m,
                Clearance = true
            };

            var p3 = new Product
            {
                Id = "3",
                Category = "Home Appliances",
                Name = "Blender",
                Quantity = 80,
                Price = 49.99m,
                Clearance = false
            };

            ItemResponse<Product> response1 = await container.UpsertItemAsync(p1, new PartitionKey(p1.Category));
            Console.WriteLine($"Created item in database with id: {response1.Resource.Id} - RU Charge: {response1.RequestCharge}");

            ItemResponse<Product> response2 = await container.UpsertItemAsync(p2, new PartitionKey(p2.Category));
            Console.WriteLine($"Created item in database with id: {response2.Resource.Id} - RU Charge: {response2.RequestCharge}");

            ItemResponse<Product> response3 = await container.UpsertItemAsync(p3, new PartitionKey(p3.Category));
            Console.WriteLine($"Created item in database with id: {response3.Resource.Id} - RU Charge: {response3.RequestCharge}");

            ItemResponse<Product> readResponse = await container.ReadItemAsync<Product>(p2.Id.ToString(), new PartitionKey(p2.Category));
            Console.WriteLine($"Read item from database with id: {readResponse.Resource.Id} - Name: {readResponse.Resource.Name} - RU Charge: {readResponse.RequestCharge}");

            await container.DeleteItemAsync<Product>(p3.Id.ToString(), new PartitionKey(p3.Category));
            Console.WriteLine("Deleted item with id: " + p3.Id);

            var sql = new QueryDefinition("SELECT * FROM products p WHERE p.Clearance = @clearance").WithParameter("@clearance", true);

            FeedIterator<Product> query = container.GetItemQueryIterator<Product>(sql);
            List<Product> results = new List<Product>();
            while (query.HasMoreResults)
            {
                FeedResponse<Product> response = await query.ReadNextAsync();
                results.AddRange(response);
                
            }
            foreach (var item in results)
            {
                Console.WriteLine($"Queried item: {item.Id} - Name: {item.Name} - Clearance: {item.Clearance}");
            }
            Console.WriteLine("\n\nProgram completed. Press any key to exit...");
            Console.ReadKey();  
        }
    }
}
