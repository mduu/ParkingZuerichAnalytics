using System.Diagnostics;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using ParkingZuerichAnalytics.DataGathering.Core;
using ParkingZuerichAnalytics.DataGathering.Core.Retrieval;

namespace ParkingZuerichAnalytics.DataGathering;

public class TimedTrigger
{
    private readonly RetrieveAndStoreMetrics retrieveAndStoreMetrics = new(
        new ParkingInfoRetriever(),
        new TelemetryConfiguration());
    
    [FunctionName("TimedTrigger")]
    public async Task RunAsync(
        [TimerTrigger("0 */5 * * * *")] TimerInfo myTimer,
        ILogger log)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        log.LogInformation("Timer trigger");

        await retrieveAndStoreMetrics.RetrieveAndStore();

        stopwatch.Stop();
        log.LogInformation($"Succeeded in {stopwatch.Elapsed.TotalMilliseconds}ms");
    }
}