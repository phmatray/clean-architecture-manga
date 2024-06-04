// <copyright file="GetAccountsUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases.GetAccounts;

using System.Threading.Tasks;
using Domain;
using Services;

/// <inheritdoc />
public sealed class GetAccountsUseCase(
    IUserService userService,
    IAccountRepository accountRepository)
    : IGetAccountsUseCase
{
    private IOutputPort _outputPort = new GetAccountPresenter();

    /// <inheritdoc />
    public void SetOutputPort(IOutputPort outputPort)
        => _outputPort = outputPort;

    /// <inheritdoc />
    public Task Execute()
    {
        string externalUserId = userService.GetCurrentUserId();
        return GetAccounts(externalUserId);
    }

    private async Task GetAccounts(string externalUserId)
    {
        var accounts = await accountRepository
            .GetAccounts(externalUserId)
            .ConfigureAwait(false);

        _outputPort.Ok(accounts);
    }
}
