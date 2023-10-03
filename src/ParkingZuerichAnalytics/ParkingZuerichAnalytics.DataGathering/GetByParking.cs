using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ParkingZuerichAnalytics.DataGathering.Core;
using ParkingZuerichAnalytics.DataGathering.Core.Retrieval;

namespace ParkingZuerichAnalytics.DataGathering;

public class GetByParking
{
    private readonly RetrieveAndStore retrieveAndStore = new(new ParkingInfoRetriever());

    [FunctionName("GetByParking")]
    public async Task<IActionResult> Run(
        [HttpTrigger(
            AuthorizationLevel.Function,
            "get",
            Route = "parking/{name}")]
        HttpRequest req,
        string name,
        ILogger log)
    {
        log.LogInformation("Request for {Function}", nameof(GetByParking));

        var fromParam = req.Query["from"].FirstOrDefault();
        var from = fromParam is not null
            ? DateTimeOffset.Parse(fromParam)
            : DateTimeOffset.UtcNow.AddDays(-14);

        var toParam = req.Query["to"].FirstOrDefault();
        var to = toParam is not null
            ? DateTimeOffset.Parse(toParam)
            : DateTimeOffset.UtcNow;

        log.LogDebug(
            "Query parking {Parking}, {From} - {To}",
            name,
            from.ToString("g"),
            to.ToString("g"));

        var result = await retrieveAndStore.GetByParking(
            name,
            from,
            to);
        
        log.LogDebug("Count ParkingInfos {Count}", result.Length);

        return new OkObjectResult(result);
    }
}