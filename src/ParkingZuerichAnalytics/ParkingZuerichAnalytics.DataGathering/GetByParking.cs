using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ParkingZuerichAnalytics.DataGathering.Core.Storage;

namespace ParkingZuerichAnalytics.DataGathering;

public class GetByParking
{
    [FunctionName("GetByParking")]
    public async Task<IActionResult> Run(
        [HttpTrigger(
            AuthorizationLevel.Anonymous,
            "get",
            Route = "parking/{name}")]
        HttpRequest req,
        string name,
        ILogger log)
    {
        log.LogInformation("Request for {Function}", nameof(GetByParking));

        var from = GetDateTimeFromQueryWithDefault(req.Query, "from", DateTimeOffset.UtcNow.AddDays(-14));
        if (from is null)
        {
            return new BadRequestErrorMessageResult("'from' is not a valid date/time!");
        }

        var to = GetDateTimeFromQueryWithDefault(req.Query, "to", DateTimeOffset.UtcNow);
        if (to is null)
        {
            return new BadRequestErrorMessageResult("'to' is not a valid date/time!");
        }

        log.LogDebug(
            "Query parking {Parking}, {From} - {To}",
            name,
            from.Value.ToString("g"),
            to.Value.ToString("g"));

        var serviceClient = TableStorageHelper.GetClient();
        var parkingInfoTable = serviceClient.GetParkingInfoTable();

        var result = await parkingInfoTable.QueryAsync<ParkingEntity>(
                x => x.PartitionKey == name)
            .Where(p =>
                p.Timestamp >= from &&
                p.Timestamp <= to)
            .Select(e => new
            {
                e.ParkingName,
                e.Timestamp,
                e.CountFreeSlots,
                e.Status,
            })
            .ToArrayAsync();

        log.LogDebug("Count ParkingInfos {Count}", result.Count());

        return new OkObjectResult(result);
    }

    private DateTimeOffset? GetDateTimeFromQueryWithDefault(
        IQueryCollection query,
        string queryFieldName,
        DateTimeOffset defaultValue)
    {
        var queryParamValue = query[queryFieldName].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(queryParamValue))
        {
            return defaultValue;
        }

        if (!DateTimeOffset.TryParse(queryParamValue, out var dateTimeOffsetValue))
        {
            return null;
        }

        return dateTimeOffsetValue;
    }
}