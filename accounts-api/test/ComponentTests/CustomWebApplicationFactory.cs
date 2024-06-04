namespace ComponentTests;

using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using WebApi;

/// <summary>
///     Custom WebApplicationFactory for testing.
/// </summary>
public sealed class CustomWebApplicationFactory
    : WebApplicationFactory<Startup>
{
    private static IReadOnlyDictionary<string, string> DefaultFeatures => new Dictionary<string, string>
    {
        ["FeatureManagement:SQLServer"] = "false",
        ["FeatureManagement:CurrencyExchangeModule"] = "false"
    };

    /// <summary>
    /// Configures the web host by adding in-memory configuration.
    /// </summary>
    /// <param name="builder">The IWebHostBuilder instance to configure.</param>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(
            (context, config) => config.AddInMemoryCollection(DefaultFeatures));
    }
}
