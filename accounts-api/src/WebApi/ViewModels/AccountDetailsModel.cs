namespace WebApi.ViewModels;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Domain;

/// <summary>
///     Account Details.
/// </summary>
public sealed class AccountDetailsModel
{
    /// <summary>
    ///     Account Details constructor.
    /// </summary>
    public AccountDetailsModel(Account account)
    {
        AccountId = account.AccountId.Id;
        CurrentBalance = account.GetCurrentBalance().Amount;
        Credits = account
            .CreditsCollection
            .Select(e => new CreditModel(e))
            .ToList();

        Debits = account
            .DebitsCollection
            .Select(e => new DebitModel(e))
            .ToList();
    }

    /// <summary>
    ///     Gets account ID.
    /// </summary>
    [Required]
    public Guid AccountId { get; }

    /// <summary>
    ///     Gets current Balance.
    /// </summary>
    [Required]
    public decimal CurrentBalance { get; }

    /// <summary>
    ///     Gets Credits.
    /// </summary>
    [Required]
    public List<CreditModel> Credits { get; }

    /// <summary>
    ///     Gets Debits.
    /// </summary>
    [Required]
    public List<DebitModel> Debits { get; }
}
