namespace WebApi.UseCases.V1.Transactions.Withdraw;

using System.ComponentModel.DataAnnotations;
using ViewModels;

/// <summary>
///     Withdraw Response.
/// </summary>
public sealed class WithdrawResponse
{
    /// <summary>
    ///     Withdraw Response constructor.
    /// </summary>
    public WithdrawResponse(DebitModel debitModel) => Transaction = debitModel;

    /// <summary>
    ///     Gets Transaction.
    /// </summary>
    [Required]
    public DebitModel Transaction { get; }
}
