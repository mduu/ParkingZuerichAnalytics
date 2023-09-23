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
        foreach (var parkingInfo in retriever.Retrieve())
        {
            telemetryClient.TrackEvent(
                "FreeParkingSlots",
                new Dictionary<string, string>
                {
                    { "parking", parkingInfo.Name }
                },
                new Dictionary<string, double>
                {
                    { "FreeParkingSlots", parkingInfo.CountFreeSlots }
                });
            
            telemetryClient.TrackMetric(
                "FreeParkingSlots",
                parkingInfo.CountFreeSlots,
                new Dictionary<string, string>
                {
                    { "parking", parkingInfo.Name }
                });
            
            telemetryClient
                .GetMetric($"FreeParkingSlots_{parkingInfo.Name}")
                .TrackValue(metricValue: parkingInfo.CountFreeSlots);
        }

        return Task.CompletedTask;
    }
}