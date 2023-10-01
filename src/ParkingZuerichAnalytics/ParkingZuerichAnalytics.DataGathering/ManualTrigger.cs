using System.Diagnostics;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ParkingZuerichAnalytics.DataGathering.Core;
using ParkingZuerichAnalytics.DataGathering.Core.Retrieval;

namespace ParkingZuerichAnalytics.DataGathering;

public class ManualTrigger
{
    private readonly RetrieveAndStoreMetrics retrieveAndStoreMetrics = new(new ParkingInfoRetriever());

    [FunctionName("ManualTrigger")]
    [TableOutput("test", Connection = "datatableconnection")]
    public ParkingEntity RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
        HttpRequest req,
        ILogger log)
    {
        // var stopwatch = new Stopwatch();
        // stopwatch.Start();
        log.LogInformation("Manuel trigger");

        return ParkingEntity.Create(new ParkingInfo("TestParking", "Open", 42));

        // try
        // {
        //     await retrieveAndStoreMetrics.RetrieveAndStore();
        // }
        // catch (Exception e)
        // {
        //     log.LogError(e, "Error in RetrieveAndStore()");
        //     throw;
        // }
        //
        // stopwatch.Stop();
        // log.LogInformation("Succeeded in {ElapsedTotalMilliseconds}ms", stopwatch.Elapsed.TotalMilliseconds);
        //
        // return new OkObjectResult($"Succeeded in {stopwatch.Elapsed.TotalMilliseconds}ms");
    }
}