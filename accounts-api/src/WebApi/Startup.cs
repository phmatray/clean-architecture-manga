namespace WebApi;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Modules;
using Modules.Common;
using Modules.Common.FeatureFlags;
using Modules.Common.Swagger;
using Prometheus;

/// <summary>
///     Startup.
/// </summary>
public sealed class Startup(IConfiguration configuration)
{
    /// <summary>
    ///     Configure dependencies from application.
    /// </summary>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddFeatureFlags(configuration); // should be the first one.
        services.AddInvalidRequestLogging();
        services.AddCurrencyExchange(configuration);
        services.AddSQLServer(configuration);
        services.AddHealthChecks(configuration);
        services.AddAuthentication(configuration);
        services.AddVersioning();
        services.AddSwagger();
        services.AddUseCases();
        services.AddCustomControllers();
        services.AddCustomCors();
        services.AddProxy();
        services.AddCustomDataProtection();
    }

    /// <summary>
    ///     Configure http request pipeline.
    /// </summary>
    public void Configure(
        IApplicationBuilder app,
        IWebHostEnvironment env,
        IApiVersionDescriptionProvider provider)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/api/V1/CustomError");
            app.UseHsts();
        }

        app.UseProxy(configuration);
        app.UseHealthChecks();
        app.UseCustomCors();
        app.UseCustomHttpMetrics();
        app.UseRouting();
        app.UseVersionedSwagger(provider, configuration, env);
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapMetrics();
        });
    }
}
