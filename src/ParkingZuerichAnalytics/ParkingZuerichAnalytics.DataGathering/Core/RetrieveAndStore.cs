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

    public async Task Run()
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
}