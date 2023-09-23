using System.Threading.Tasks;
using ParkingZuerichAnalytics.DataGathering.Core.Retrieval;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace ParkingZuerichAnalytics.DataGathering.Core;

public class RetrieveAndStoreMetrics
{
    private readonly ParkingInfoRetriever retriever;
    private TelemetryClient telemetry = new();

    public RetrieveAndStoreMetrics(ParkingInfoRetriever retriever)
    {
        this.retriever = retriever;
    }

    public async Task RetrieveAndStore()
    {
        foreach (var parkingInfo in retriever.Retrieve())
        {
            var sample = new MetricTelemetry();
            sample.Name = "FreeParkingSlots";
            sample.Sum = parkingInfo.CountFreeSlots;
            sample.Properties["parking"] = parkingInfo.Name;

            telemetry.TrackMetric(sample);
        }
    }
}