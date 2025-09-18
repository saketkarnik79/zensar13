using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace FunctionApp2;

public static class DurableFunctionsDemo
{
    //[Function(nameof(DurableFunctionsDemo))]
    //public static async Task<List<string>> RunOrchestrator(
    //    [OrchestrationTrigger] TaskOrchestrationContext context)
    //{
    //ILogger logger = context.CreateReplaySafeLogger(nameof(DurableFunctionsDemo));
    //logger.LogInformation("Saying hello.");
    //var outputs = new List<string>();

    //// Replace name and input with values relevant for your Durable Functions Activity
    //outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "Tokyo"));
    //outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "Seattle"));
    //outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "London"));

    //// returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
    //return outputs;
    //}


    [Function("DurableFunctionsDemo")]
    public static async Task<List<string>> RunOrchestrator(
        [OrchestrationTrigger] TaskOrchestrationContext context)
    {
        var outputs = new List<string>();

        outputs.Add(await context.CallActivityAsync<string>("ActivityFunction", "Validate Order"));
        outputs.Add(await context.CallActivityAsync<string>("ActivityFunction", "Charge Payment"));
        outputs.Add(await context.CallActivityAsync<string>("ActivityFunction", "Send Confirmation"));

        return outputs;
    }



    [Function("ActivityFunction")]
    public static string RunActivity([ActivityTrigger] string step, FunctionContext executionContext)
    {
        ILogger logger = executionContext.GetLogger("RunActivity");
        logger.LogInformation($"Executing step: {step}");
        return $"Completed: {step}";
    }


    //[Function(nameof(SayHello))]
    //public static string SayHello([ActivityTrigger] string name, FunctionContext executionContext)
    //{
    //    ILogger logger = executionContext.GetLogger("SayHello");
    //    logger.LogInformation("Saying hello to {name}.", name);
    //    return $"Hello {name}!";
    //}

    [Function("DurableFunctionsDemo_HttpStart")]
    public static async Task<HttpResponseData> HttpStart(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
        [DurableClient] DurableTaskClient client,
        FunctionContext executionContext)
    {
        ILogger logger = executionContext.GetLogger("DurableFunctionsDemo_HttpStart");

        // Function input comes from the request content.
        string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
            nameof(DurableFunctionsDemo));
        
        logger.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

        // Returns an HTTP 202 response with an instance management payload.
        // See https://learn.microsoft.com/azure/azure-functions/durable/durable-functions-http-api#start-orchestration
        return await client.CreateCheckStatusResponseAsync(req, instanceId);
    }
}