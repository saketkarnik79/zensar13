using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionApp2;

public class StorageFunction
{
    private readonly ILogger<StorageFunction> _logger;

    public StorageFunction(ILogger<StorageFunction> logger)
    {
        _logger = logger;
    }

    [Function(nameof(StorageFunction))]
    public async Task Run([BlobTrigger("democontainer/{name}", Connection = "AzureWebJobsStorage")] Stream stream, string name)
    {
        using var blobStreamReader = new StreamReader(stream);
        var content = await blobStreamReader.ReadToEndAsync();
        _logger.LogInformation("C# Blob trigger function Processed blob\n Name: {name} \n Data: {content}", name, content);
        Console.WriteLine($"Blob trigger function processed blob\n Name: {name} \n Data: {content}");
    }
}