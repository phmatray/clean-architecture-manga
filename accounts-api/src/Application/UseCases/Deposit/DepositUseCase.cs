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
public sealed class DepositUseCase : IDepositUseCase
{
    private readonly IAccountFactory _accountFactory;
    private readonly IAccountRepository _accountRepository;
    private readonly ICurrencyExchange _currencyExchange;
    private readonly IUnitOfWork _unitOfWork;
    private IOutputPort _outputPort;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DepositUseCase" /> class.
    /// </summary>
    /// <param name="accountRepository">Account Repository.</param>
    /// <param name="unitOfWork">Unit Of Work.</param>
    /// <param name="accountFactory"></param>
    /// <param name="currencyExchange"></param>
    public DepositUseCase(
        IAccountRepository accountRepository,
        IUnitOfWork unitOfWork,
        IAccountFactory accountFactory,
        ICurrencyExchange currencyExchange)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
        _accountFactory = accountFactory;
        _currencyExchange = currencyExchange;
        _outputPort = new DepositPresenter();
    }

    /// <inheritdoc />
    public void SetOutputPort(IOutputPort outputPort) => _outputPort = outputPort;

    /// <inheritdoc />
    public Task Execute(Guid accountId, decimal amount, string currency) =>
        Deposit(
            new AccountId(accountId),
            new Money(amount, new Currency(currency)));

    private async Task Deposit(AccountId accountId, Money amount)
    {
        IAccount account = await _accountRepository
            .GetAccount(accountId)
            .ConfigureAwait(false);

        if (account is Account depositAccount)
        {
            Money convertedAmount =
                await _currencyExchange
                    .Convert(amount, depositAccount.Currency)
                    .ConfigureAwait(false);

            Credit credit = _accountFactory
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

        await _accountRepository
            .Update(account, credit)
            .ConfigureAwait(false);

        await _unitOfWork
            .Save()
            .ConfigureAwait(false);
    }
}
