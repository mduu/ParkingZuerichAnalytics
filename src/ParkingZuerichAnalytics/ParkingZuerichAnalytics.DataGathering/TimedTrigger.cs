using System.Diagnostics;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using ParkingZuerichAnalytics.DataGathering.Core;
using ParkingZuerichAnalytics.DataGathering.Core.Retrieval;

namespace ParkingZuerichAnalytics.DataGathering;

public class TimedTrigger
{
    private readonly RetrieveAndStoreMetrics retrieveAndStoreMetrics = new(
        new ParkingInfoRetriever(),
        Environment.GetEnvironmentVariable("ConnectionStrings:datatableconnection"));
    
    [FunctionName("TimedTrigger")]
    public async Task RunAsync(
        [TimerTrigger("0 */30 * * * *")] TimerInfo myTimer,
        ILogger log)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        log.LogInformation("Timer trigger");

        await retrieveAndStoreMetrics.RetrieveAndStore();

        stopwatch.Stop();
        log.LogInformation("Succeeded in {ElapsedTotalMilliseconds}ms", stopwatch.Elapsed.TotalMilliseconds);
    }
}