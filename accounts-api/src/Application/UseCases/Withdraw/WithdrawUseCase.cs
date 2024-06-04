// <copyright file="WithdrawUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases.Withdraw;

using System;
using System.Threading.Tasks;
using Domain;
using Domain.Debits;
using Domain.ValueObjects;
using Services;

/// <inheritdoc />
public sealed class WithdrawUseCase(
    IAccountRepository accountRepository,
    IUnitOfWork unitOfWork,
    IAccountFactory accountFactory,
    IUserService userService,
    ICurrencyExchange currencyExchange)
    : IWithdrawUseCase
{
    private IOutputPort _outputPort = new WithdrawPresenter();

    /// <inheritdoc />
    public void SetOutputPort(IOutputPort outputPort)
        => _outputPort = outputPort;

    /// <inheritdoc />
    public Task Execute(Guid accountId, decimal amount, string currency)
        => Withdraw(
            new AccountId(accountId),
            new Money(amount, new Currency(currency)));

    private async Task Withdraw(AccountId accountId, Money withdrawAmount)
    {
        string externalUserId = userService
            .GetCurrentUserId();

        IAccount account = await accountRepository
            .Find(accountId, externalUserId)
            .ConfigureAwait(false);

        if (account is Account withdrawAccount)
        {
            Money localCurrencyAmount =
                await currencyExchange
                    .Convert(withdrawAmount, withdrawAccount.Currency)
                    .ConfigureAwait(false);

            Debit debit = accountFactory
                .NewDebit(withdrawAccount, localCurrencyAmount, DateTime.Now);

            if (withdrawAccount.GetCurrentBalance().Subtract(debit.Amount).Amount < 0)
            {
                _outputPort?.OutOfFunds();
                return;
            }

            await Withdraw(withdrawAccount, debit).ConfigureAwait(false);

            _outputPort.Ok(debit, withdrawAccount);
            return;
        }

        _outputPort.NotFound();
    }

    private async Task Withdraw(Account account, Debit debit)
    {
        account.Withdraw(debit);

        await accountRepository
            .Update(account, debit)
            .ConfigureAwait(false);

        await unitOfWork
            .Save()
            .ConfigureAwait(false);
    }
}
