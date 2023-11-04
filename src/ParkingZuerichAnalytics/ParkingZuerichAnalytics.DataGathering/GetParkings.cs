using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ParkingZuerichAnalytics.DataGathering.Core.Storage;

namespace ParkingZuerichAnalytics.DataGathering;

public static class GetParkings
{
    [FunctionName("GetParkings")]
    public static IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "parking")]
        HttpRequest req,
        ILogger log)
    {
        log.LogInformation("Request for {Function}", nameof(GetParkings));

        var client = TableStorageHelper.GetClient();
        var parkingAddressTable = client.GetParkingAddressTable();

        var result = parkingAddressTable.Query<ParkingAddressEntity>(
            x => x.PartitionKey == ParkingAddressEntity.ThePartitionKey)
            .Select(e => new
            {
                ParkingName = e.RowKey,
                e.Title,
                e.Address,
                e.Url,
            });
        
        log.LogDebug("Count parkings {Count}", result.Count());

        return new OkObjectResult(result);
    }
}