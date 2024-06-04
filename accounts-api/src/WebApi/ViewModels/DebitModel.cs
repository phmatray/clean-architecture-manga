namespace WebApi.ViewModels;

using System;
using System.ComponentModel.DataAnnotations;
using Domain.Debits;

/// <summary>
///     Debit.
/// </summary>
public sealed class DebitModel(Debit credit)
{
    /// <summary>
    ///     Gets the TransactionId.
    /// </summary>
    [Required]
    public Guid TransactionId { get; } = credit.DebitId.Id;

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
    public string Description { get; } = "Debit";

    /// <summary>
    ///     Gets the Transaction Date.
    /// </summary>
    [Required]
    public DateTime TransactionDate { get; } = credit.TransactionDate;
}
