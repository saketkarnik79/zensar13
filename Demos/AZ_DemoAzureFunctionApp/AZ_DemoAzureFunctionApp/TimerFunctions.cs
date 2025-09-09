using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AZ_DemoAzureFunctionApp;

public class TimerFunctions
{
    private readonly ILogger _logger;

    public TimerFunctions(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<TimerFunctions>();
    }

    [Function("GenerateLogEveryMinute")]
    public void Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer)
    {
        _logger.LogInformation("C# Timer trigger function executed at: {executionTime}", DateTime.Now);
        
        if (myTimer.ScheduleStatus is not null)
        {
            _logger.LogInformation("Next timer schedule at: {nextSchedule}", myTimer.ScheduleStatus.Next);
        }
    }
}