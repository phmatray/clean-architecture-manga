using FluentAssertions;

namespace ComponentTests.V1;

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

[Collection("WebApi Collection")]
public sealed class GetAccountsTests(CustomWebApplicationFactoryFixture fixture)
{
    [Fact]
    public async Task GetAccountsReturnsList()
    {
        HttpClient client = fixture
            .CustomWebApplicationFactory
            .CreateClient();

        HttpResponseMessage actualResponse = await client.GetAsync("/api/v1/Accounts/");
        string actualResponseString = await actualResponse.Content.ReadAsStringAsync();

        actualResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        using var stringReader = new StringReader(actualResponseString);
        await using var reader = new JsonTextReader(stringReader);
        reader.DateParseHandling = DateParseHandling.None;
        
        JObject jsonResponse = await JObject.LoadAsync(reader);

        jsonResponse["accounts"]![0]!["accountId"]!.Type.Should().Be(JTokenType.String);
        jsonResponse["accounts"]![0]!["currentBalance"]!.Type.Should().Be(JTokenType.Integer);

        Guid.TryParse(jsonResponse["accounts"]![0]!["accountId"]!.Value<string>(), out _).Should().BeTrue();
        decimal.TryParse(jsonResponse["accounts"]![0]!["currentBalance"]!.Value<string>(), out _).Should().BeTrue();
    }
}
