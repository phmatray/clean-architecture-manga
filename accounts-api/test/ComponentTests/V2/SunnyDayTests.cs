namespace ComponentTests.V2;

using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

public sealed class SunnyDayTests(CustomWebApplicationFactory factory)
    : IClassFixture<CustomWebApplicationFactory>
{
    private async Task<Tuple<Guid, decimal>> GetAccounts()
    {
        HttpClient client = factory.CreateClient();
        HttpResponseMessage actualResponse = await client
            .GetAsync("/api/v1/Accounts/")
            .ConfigureAwait(false);

        string actualResponseString = await actualResponse.Content
            .ReadAsStringAsync()
            .ConfigureAwait(false);

        using var stringReader = new StringReader(actualResponseString);
        await using var reader = new JsonTextReader(stringReader);
        reader.DateParseHandling = DateParseHandling.None;

        JObject jsonResponse = await JObject.LoadAsync(reader)
            .ConfigureAwait(false);

        Guid.TryParse(jsonResponse["accounts"]![0]!["accountId"]!.Value<string>(), out Guid accountId);
        decimal.TryParse(jsonResponse["accounts"]![0]!["currentBalance"]!.Value<string>(),
            out decimal currentBalance);

        return new Tuple<Guid, decimal>(accountId, currentBalance);
    }

    private async Task GetAccount(string accountId)
    {
        HttpClient client = factory.CreateClient();
        await client.GetAsync($"/api/v2/Accounts/{accountId}")
            .ConfigureAwait(false);
    }

    [Fact]
    public async Task GetAccounts_GetAccount()
    {
        var account = await GetAccounts();
        await GetAccount(account.Item1.ToString());
    }
}
