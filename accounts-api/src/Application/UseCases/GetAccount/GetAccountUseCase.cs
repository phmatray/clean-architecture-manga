// <copyright file="GetAccountUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases.GetAccount;

using System;
using System.Threading.Tasks;
using Domain;
using Domain.ValueObjects;

/// <inheritdoc />
public sealed class GetAccountUseCase : IGetAccountUseCase
{
    private readonly IAccountRepository _accountRepository;
    private IOutputPort _outputPort;

    /// <summary>
    ///     Initializes a new instance of the <see cref="GetAccountUseCase" /> class.
    /// </summary>
    /// <param name="accountRepository">Account Repository.</param>
    public GetAccountUseCase(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
        _outputPort = new GetAccountPresenter();
    }

    /// <inheritdoc />
    public void SetOutputPort(IOutputPort outputPort) => _outputPort = outputPort;

    /// <inheritdoc />
    public Task Execute(Guid accountId) =>
        GetAccountInternal(new AccountId(accountId));

    private async Task GetAccountInternal(AccountId accountId)
    {
        IAccount account = await _accountRepository
            .GetAccount(accountId)
            .ConfigureAwait(false);

        if (account is Account getAccount)
        {
            _outputPort.Ok(getAccount);
            return;
        }

        _outputPort.NotFound();
    }
}
