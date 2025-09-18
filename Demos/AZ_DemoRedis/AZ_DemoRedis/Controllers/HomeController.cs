using System.Diagnostics;
using AZ_DemoRedis.Models;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Text.Json;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace AZ_DemoRedis.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Data()
        {
            //Declare string variable for Key Vault Url
            string kvUrl = "https://skzenkv.vault.azure.net/";
            //Create a string variable to hold the secret value
            string redisCacheConnectionString = "";
            //Authenticate to Key Vault using DefaultAzureCredential
            //var credential = new DefaultAzureCredential();

            //Authenticate by using Azure Entra ID App Registration

            var tenantId = "d463edec-5f3e-4fc1-9d60-45c87054ae76";
            var clientId = "3c6c1575-12a0-4ab1-9142-ae2919db04e1";
            var clientSecret = "YfI8Q~6K3wVYSExtlqdLlF6ktQMpNrEj5pQ5_cwO";
           
            var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);


            //create Key Vault Client
            var client = new SecretClient(new Uri(kvUrl), credential);

            //Retrieve the secret from Key Vault
            KeyVaultSecret secret = await client.GetSecretAsync("RedisCS");
            redisCacheConnectionString = secret.Value;
            // Connect to the Redis server
            var redis = await ConnectionMultiplexer.ConnectAsync(redisCacheConnectionString);
            var cache = redis.GetDatabase();
            // Set a value in Redis
            if(!cache.KeyExists("data"))
                cache.StringSet("data", $"Hello from Redis Cache created at {DateTime.Now.ToString()}!", TimeSpan.FromSeconds(40));
            // Get the value from Redis
            string value = cache.StringGet("data")!;
            // Pass the value to the view
            if(!string.IsNullOrEmpty(value))
                ViewBag.RedisCacheValue = value;
            else
                ViewBag.RedisCacheValue = "No value found in Redis cache.";

            Person p = new Person()
            {
                Id = 1,
                Name = "John Doe",
                Age = 30
            };

            string personJson = JsonSerializer.Serialize(p);
            if(!cache.KeyExists("person:1"))
                cache.StringSet("person:1", personJson, TimeSpan.FromSeconds(40));
            personJson = "";
            p = null;
            personJson = cache.StringGet("person:1")!;
            if(!string.IsNullOrEmpty(personJson))
                p = JsonSerializer.Deserialize<Person>(personJson)!;
            ViewBag.Person = p;
            await redis.CloseAsync();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
