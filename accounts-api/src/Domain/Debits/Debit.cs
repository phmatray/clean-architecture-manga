// <copyright file="Debit.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Domain.Debits;

using System;
using ValueObjects;

/// <summary>
///     Debit
///     <see href="https://github.com/ivanpaulovich/clean-architecture-manga/wiki/Domain-Driven-Design-Patterns#entity">
///         Entity
///         Design Pattern
///     </see>
///     .
/// </summary>
public class Debit(DebitId debitId, AccountId accountId, DateTime transactionDate, decimal value, string currency)
    : IDebit
{
    /// <summary>
    ///     Gets Description.
    /// </summary>
    public static string Description => "Debit";

    /// <summary>
    ///     Gets or sets Transaction Date.
    /// </summary>
    public DateTime TransactionDate { get; } = transactionDate;

    /// <summary>
    ///     Gets the AccountId.
    /// </summary>
    public AccountId AccountId { get; } = accountId;

    public Account? Account { get; set; }

    public decimal Value => Amount.Amount;

    public string Currency => Amount.Currency.Code;

    /// <summary>
    ///     Gets or sets Id.
    /// </summary>
    public DebitId DebitId { get; } = debitId;

    /// <summary>
    ///     Gets or sets Amount.
    /// </summary>
    public Money Amount { get; } = new(value, new Currency(currency));

    /// <summary>
    ///     Calculate the sum of positive amounts.
    /// </summary>
    /// <param name="amount">Positive amount.</param>
    /// <returns>The positive sum.</returns>
    public Money Sum(Money amount) => Amount.Add(amount);
}
