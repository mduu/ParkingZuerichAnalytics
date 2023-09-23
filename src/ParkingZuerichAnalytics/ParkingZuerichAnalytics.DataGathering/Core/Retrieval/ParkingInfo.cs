namespace ParkingZuerichAnalytics.DataGathering.Core.Retrieval;

public record ParkingInfo(
    string Name,
    string Status,
    int CountFreeSlots)
{
    public const string StatusOpen = "open";
}