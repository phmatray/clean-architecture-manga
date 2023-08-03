// <copyright file="OpenAccountUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases.OpenAccount;

using System;
using System.Threading.Tasks;
using Domain;
using Domain.Credits;
using Domain.ValueObjects;
using Services;

/// <inheritdoc />
public sealed class OpenAccountUseCase(
        IAccountRepository accountRepository,
        IUnitOfWork unitOfWork,
        IUserService userService,
        IAccountFactory accountFactory)
    : IOpenAccountUseCase
{
    private IOutputPort _outputPort = new OpenAccountPresenter();

    /// <inheritdoc />
    public void SetOutputPort(IOutputPort outputPort) => _outputPort = outputPort;

    /// <inheritdoc />
    public Task Execute(decimal amount, string currency) =>
        OpenAccount(new Money(amount, new Currency(currency)));

    private async Task OpenAccount(Money amountToDeposit)
    {
        string externalUserId = userService
            .GetCurrentUserId();

        Account account = accountFactory
            .NewAccount(externalUserId, amountToDeposit.Currency);

        Credit credit = accountFactory
            .NewCredit(account, amountToDeposit, DateTime.Now);

        await Deposit(account, credit)
            .ConfigureAwait(false);

        _outputPort?.Ok(account);
    }

    private async Task Deposit(Account account, Credit credit)
    {
        account.Deposit(credit);

        await accountRepository
            .Add(account, credit)
            .ConfigureAwait(false);

        await unitOfWork
            .Save()
            .ConfigureAwait(false);
    }
}
