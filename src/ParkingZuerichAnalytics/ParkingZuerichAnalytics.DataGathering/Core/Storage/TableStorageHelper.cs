using Azure.Data.Tables;

namespace ParkingZuerichAnalytics.DataGathering.Core.Storage;

public static class TableStorageHelper
{
    public const string ConnectionStringName = "ParkingData";

    public static TableServiceClient GetClient() => new(GetConnectionString());

    public static TableClient GetParkingInfoTable(this TableServiceClient client)
        => client.GetTableClient("parkinginfo");

    public static TableClient GetParkingAddressTable(this TableServiceClient client)
        => client.GetTableClient("parkingaddress");

    private static string GetConnectionString()
    {
        var connectionString = Environment.GetEnvironmentVariable($"ConnectionStrings:{ConnectionStringName}",
            EnvironmentVariableTarget.Process);

        // Azure Functions App Service 
        if (string.IsNullOrEmpty(connectionString))
        {
            connectionString = Environment.GetEnvironmentVariable($"CUSTOMCONNSTR_{ConnectionStringName}",
                EnvironmentVariableTarget.Process);
        }

        return connectionString;
    }
}