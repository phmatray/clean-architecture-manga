// <copyright file="MangaContextFake.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Infrastructure.DataAccess;

using System;
using System.Collections.ObjectModel;
using Domain;
using Domain.Credits;
using Domain.Debits;
using Domain.ValueObjects;

/// <summary>
/// </summary>
public sealed class MangaContextFake
{
    /// <summary>
    /// </summary>
    public MangaContextFake()
    {
        var credit = new Credit(
            new CreditId(Guid.NewGuid()),
            SeedData.DefaultAccountId,
            DateTime.Now,
            800,
            Currency.Dollar.Code);

        var debit = new Debit(
            new DebitId(Guid.NewGuid()),
            SeedData.DefaultAccountId,
            DateTime.Now,
            300,
            Currency.Dollar.Code);

        var account = new Account(
            SeedData.DefaultAccountId,
            SeedData.DefaultExternalUserId,
            Currency.Dollar);

        account.CreditsCollection.Add(credit);
        account.DebitsCollection.Add(debit);

        Accounts.Add(account);
        Credits.Add(credit);
        Debits.Add(debit);

        var account2 = new Account(
            SeedData.SecondAccountId,
            SeedData.SecondExternalUserId,
            Currency.Dollar);

        Accounts.Add(account2);
    }

    /// <summary>
    ///     Gets or sets Accounts.
    /// </summary>
    public Collection<Account> Accounts { get; } = new();

    /// <summary>
    ///     Gets or sets Credits.
    /// </summary>
    public Collection<Credit> Credits { get; } = new();

    /// <summary>
    ///     Gets or sets Debits.
    /// </summary>
    public Collection<Debit> Debits { get; } = new();
}
