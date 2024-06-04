// <copyright file="MangaContext.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Infrastructure.DataAccess;

using System;
using Domain;
using Domain.Credits;
using Domain.Debits;
using Microsoft.EntityFrameworkCore;

/// <inheritdoc />
public sealed class MangaContext(DbContextOptions options)
    : DbContext(options)
{
    /// <summary>
    ///     Gets or sets Accounts
    /// </summary>
    public DbSet<Account> Accounts { get; init; }

    /// <summary>
    ///     Gets or sets Credits
    /// </summary>
    public DbSet<Credit> Credits { get; init; }

    /// <summary>
    ///     Gets or sets Debits
    /// </summary>
    public DbSet<Debit> Debits { get; init; }

    /// <summary>
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MangaContext).Assembly);
        SeedData.Seed(modelBuilder);
    }
}
