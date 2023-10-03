using ParkingZuerichAnalytics.DataGathering.Core.Retrieval;
using ParkingZuerichAnalytics.DataGathering.Core.Storage;

namespace ParkingZuerichAnalytics.DataGathering.Core;

public class RetrieveAndStore
{
    private readonly ParkingInfoRetriever retriever;

    public RetrieveAndStore(ParkingInfoRetriever retriever)
    {
        this.retriever = retriever;
    }

    public async Task Update()
    {
        var serviceClient = TableStorageHelper.GetClient();
        var parkingInfoTable = serviceClient.GetParkingInfoTable();
        var parkingAddressTable = serviceClient.GetParkingAddressTable();

        foreach (var parkingInfo in retriever.Retrieve())
        {
            await parkingInfoTable.AddEntityAsync(ParkingEntity.Create(parkingInfo));
            await parkingAddressTable.UpsertEntityAsync(ParkingAddressEntity.Create(parkingInfo));
        }
    }
}