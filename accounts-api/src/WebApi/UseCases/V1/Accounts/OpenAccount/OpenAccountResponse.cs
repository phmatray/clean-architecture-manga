namespace WebApi.UseCases.V1.Accounts.OpenAccount;

using System.ComponentModel.DataAnnotations;
using ViewModels;

/// <summary>
///     The response for Registration.
/// </summary>
public sealed class OpenAccountResponse(AccountModel accountModel)
{
    /// <summary>
    ///     Gets customer.
    /// </summary>
    [Required]
    public AccountModel Account { get; } = accountModel;
}
