using DbUp;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace FootballStatsApi.Database
{
    class MainClass
    {
        public static int Main(string[] args)
        {
            Console.WriteLine("Start");
            
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables()
                .Build();

            var upgrader =
                DeployChanges.To
                    .PostgresqlDatabase(configuration.GetConnectionString("Football"))
                    .WithTransaction()
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .LogToConsole()
                    .Build();


            var isConnected = false;

            while (!isConnected)
            {
                isConnected = upgrader.TryConnect(out var error);
                if (!isConnected)
                {
                    LogError($"Unable to connect to db. {error}");
                    Task.Delay(5000).Wait();
                } else
                {
                    LogSuccess("Connected");
                }
            }

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                LogError(result.Error.ToString());
                return -1;
            }

            LogSuccess("Success!");

            return 0;
        }

        private static void LogError(string error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(error);
            Console.ResetColor();
        }

        private static void LogSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
