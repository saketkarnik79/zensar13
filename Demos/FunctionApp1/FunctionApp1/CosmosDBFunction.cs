using System;
using System.Collections.Generic;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace FunctionApp1;

public class CosmosDBFunction
{
    private readonly ILogger<CosmosDBFunction> _logger;

    public CosmosDBFunction(ILogger<CosmosDBFunction> logger)
    {
        _logger = logger;
    }

    [Function("CosmosDBFunction")]
    public void Run([CosmosDBTrigger(
        databaseName: "InventoryDB",
        containerName: "products",
        Connection = "cs",
        LeaseContainerName = "leases",
        CreateLeaseContainerIfNotExists = true)] IReadOnlyList<JObject> input)
    {
        if (input != null && input.Count > 0)
        {
            _logger.LogInformation("Documents modified: " + input.Count);
            _logger.LogInformation("First document Id: " + input[0]["id"]);
        }
    }
}

//public class MyDocument
//{
//    public string id { get; set; }

//    public string Text { get; set; }

//    public int Number { get; set; }

//    public bool Boolean { get; set; }
//}