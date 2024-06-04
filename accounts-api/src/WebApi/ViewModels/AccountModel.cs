namespace WebApi.ViewModels;

using System;
using System.ComponentModel.DataAnnotations;
using Domain;

/// <summary>
///     Account Details.
/// </summary>
public sealed class AccountModel(Account account)
{
    /// <summary>
    ///     Gets account ID.
    /// </summary>
    [Required]
    public Guid AccountId { get; } = account.AccountId.Id;

    /// <summary>
    ///     Gets current Balance.
    /// </summary>
    [Required]
    public decimal CurrentBalance { get; } = account.GetCurrentBalance().Amount;

    /// <summary>
    ///     Gets current Balance.
    /// </summary>
    [Required]
    public string Currency { get; } = account.Currency.Code;
}
