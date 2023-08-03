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
public class Debit : IDebit
{
    public Debit(DebitId DebitId, AccountId accountId, DateTime transactionDate, decimal value, string currency)
    {
        this.DebitId = DebitId;
        AccountId = accountId;
        TransactionDate = transactionDate;
        Amount = new Money(value, new Currency(currency));
    }

    /// <summary>
    ///     Gets Description.
    /// </summary>
    public static string Description => "Debit";

    /// <summary>
    ///     Gets or sets Transaction Date.
    /// </summary>
    public DateTime TransactionDate { get; }

    /// <summary>
    ///     Gets the AccountId.
    /// </summary>
    public AccountId AccountId { get; }

    public Account? Account { get; set; }

    public decimal Value => Amount.Amount;

    public string Currency => Amount.Currency.Code;

    /// <summary>
    ///     Gets or sets Id.
    /// </summary>
    public DebitId DebitId { get; }

    /// <summary>
    ///     Gets or sets Amount.
    /// </summary>
    public Money Amount { get; }

    /// <summary>
    ///     Calculate the sum of positive amounts.
    /// </summary>
    /// <param name="amount">Positive amount.</param>
    /// <returns>The positive sum.</returns>
    public Money Sum(Money amount) => Amount.Add(amount);
}
