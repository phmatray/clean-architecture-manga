// <copyright file="AccountId.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Domain.ValueObjects;

using System;

/// <summary>
///     AccountId
///     <see href="https://github.com/ivanpaulovich/clean-architecture-manga/wiki/Domain-Driven-Design-Patterns#entity">
///         Entity
///         Design Pattern
///     </see>
///     .
/// </summary>
public readonly struct AccountId : IEquatable<AccountId>
{
    public Guid Id { get; }

    public AccountId(Guid id) =>
        Id = id;

    public override bool Equals(object? obj) =>
        obj is AccountId o && Equals(o);

    public bool Equals(AccountId other) => Id == other.Id;

    public override int GetHashCode() =>
        HashCode.Combine(Id);

    public static bool operator ==(AccountId left, AccountId right) => left.Equals(right);

    public static bool operator !=(AccountId left, AccountId right) => !(left == right);

    public override string ToString() => Id.ToString();
}
