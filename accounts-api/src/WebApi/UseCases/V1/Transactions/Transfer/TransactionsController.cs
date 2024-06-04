namespace WebApi.UseCases.V1.Transactions.Transfer;

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Application.Services;
using Application.UseCases.Transfer;
using Domain;
using Domain.Credits;
using Domain.Debits;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Modules.Common;
using Modules.Common.FeatureFlags;
using ViewModels;

/// <summary>
///     Accounts
///     <see href="https://github.com/ivanpaulovich/clean-architecture-manga/wiki/Design-Patterns#controller">
///         Controller Design Pattern
///     </see>
///     .
/// </summary>
[FeatureGate(CustomFeature.Transfer)]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public sealed class TransactionsController(Notification notification)
    : ControllerBase, IOutputPort
{
    private IActionResult _viewModel;

    void IOutputPort.OutOfFunds()
        => _viewModel = BadRequest("Out of funds.");

    void IOutputPort.Invalid()
    {
        var problemDetails = new ValidationProblemDetails(notification.ModelState);
        _viewModel = BadRequest(problemDetails);
    }

    void IOutputPort.NotFound()
        => _viewModel = NotFound();

    void IOutputPort.Ok(Account originAccount, Debit debit, Account destinationAccount, Credit credit)
        => _viewModel = Ok(new TransferResponse(new DebitModel(debit)));

    /// <summary>
    ///     Transfer to an account.
    /// </summary>
    /// <response code="200">The updated balance.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="404">Not Found.</response>
    /// <param name="useCase"></param>
    /// <param name="accountId"></param>
    /// <param name="destinationAccountId"></param>
    /// <param name="amount"></param>
    /// <param name="currency"></param>
    /// <returns>The updated balance.</returns>
    [Authorize]
    [HttpPatch("{accountId:guid}/{destinationAccountId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TransferResponse))]
    [ApiConventionMethod(typeof(CustomApiConventions), nameof(CustomApiConventions.Patch))]
#pragma warning disable SCS0016 // Controller method is potentially vulnerable to Cross Site Request Forgery (CSRF).
    public async Task<IActionResult> Transfer(
#pragma warning restore SCS0016 // Controller method is potentially vulnerable to Cross Site Request Forgery (CSRF).
        [FromServices] ITransferUseCase useCase,
        [FromRoute][Required] Guid accountId,
        [FromRoute][Required] Guid destinationAccountId,
        [FromForm][Required] decimal amount,
        [FromForm][Required] string currency)
    {
        useCase.SetOutputPort(this);
        await useCase.Execute(accountId, destinationAccountId, amount, currency).ConfigureAwait(false);
        return _viewModel!;
    }
}
