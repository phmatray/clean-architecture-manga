namespace WebApi.ViewModels;

using System;
using System.ComponentModel.DataAnnotations;
using Domain.Credits;

/// <summary>
///     Credit.
/// </summary>
public sealed class CreditModel
{
    /// <summary>
    ///     Credit constructor.
    /// </summary>
    public CreditModel(Credit credit)
    {
        TransactionId = credit.CreditId.Id;
        Amount = credit.Amount.Amount;
        Currency = credit.Amount.Currency.Code;
        Description = "Credit";
        TransactionDate = credit.TransactionDate;
    }

    /// <summary>
    ///     Gets the TransactionId.
    /// </summary>
    [Required]
    public Guid TransactionId { get; }

    /// <summary>
    ///     Gets the Amount.
    /// </summary>
    [Required]
    public decimal Amount { get; }

    /// <summary>
    ///     Gets the Currency.
    /// </summary>
    [Required]
    public string Currency { get; }

    /// <summary>
    ///     Gets the Description.
    /// </summary>
    [Required]
    public string Description { get; }

    /// <summary>
    ///     Gets the Transaction Date.
    /// </summary>
    [Required]
    public DateTime TransactionDate { get; }
}
