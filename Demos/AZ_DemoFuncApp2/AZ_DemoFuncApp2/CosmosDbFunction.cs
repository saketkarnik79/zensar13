using System;
using System.Collections.Generic;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AZ_DemoFuncApp2;

public class CosmosDbFunction
{
    private readonly ILogger<CosmosDbFunction> _logger;

    public CosmosDbFunction(ILogger<CosmosDbFunction> logger)
    {
        _logger = logger;
    }

    [Function("CosmosDbFunction")]
    public void Run([CosmosDBTrigger(
        databaseName: "InventoryDB",
        containerName: "products",
        Connection = "ConnectionStrings:CosmosDBConnection",
        LeaseContainerName = "leases",
        CreateLeaseContainerIfNotExists = true)] IReadOnlyList<MyDocument> input)
    {
        if (input != null && input.Count > 0)
        {
            _logger.LogInformation("Documents modified: " + input.Count);
            _logger.LogInformation("First document Id: " + input[0].id);
        }
    }
}

public class MyDocument
{
    public string id { get; set; }

    public string Text { get; set; }

    public int Number { get; set; }

    public bool Boolean { get; set; }
}