namespace WebApi.UseCases.V1.Accounts.GetAccount;

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Application.Services;
using Application.UseCases.GetAccount;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Modules.Common;
using Modules.Common.FeatureFlags;

/// <summary>
///     Accounts
///     <see href="https://github.com/ivanpaulovich/clean-architecture-manga/wiki/Design-Patterns#controller">
///         Controller Design Pattern
///     </see>
///     .
/// </summary>
[ApiVersion("1.0")]
[FeatureGate(CustomFeature.GetAccount)]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public sealed class AccountsController(Notification notification)
    : ControllerBase, IOutputPort
{
    private IActionResult _viewModel;

    void IOutputPort.Invalid()
    {
        var problemDetails = new ValidationProblemDetails(notification.ModelState);
        _viewModel = BadRequest(problemDetails);
    }

    void IOutputPort.NotFound() => _viewModel = NotFound();

    void IOutputPort.Ok(Account account) => _viewModel = Ok(new GetAccountResponse(account));

    /// <summary>
    ///     Get an account details.
    /// </summary>
    /// <response code="200">The Account.</response>
    /// <response code="404">Not Found.</response>
    /// <param name="useCase">Use case.</param>
    /// <param name="accountId"></param>
    /// <returns>An asynchronous <see cref="IActionResult" />.</returns>
    [Authorize]
    [HttpGet("{accountId:guid}", Name = "GetAccount")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAccountResponse))]
    [ApiConventionMethod(typeof(CustomApiConventions), nameof(CustomApiConventions.Find))]
    public async Task<IActionResult> Get(
        [FromServices] IGetAccountUseCase useCase,
        [FromRoute][Required] Guid accountId)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute(accountId)
            .ConfigureAwait(false);

        return _viewModel!;
    }
}
