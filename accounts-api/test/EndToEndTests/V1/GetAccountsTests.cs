namespace EndToEndTests.V1;

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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

        Assert.Equal(HttpStatusCode.OK, actualResponse.StatusCode);
    }
}
