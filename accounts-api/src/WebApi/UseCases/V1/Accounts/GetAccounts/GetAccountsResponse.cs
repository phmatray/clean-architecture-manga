using System.Linq;

namespace WebApi.UseCases.V1.Accounts.GetAccounts;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain;
using ViewModels;

/// <summary>
///     Get Accounts Response.
/// </summary>
public sealed class GetAccountsResponse(IEnumerable<Account> accounts)
{
    /// <summary>
    ///     Accounts
    /// </summary>
    [Required]
    public List<AccountModel> Accounts { get; } = accounts
        .Select(account => new AccountModel(account))
        .ToList();
}
