namespace WebApi.ViewModels;

using System;
using System.ComponentModel.DataAnnotations;
using Domain.Credits;

/// <summary>
///     Credit.
/// </summary>
public sealed class CreditModel(Credit credit)
{
    /// <summary>
    ///     Gets the TransactionId.
    /// </summary>
    [Required]
    public Guid TransactionId { get; } = credit.CreditId.Id;

    /// <summary>
    ///     Gets the Amount.
    /// </summary>
    [Required]
    public decimal Amount { get; } = credit.Amount.Amount;

    /// <summary>
    ///     Gets the Currency.
    /// </summary>
    [Required]
    public string Currency { get; } = credit.Amount.Currency.Code;

    /// <summary>
    ///     Gets the Description.
    /// </summary>
    [Required]
    public string Description { get; } = "Credit";

    /// <summary>
    ///     Gets the Transaction Date.
    /// </summary>
    [Required]
    public DateTime TransactionDate { get; } = credit.TransactionDate;
}
