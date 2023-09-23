using System.ServiceModel.Syndication;
using System.Xml;

namespace ParkingZuerichAnalytics.DataGathering.Core.Retrieval;

public class ParkingInfoRetriever
{
    private const string RssUrl = "https://www.pls-zh.ch/plsFeed/rss";

    public IEnumerable<ParkingInfo> Retrieve()
    {
        using var reader = XmlReader.Create(RssUrl);
        var feed = SyndicationFeed.Load(reader);

        foreach (var item in feed.Items)
        {
            var titleParts = item.Title.Text.Split("/", StringSplitOptions.TrimEntries);
            var summaryParts = item.Summary.Text.Split("/", StringSplitOptions.TrimEntries);

            int.TryParse(summaryParts[1], out var numberOfFreeSlots);

            var title = titleParts[0].StartsWith("Parkhaus")
                ? string.Join(' ', titleParts[0].Split(' ').Skip(1))
                : titleParts[0];

            yield return new ParkingInfo(
                title,
                summaryParts[0],
                numberOfFreeSlots);
        }
    }
}