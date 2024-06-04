// <copyright file="DepositUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases.Deposit;

using System;
using System.Threading.Tasks;
using Domain;
using Domain.Credits;
using Domain.ValueObjects;
using Services;

/// <inheritdoc />
public sealed class DepositUseCase(
    IAccountRepository accountRepository,
    IUnitOfWork unitOfWork,
    IAccountFactory accountFactory,
    ICurrencyExchange currencyExchange)
    : IDepositUseCase
{
    private IOutputPort _outputPort = new DepositPresenter();

    /// <inheritdoc />
    public void SetOutputPort(IOutputPort outputPort)
        => _outputPort = outputPort;

    /// <inheritdoc />
    public Task Execute(Guid accountId, decimal amount, string currency) =>
        Deposit(
            new AccountId(accountId),
            new Money(amount, new Currency(currency)));

    private async Task Deposit(AccountId accountId, Money amount)
    {
        IAccount account = await accountRepository
            .GetAccount(accountId)
            .ConfigureAwait(false);

        if (account is Account depositAccount)
        {
            Money convertedAmount =
                await currencyExchange
                    .Convert(amount, depositAccount.Currency)
                    .ConfigureAwait(false);

            Credit credit = accountFactory
                .NewCredit(depositAccount, convertedAmount, DateTime.Now);

            await Deposit(depositAccount, credit)
                .ConfigureAwait(false);

            _outputPort.Ok(credit, depositAccount);
            return;
        }

        _outputPort.NotFound();
    }

    private async Task Deposit(Account account, Credit credit)
    {
        account.Deposit(credit);

        await accountRepository
            .Update(account, credit)
            .ConfigureAwait(false);

        await unitOfWork
            .Save()
            .ConfigureAwait(false);
    }
}
