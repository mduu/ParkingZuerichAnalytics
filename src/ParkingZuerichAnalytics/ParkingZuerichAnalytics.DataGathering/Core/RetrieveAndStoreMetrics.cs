using Azure.Data.Tables;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using ParkingZuerichAnalytics.DataGathering.Core.Retrieval;

namespace ParkingZuerichAnalytics.DataGathering.Core;

public class RetrieveAndStoreMetrics
{
    private readonly ParkingInfoRetriever retriever;
    private readonly TelemetryClient telemetryClient;

    public RetrieveAndStoreMetrics(
        ParkingInfoRetriever retriever,
        TelemetryConfiguration telemetryConfiguration)
    {
        this.retriever = retriever;
        telemetryClient = new TelemetryClient(telemetryConfiguration);
    }

    public Task RetrieveAndStore()
    {
        var table = GetTable();

        foreach (var parkingInfo in retriever.Retrieve())
        {
            table.AddEntity(
                new TableEntity(
                    "ParkingInfos",
                    Guid.NewGuid().ToString())
                {
                    { "ParkingName", parkingInfo.Name },
                    { "Status", parkingInfo.Status },
                    { "CountFreeSlots", parkingInfo.CountFreeSlots },
                });
        }

        return Task.CompletedTask;
    }

    private static TableClient GetTable()
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings:datatableconnection");
        var serviceClient = new TableServiceClient(connectionString);
        TableClient table = serviceClient.GetTableClient("parking_info");
        table.CreateIfNotExists();
        
        return table;
    }
}