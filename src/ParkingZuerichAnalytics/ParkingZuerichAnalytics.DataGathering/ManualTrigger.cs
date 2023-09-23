using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ParkingZuerichAnalytics.DataGathering.Core;

namespace ParkingZuerichAnalytics.DataGathering;

public class ManualTrigger
{
    private readonly RetrieveAndStoreMetrics retrieveAndStoreMetrics;

    public ManualTrigger(RetrieveAndStoreMetrics retrieveAndStoreMetrics)
    {
        this.retrieveAndStoreMetrics = retrieveAndStoreMetrics;
    }
    
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
        var msg = $"Succeeded in {stopwatch.Elapsed.TotalMilliseconds}ms";
        log.LogInformation(msg);
        return new OkObjectResult(msg);
    }
}