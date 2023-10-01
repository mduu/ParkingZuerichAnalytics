namespace ParkingZuerichAnalytics.DataGathering.Core;

public class ConnectionStringHelper
{
    public const string ConnectionStringName = "datatableconnection";
    
    public static string GetConnectionString()
    {
        string connectionString = Environment.GetEnvironmentVariable($"ConnectionStrings:{ConnectionStringName}", 
            EnvironmentVariableTarget.Process);

        // Azure Functions App Service 
        if (string.IsNullOrEmpty(connectionString))
        {
            connectionString = Environment.GetEnvironmentVariable($"{ConnectionStringName}",
                EnvironmentVariableTarget.Process);
        }

        return connectionString;
    }
}