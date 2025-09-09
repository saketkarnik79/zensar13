<<<<<<< HEAD
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AZ_DemoAzureFunctionApp;

public class AddFunction
{
    private readonly ILogger<AddFunction> _logger;

    public AddFunction(ILogger<AddFunction> logger)
    {
        _logger = logger;
    }

    [Function("Add")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route ="add")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        string? n1= req.Query["n1"];
        string? n2= req.Query["n2"];

       double? x=null, y= null;

       if (double.TryParse(n1, out double num1) && double.TryParse(n2, out double num2))
       {
           x = num1;
           y = num2;    
       }
       if (x is null || y is null)
        {
            var body="";
            using var reader = new StreamReader(req.Body);
            try
            {
                body = await reader.ReadToEndAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
           if (!string.IsNullOrEmpty(body))
           {
               var data = JsonSerializer.Deserialize<Dictionary<string, double>>(body);
                x ??= data["x"];
                y ??= data["y"];
            }
       }

       if (x is null || y is null)
       {
           return new BadRequestObjectResult("Please pass two valid numbers on the query string or in the request body");
       }

        return new OkObjectResult($"The sum of {x} and {y} is {x + y}.");
    }
=======
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AZ_DemoAzureFunctionApp;

public class AddFunction
{
    private readonly ILogger<AddFunction> _logger;

    public AddFunction(ILogger<AddFunction> logger)
    {
        _logger = logger;
    }

    [Function("Add")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route ="add")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        string? n1= req.Query["n1"];
        string? n2= req.Query["n2"];

       double? x=null, y= null;

       if (double.TryParse(n1, out double num1) && double.TryParse(n2, out double num2))
       {
           x = num1;
           y = num2;    
       }
       if (x is null || y is null)
        {
            var body="";
            using var reader = new StreamReader(req.Body);
            try
            {
                body = await reader.ReadToEndAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
           if (!string.IsNullOrEmpty(body))
           {
               var data = JsonSerializer.Deserialize<Dictionary<string, double>>(body);
                x ??= data["x"];
                y ??= data["y"];
            }
       }

       if (x is null || y is null)
       {
           return new BadRequestObjectResult("Please pass two valid numbers on the query string or in the request body");
       }

        return new OkObjectResult($"The sum of {x} and {y} is {x + y}.");
    }
>>>>>>> ff0cba1b69401430554019c3ab9d9afdda7f4eee
}