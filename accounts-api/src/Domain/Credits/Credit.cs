// <copyright file="Credit.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Domain.Credits;

using System;
using ValueObjects;

/// <summary>
///     Credit
///     <see href="https://github.com/ivanpaulovich/clean-architecture-manga/wiki/Domain-Driven-Design-Patterns#entity">
///         Entity
///         Design Pattern
///     </see>
///     .
/// </summary>
public class Credit(CreditId creditId, AccountId accountId, DateTime transactionDate, decimal value, string currency)
    : ICredit
{
    /// <summary>
    ///     Gets Description.
    /// </summary>
    public static string Description => "Credit";

    /// <summary>
    ///     Gets or sets Transaction Date.
    /// </summary>
    public DateTime TransactionDate { get; } = transactionDate;

    /// <summary>
    ///     Gets or sets AccountId.
    /// </summary>
    public AccountId AccountId { get; } = accountId;

    public Account? Account { get; set; }

    public decimal Value => Amount.Amount;

    public string Currency => Amount.Currency.Code;

    /// <summary>
    ///     Gets or sets Id.
    /// </summary>
    public CreditId CreditId { get; } = creditId;

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
