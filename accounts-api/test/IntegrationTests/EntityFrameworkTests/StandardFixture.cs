namespace IntegrationTests.EntityFrameworkTests;

using System;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

public sealed class StandardFixture : IDisposable
{
    public StandardFixture()
    {
        const string connectionString =
            "Server=localhost;User Id=sa;Password=<YourStrong!Passw0rd>;Database=Accounts;";

        var options = new DbContextOptionsBuilder<MangaContext>()
            .UseSqlServer(connectionString)
            .Options;

        Context = new MangaContext(options);
        Context
            .Database
            .EnsureCreated();
    }

    public MangaContext Context { get; }

    public void Dispose() => Context.Dispose();
}
