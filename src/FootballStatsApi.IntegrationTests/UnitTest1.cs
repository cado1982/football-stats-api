using FootballStatsApi.Domain;
using FootballStatsApi.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FootballStatsApi.IntegrationTests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            var endpoint = Environment.GetEnvironmentVariable("ApiEndpoint");

            await SetupUser();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(endpoint);
                var request = new HttpRequestMessage();
                request.Headers.Add("X-API-Key", Guid.Empty.ToString());
                request.RequestUri = new Uri("v1/competitions");
                var response = await client.SendAsync(request);

                Console.WriteLine(response.StatusCode);
                Console.WriteLine(response.Content);

                Assert.True(response.IsSuccessStatusCode);
            }
        }

        private async Task SetupUser()
        {
            var dbConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings__Football");

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql(dbConnectionString);
            using (var context = new ApplicationDbContext(optionsBuilder.Options))
            {
                var user = new User();
                user.Id = 1;
                user.ApiKey = Guid.Empty;
                context.Users.Add(user);
                await context.SaveChangesAsync();
            }
        }
    }
}
