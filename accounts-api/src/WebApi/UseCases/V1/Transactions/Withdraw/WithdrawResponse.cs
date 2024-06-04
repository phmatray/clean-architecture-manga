namespace WebApi.UseCases.V1.Transactions.Withdraw;

using System.ComponentModel.DataAnnotations;
using ViewModels;

/// <summary>
///     Withdraw Response.
/// </summary>
public sealed class WithdrawResponse(DebitModel debitModel)
{
    /// <summary>
    ///     Gets Transaction.
    /// </summary>
    [Required]
    public DebitModel Transaction { get; } = debitModel;
}
