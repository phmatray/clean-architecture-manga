// <copyright file="CloseAccountUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases.CloseAccount;

using System;
using System.Threading.Tasks;
using Domain;
using Domain.ValueObjects;
using Services;

/// <inheritdoc />
public sealed class CloseAccountUseCase(
    IAccountRepository accountRepository,
    IUserService userService,
    IUnitOfWork unitOfWork)
    : ICloseAccountUseCase
{
    private IOutputPort _outputPort = new CloseAccountPresenter();

    /// <inheritdoc />
    public void SetOutputPort(IOutputPort outputPort)
        => _outputPort = outputPort;

    /// <inheritdoc />
    public Task Execute(Guid accountId)
    {
        string externalUserId = userService.GetCurrentUserId();
        return CloseAccountInternal(new AccountId(accountId), externalUserId);
    }

    private async Task CloseAccountInternal(AccountId accountId, string externalUserId)
    {
        IAccount account = await accountRepository
            .Find(accountId, externalUserId)
            .ConfigureAwait(false);

        if (account is Account closingAccount)
        {
            if (!closingAccount.IsClosingAllowed())
            {
                _outputPort.HasFunds();
                return;
            }

            await Close(closingAccount)
                .ConfigureAwait(false);

            _outputPort.Ok(closingAccount);
            return;
        }

        _outputPort.NotFound();
    }

    private async Task Close(IAccount closeAccount)
    {
        await accountRepository
            .Delete(closeAccount.AccountId)
            .ConfigureAwait(false);

        await unitOfWork
            .Save()
            .ConfigureAwait(false);
    }
}
