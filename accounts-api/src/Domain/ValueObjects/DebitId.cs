// <copyright file="DebitId.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Domain.ValueObjects;

using System;

/// <summary>
///     Debit
///     <see
///         href="https://github.com/ivanpaulovich/clean-architecture-manga/wiki/Domain-Driven-Design-Patterns#value-object">
///         Value
///         Object Domain-Driven Design Pattern
///     </see>
///     .
/// </summary>
public readonly struct DebitId(Guid id)
    : IEquatable<DebitId>
{
    public Guid Id { get; } = id;

    public override bool Equals(object? obj) =>
        obj is DebitId o && Equals(o);

    public bool Equals(DebitId other) => Id == other.Id;

    public override int GetHashCode() =>
        HashCode.Combine(Id);

    public static bool operator ==(DebitId left, DebitId right) => left.Equals(right);

    public static bool operator !=(DebitId left, DebitId right) => !(left == right);

    public override string ToString() => Id.ToString();
}
