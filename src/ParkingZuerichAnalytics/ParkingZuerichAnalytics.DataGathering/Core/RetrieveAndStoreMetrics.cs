using System.Threading.Tasks;
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

    public async Task RetrieveAndStore()
    {
        foreach (var parkingInfo in retriever.Retrieve())
        {
            var sample = new MetricTelemetry();
            sample.Name = "FreeParkingSlots";
            sample.Sum = parkingInfo.CountFreeSlots;
            sample.Properties["parking"] = parkingInfo.Name;

            telemetryClient.GetMetric("contentLength").TrackValue(sample);
        }
    }
}