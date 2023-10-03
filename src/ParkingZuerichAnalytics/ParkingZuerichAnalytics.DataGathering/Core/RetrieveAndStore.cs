using Azure.Data.Tables;
using ParkingZuerichAnalytics.DataGathering.Core.Retrieval;

namespace ParkingZuerichAnalytics.DataGathering.Core;

public class RetrieveAndStore
{
    private readonly ParkingInfoRetriever retriever;
    private readonly string azureTableStorageConnectionString;

    public RetrieveAndStore(
        ParkingInfoRetriever retriever)
    {
        this.retriever = retriever;
        azureTableStorageConnectionString = ConnectionStringHelper.GetConnectionString();
    }

    public async Task Update()
    {
        var serviceClient = new TableServiceClient(azureTableStorageConnectionString);
        var parkingInfoTable = serviceClient.GetTableClient("parkinginfo");
        var parkingAddressTable = serviceClient.GetTableClient("parkingaddress");

        foreach (var parkingInfo in retriever.Retrieve())
        {
            await parkingInfoTable.AddEntityAsync(ParkingEntity.Create(parkingInfo));
            await parkingAddressTable.UpsertEntityAsync(ParkingAddressEntity.Create(parkingInfo));
        }
    }

    public async Task<ParkingEntity[]> GetByParking(
        string parkingName,
        DateTimeOffset fromTime,
        DateTimeOffset toTime)
    {
        var serviceClient = new TableServiceClient(azureTableStorageConnectionString);
        var parkingInfoTable = serviceClient.GetTableClient("parkinginfo");

        return await parkingInfoTable.QueryAsync<ParkingEntity>(
            x => x.PartitionKey == parkingName)
            // .Where(p => 
            //     p.Timestamp is null ||(
            //     p.Timestamp.Value >= fromTime &&
            //     p.Timestamp.Value <= toTime))
            .ToArrayAsync();
    }
}