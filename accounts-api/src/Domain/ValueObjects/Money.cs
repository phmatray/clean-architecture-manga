// <copyright file="Money.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Domain.ValueObjects;

using System;

/// <summary>
///     Money
///     <see href="https://github.com/ivanpaulovich/clean-architecture-manga/wiki/Domain-Driven-Design-Patterns#entity">
///         Entity
///         Design Pattern
///     </see>
///     .
/// </summary>
public readonly struct Money(decimal amount, Currency currency)
    : IEquatable<Money>
{
    public decimal Amount { get; } = amount;
    public Currency Currency { get; } = currency;

    public override bool Equals(object? obj) =>
        obj is Money o && Equals(o);

    public bool Equals(Money other) =>
        Amount == other.Amount &&
        Currency == other.Currency;

    public override int GetHashCode() =>
        HashCode.Combine(Amount, Currency);

    public static bool operator ==(Money left, Money right) => left.Equals(right);

    public static bool operator !=(Money left, Money right) => !(left == right);

    public bool IsZero() => Amount == 0;

    public Money Subtract(Money debit)
        => new(Math.Round(Amount - debit.Amount, 2), Currency);

    public Money Add(Money amount) => new(Math.Round(Amount + amount.Amount, 2), Currency);

    public override string ToString() => string.Format($"{Amount} {Currency}");
}
