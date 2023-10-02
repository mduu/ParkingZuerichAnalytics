using Azure;
using ParkingZuerichAnalytics.DataGathering.Core.Retrieval;

namespace ParkingZuerichAnalytics.DataGathering.Core;

public class ParkingAddressEntity : Azure.Data.Tables.ITableEntity
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    public string Title { get; set; }
    public string Address { get; set; }
    public string Url { get; set; }

    public static ParkingAddressEntity Create(ParkingInfo parkingInfo)
        => new()
        {
            PartitionKey = "ParkingAddress",
            RowKey = parkingInfo.Name,
            Timestamp = DateTimeOffset.Now,
            ETag = default,
            Title = parkingInfo.Title,
            Address = parkingInfo.Address,
            Url = parkingInfo.Url,
        };
}