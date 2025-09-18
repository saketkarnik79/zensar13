using System;
using System.Collections.Generic;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace FunctionApp2;

public class CosmosDbFunction
{
    private readonly ILogger<CosmosDbFunction> _logger;

    public CosmosDbFunction(ILogger<CosmosDbFunction> logger)
    {
        _logger = logger;
    }

    [Function(nameof(CosmosDbFunction))]
    public void Run([CosmosDBTrigger(
        databaseName: "InventoryDB",
        containerName: "products",
        Connection = "cs",
        LeaseContainerName = "leases",
        CreateLeaseContainerIfNotExists = true)] IReadOnlyList<Product> input)
    {
        if (input != null && input.Count > 0)
        {
            _logger.LogInformation("Documents modified: " + input.Count);
            _logger.LogInformation("First document Id: " + input[0].id);
        }
    }

    public class Product
    {
        public string? id { get; set; }
        public string? category { get; set; }
        public string? name { get; set; }
        public decimal price { get; set; }
    }
}