namespace ParkingZuerichAnalytics.DataGathering.Core.Retrieval;

public record ParkingInfo(
    string Name,
    string Status,
    int CountFreeSlots,
    string Title,
    string Address,
    string Url)
{
    public const string StatusOpen = "open";
}