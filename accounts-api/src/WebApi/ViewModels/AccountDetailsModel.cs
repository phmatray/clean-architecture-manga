namespace WebApi.ViewModels;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Domain;

/// <summary>
///     Account Details.
/// </summary>
public sealed class AccountDetailsModel(Account account)
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
    ///     Gets Credits.
    /// </summary>
    [Required]
    public List<CreditModel> Credits { get; } = account
        .CreditsCollection
        .Select(e => new CreditModel(e))
        .ToList();

    /// <summary>
    ///     Gets Debits.
    /// </summary>
    [Required]
    public List<DebitModel> Debits { get; } = account
        .DebitsCollection
        .Select(e => new DebitModel(e))
        .ToList();
}
