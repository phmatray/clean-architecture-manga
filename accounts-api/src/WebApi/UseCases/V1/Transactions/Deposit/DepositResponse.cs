namespace WebApi.UseCases.V1.Transactions.Deposit;

using System.ComponentModel.DataAnnotations;
using ViewModels;

/// <summary>
///     The response for a successful Deposit.
/// </summary>
public sealed class DepositResponse(CreditModel transaction)
{
    /// <summary>
    ///     Gets Transaction.
    /// </summary>
    [Required]
    public CreditModel Transaction { get; } = transaction;
}
