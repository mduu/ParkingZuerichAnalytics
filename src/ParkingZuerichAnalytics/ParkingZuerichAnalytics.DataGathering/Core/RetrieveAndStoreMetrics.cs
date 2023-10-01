using Azure.Data.Tables;
using ParkingZuerichAnalytics.DataGathering.Core.Retrieval;

namespace ParkingZuerichAnalytics.DataGathering.Core;

public class RetrieveAndStoreMetrics
{
    private readonly ParkingInfoRetriever retriever;
    private readonly string azureTableStorageConnectionString;

    public RetrieveAndStoreMetrics(
        ParkingInfoRetriever retriever)
    {
        this.retriever = retriever;
        azureTableStorageConnectionString = ConnectionStringHelper.GetConnectionString();
    }

    public Task RetrieveAndStore()
    {
        var table = GetTable();

        foreach (var parkingInfo in retriever.Retrieve())
        {
            table.AddEntity(ParkingEntity.Create(parkingInfo));
        }

        return Task.CompletedTask;
    }

    private TableClient GetTable()
    {
        var serviceClient = new TableServiceClient(azureTableStorageConnectionString);
        var table = serviceClient.GetTableClient("parkinginfo");

#if DEBUG
        table.CreateIfNotExists();
#endif

        return table;
    }
}