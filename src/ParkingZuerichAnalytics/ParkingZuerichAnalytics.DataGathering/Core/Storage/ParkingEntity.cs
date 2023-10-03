using Azure;
using ParkingZuerichAnalytics.DataGathering.Core.Retrieval;

namespace ParkingZuerichAnalytics.DataGathering.Core.Storage;

public class ParkingEntity : Azure.Data.Tables.ITableEntity
{
    public const string StatusOpen = "open";

    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    public string ParkingName { get; set; }
    public string Status { get; set; } = StatusOpen;
    public int CountFreeSlots { get; set; }

    public static ParkingEntity Create(ParkingInfo parkingInfo)
        => new()
        {
            PartitionKey = parkingInfo.Name,
            RowKey = DateTimeOffset.Now.ToString("u"),
            Timestamp = DateTimeOffset.Now,
            ETag = default,
            ParkingName = parkingInfo.Name,
            Status = parkingInfo.Status,
            CountFreeSlots = parkingInfo.CountFreeSlots
        };
}