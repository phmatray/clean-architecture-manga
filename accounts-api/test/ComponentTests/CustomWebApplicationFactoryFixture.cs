namespace ComponentTests;

using System;

/// <summary>
/// </summary>
public sealed class CustomWebApplicationFactoryFixture : IDisposable
{
    /// <summary>
    /// </summary>
    public CustomWebApplicationFactory CustomWebApplicationFactory { get; } = new();

    public void Dispose() => CustomWebApplicationFactory?.Dispose();
}
