// <copyright file="GetAccountsUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases.GetAccounts;

using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Services;

/// <inheritdoc />
public sealed class GetAccountsUseCase : IGetAccountsUseCase
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUserService _userService;
    private IOutputPort _outputPort;

    /// <summary>
    ///     Initializes a new instance of the <see cref="GetAccountsUseCase" /> class.
    /// </summary>
    /// <param name="userService">User Service.</param>
    /// <param name="accountRepository">Customer Repository.</param>
    public GetAccountsUseCase(
        IUserService userService,
        IAccountRepository accountRepository)
    {
        _userService = userService;
        _accountRepository = accountRepository;
        _outputPort = new GetAccountPresenter();
    }

    /// <inheritdoc />
    public void SetOutputPort(IOutputPort outputPort) => _outputPort = outputPort;

    /// <inheritdoc />
    public Task Execute()
    {
        string externalUserId = _userService
            .GetCurrentUserId();

        return GetAccounts(externalUserId);
    }

    private async Task GetAccounts(string externalUserId)
    {
        IList<Account>? accounts = await _accountRepository
            .GetAccounts(externalUserId)
            .ConfigureAwait(false);

        _outputPort.Ok(accounts);
    }
}
