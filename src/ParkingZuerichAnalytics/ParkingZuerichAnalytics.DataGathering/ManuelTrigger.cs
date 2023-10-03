using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ParkingZuerichAnalytics.DataGathering.Core;
using ParkingZuerichAnalytics.DataGathering.Core.Retrieval;

namespace ParkingZuerichAnalytics.DataGathering;

public class ManuelTrigger
{
    private readonly RetrieveAndStore retrieveAndStore = new(new ParkingInfoRetriever());

    [FunctionName("ManuelTrigger")]
    public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        log.LogInformation("Manual trigger");

        try
        {
            await retrieveAndStore.Update();
        }
        catch (Exception e)
        {
            log.LogError(e, "Error in RetrieveAndStore.Run()");
            throw;
        }

        stopwatch.Stop();
        log.LogInformation("Manual trigger succeeded in {ElapsedTotalMilliseconds}ms", stopwatch.Elapsed.TotalMilliseconds);

        return new OkResult();
    }
}