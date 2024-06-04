namespace WebApi.UseCases.V1.Accounts.CloseAccount;

using System;
using System.ComponentModel.DataAnnotations;
using Domain;

/// <summary>
///     The response Close an Account.
/// </summary>
public sealed class CloseAccountResponse(Account account)
{
    /// <summary>
    ///     Gets account ID.
    /// </summary>
    [Required]
    public Guid AccountId { get; } = account.AccountId.Id;
}
