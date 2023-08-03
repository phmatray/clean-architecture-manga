namespace EndToEndTests;

using System;

/// <summary>
/// </summary>
public sealed class CustomWebApplicationFactoryFixture : IDisposable
{
    public CustomWebApplicationFactoryFixture() =>
        CustomWebApplicationFactory = new CustomWebApplicationFactory();

    /// <summary>
    /// </summary>
    public CustomWebApplicationFactory CustomWebApplicationFactory { get; }

    public void Dispose() => CustomWebApplicationFactory?.Dispose();
}
