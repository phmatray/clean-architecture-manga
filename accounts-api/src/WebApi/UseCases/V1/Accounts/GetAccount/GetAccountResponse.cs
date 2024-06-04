namespace WebApi.UseCases.V1.Accounts.GetAccount;

using System.ComponentModel.DataAnnotations;
using Domain;
using ViewModels;

/// <summary>
///     Get Account Response.
/// </summary>
public sealed class GetAccountResponse(Account account)
{
    /// <summary>
    ///     Gets account ID.
    /// </summary>
    [Required]
    public AccountDetailsModel Account { get; } = new(account);
}
