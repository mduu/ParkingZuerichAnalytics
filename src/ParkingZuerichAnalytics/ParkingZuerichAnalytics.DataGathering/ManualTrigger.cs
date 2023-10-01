using System.Diagnostics;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ParkingZuerichAnalytics.DataGathering.Core;
using ParkingZuerichAnalytics.DataGathering.Core.Retrieval;

namespace ParkingZuerichAnalytics.DataGathering;

public class ManualTrigger
{
    private readonly RetrieveAndStoreMetrics retrieveAndStoreMetrics = new(
        new ParkingInfoRetriever(),
        Environment.GetEnvironmentVariable("ConnectionStrings:datatableconnection"));

    [FunctionName("ManualTrigger")]
    public async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
        HttpRequest req,
        ILogger log)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        log.LogInformation("Manuel trigger");

        await retrieveAndStoreMetrics.RetrieveAndStore();

        stopwatch.Stop();
        log.LogInformation("Succeeded in {ElapsedTotalMilliseconds}ms", stopwatch.Elapsed.TotalMilliseconds);
        
        return new OkObjectResult($"Succeeded in {stopwatch.Elapsed.TotalMilliseconds}ms");
    }
}