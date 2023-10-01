namespace ParkingZuerichAnalytics.DataGathering.Core;

public class ConnectionStringHelper
{
    public const string ConnectionStringName = "ParkingData";
    
    public static string GetConnectionString()
    {
        string connectionString = Environment.GetEnvironmentVariable($"ConnectionStrings:{ConnectionStringName}", 
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