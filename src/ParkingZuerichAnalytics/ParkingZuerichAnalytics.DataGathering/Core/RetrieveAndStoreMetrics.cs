using Microsoft.ApplicationInsights;
using ParkingZuerichAnalytics.DataGathering.Core.Retrieval;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

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
        foreach (var parkingInfo in retriever.Retrieve())
        {
            telemetryClient
                .GetMetric("FreeParkingSlots")
                .TrackValue(parkingInfo.CountFreeSlots, parkingInfo.Name);
        }

        return Task.CompletedTask;
    }
}