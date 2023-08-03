namespace Domain.ValueObjects;

using System;

/// <summary>
///     Currency
///     <see
///         href="https://github.com/ivanpaulovich/clean-architecture-manga/wiki/Domain-Driven-Design-Patterns#value-object">
///         Value Object
///         Design Pattern
///     </see>
///     .
/// </summary>
public readonly struct Currency(string code)
    : IEquatable<Currency>
{
    public string Code { get; } = code;

    public override bool Equals(object? obj) =>
        obj is Currency o && Equals(o);

    public bool Equals(Currency other) => Code == other.Code;

    public override int GetHashCode() =>
        HashCode.Combine(Code);

    public static bool operator ==(Currency left, Currency right) => left.Equals(right);

    public static bool operator !=(Currency left, Currency right) => !(left == right);

    /// <summary>
    ///     Dollar.
    /// </summary>
    /// <returns>Currency.</returns>
    public static readonly Currency Dollar = new("USD");

    /// <summary>
    ///     Euro.
    /// </summary>
    /// <returns>Currency.</returns>
    public static readonly Currency Euro = new("EUR");

    /// <summary>
    ///     British Pound.
    /// </summary>
    /// <returns>Currency.</returns>
    public static readonly Currency BritishPound = new("GBP");

    /// <summary>
    ///     Canadian Dollar.
    /// </summary>
    /// <returns>Currency.</returns>
    public static readonly Currency Canadian = new("CAD");

    /// <summary>
    ///     Brazilian Real.
    /// </summary>
    /// <returns>Currency.</returns>
    public static readonly Currency Real = new("BRL");

    /// <summary>
    ///     Swedish Krona.
    /// </summary>
    /// <returns>Currency.</returns>
    public static readonly Currency Krona = new("SEK");

    public override string ToString() => Code;
}
