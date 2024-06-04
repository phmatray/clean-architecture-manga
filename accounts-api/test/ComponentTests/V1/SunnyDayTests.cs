namespace ComponentTests.V1;

using System;
using System.Collections.Generic;
using System.Globalization;
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

    private async Task<Tuple<Guid, decimal>> GetAccount(string accountId)
    {
        HttpClient client = factory.CreateClient();
        string actualResponseString = await client
            .GetStringAsync($"/api/v1/Accounts/{accountId}")
            .ConfigureAwait(false);

        using var stringReader = new StringReader(actualResponseString);
        await using var reader = new JsonTextReader(stringReader);
        reader.DateParseHandling = DateParseHandling.None;

        JObject jsonResponse = await JObject.LoadAsync(reader)
            .ConfigureAwait(false);

        Guid.TryParse(jsonResponse["account"]!["accountId"]!.Value<string>(), out Guid getAccountId);
        decimal.TryParse(jsonResponse["account"]!["currentBalance"]!.Value<string>(), out decimal currentBalance);

        return new Tuple<Guid, decimal>(getAccountId, currentBalance);
    }

    private async Task Deposit(string account, decimal amount)
    {
        HttpClient client = factory.CreateClient();
        var content = new FormUrlEncodedContent(new[]
        {
                new KeyValuePair<string, string>("amount", amount.ToString(CultureInfo.InvariantCulture)),
                new KeyValuePair<string, string>("currency", "USD")
            });

        HttpResponseMessage response = await client.PatchAsync($"api/v1/Transactions/{account}/Deposit", content)
            .ConfigureAwait(false);

        string result = await response.Content
            .ReadAsStringAsync()
            .ConfigureAwait(false);

        response.EnsureSuccessStatusCode();
    }

    private async Task Withdraw(string account, decimal amount)
    {
        HttpClient client = factory.CreateClient();

        var content = new FormUrlEncodedContent(new[]
        {
                new KeyValuePair<string, string>("amount", amount.ToString(CultureInfo.InvariantCulture)),
                new KeyValuePair<string, string>("currency", "USD")
            });

        HttpResponseMessage response = await client.PatchAsync($"api/v1/Transactions/{account}/Withdraw", content)
            .ConfigureAwait(false);

        string responseBody = await response.Content
            .ReadAsStringAsync()
            .ConfigureAwait(false);

        response.EnsureSuccessStatusCode();
    }

    private async Task Close(string account)
    {
        HttpClient client = factory.CreateClient();
        HttpResponseMessage response = await client.DeleteAsync($"api/v1/Accounts/{account}")
            .ConfigureAwait(false);

        string responseBody = await response.Content
            .ReadAsStringAsync()
            .ConfigureAwait(false);

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetAccount_Withdraw_Deposit_Withdraw_Withdraw_Close()
    {
        var account = await GetAccounts();
        await GetAccount(account.Item1.ToString());
        await Withdraw(account.Item1.ToString(), account.Item2);
        await Deposit(account.Item1.ToString(), 500);
        await Deposit(account.Item1.ToString(), 300);
        await Withdraw(account.Item1.ToString(), 400);
        await Withdraw(account.Item1.ToString(), 400);
        account = await GetAccounts();
        await Close(account.Item1.ToString());
    }
}
