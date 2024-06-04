// <copyright file="GetAccountUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases.GetAccount;

using System;
using System.Threading.Tasks;
using Domain;
using Domain.ValueObjects;

/// <inheritdoc />
public sealed class GetAccountUseCase(IAccountRepository accountRepository)
    : IGetAccountUseCase
{
    private IOutputPort _outputPort = new GetAccountPresenter();

    /// <inheritdoc />
    public void SetOutputPort(IOutputPort outputPort)
        => _outputPort = outputPort;

    /// <inheritdoc />
    public Task Execute(Guid accountId)
        => GetAccountInternal(new AccountId(accountId));

    private async Task GetAccountInternal(AccountId accountId)
    {
        IAccount account = await accountRepository
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
