// <copyright file="Account.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Domain;

using Credits;
using Debits;
using ValueObjects;

/// <inheritdoc />
public class Account(AccountId accountId, string externalUserId, Currency currency)
    : IAccount
{
    /// <summary>
    ///     Gets the ExternalUserId.
    /// </summary>
    public string ExternalUserId { get; } = externalUserId;

    /// <summary>
    ///     Gets the Credits List.
    /// </summary>
    public CreditsCollection CreditsCollection { get; } = [];

    /// <summary>
    ///     Gets the Debits List.
    /// </summary>
    public DebitsCollection DebitsCollection { get; } = [];

    /// <summary>
    ///     Gets the Currency.
    /// </summary>
    public Currency Currency { get; } = currency;

    /// <inheritdoc />
    public AccountId AccountId { get; } = accountId;

    /// <inheritdoc />
    public void Deposit(Credit credit) => CreditsCollection.Add(credit);

    /// <inheritdoc />
    public void Withdraw(Debit debit) => DebitsCollection.Add(debit);

    /// <inheritdoc />
    public bool IsClosingAllowed() => GetCurrentBalance().IsZero();

    /// <inheritdoc />
    public Money GetCurrentBalance()
    {
        Money totalCredits = CreditsCollection.GetTotal();
        Money totalDebits = DebitsCollection.GetTotal();
        Money totalAmount = totalCredits.Subtract(totalDebits);

        return totalAmount;
    }
}
