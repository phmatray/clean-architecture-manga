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
public sealed class WithdrawUseCase : IWithdrawUseCase
{
    private readonly IAccountFactory _accountFactory;
    private readonly IAccountRepository _accountRepository;
    private readonly ICurrencyExchange _currencyExchange;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;
    private IOutputPort _outputPort;

    /// <summary>
    ///     Initializes a new instance of the <see cref="WithdrawUseCase" /> class.
    /// </summary>
    /// <param name="accountRepository">Account Repository.</param>
    /// <param name="unitOfWork">Unit Of Work.</param>
    /// <param name="accountFactory"></param>
    /// <param name="userService"></param>
    /// <param name="currencyExchange"></param>
    public WithdrawUseCase(
        IAccountRepository accountRepository,
        IUnitOfWork unitOfWork,
        IAccountFactory accountFactory,
        IUserService userService,
        ICurrencyExchange currencyExchange)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
        _accountFactory = accountFactory;
        _userService = userService;
        _currencyExchange = currencyExchange;
        _outputPort = new WithdrawPresenter();
    }

    /// <inheritdoc />
    public void SetOutputPort(IOutputPort outputPort) => _outputPort = outputPort;

    /// <inheritdoc />
    public Task Execute(Guid accountId, decimal amount, string currency) =>
        Withdraw(
            new AccountId(accountId),
            new Money(amount, new Currency(currency)));

    private async Task Withdraw(AccountId accountId, Money withdrawAmount)
    {
        string externalUserId = _userService
            .GetCurrentUserId();

        IAccount account = await _accountRepository
            .Find(accountId, externalUserId)
            .ConfigureAwait(false);

        if (account is Account withdrawAccount)
        {
            Money localCurrencyAmount =
                await _currencyExchange
                    .Convert(withdrawAmount, withdrawAccount.Currency)
                    .ConfigureAwait(false);

            Debit debit = _accountFactory
                .NewDebit(withdrawAccount, localCurrencyAmount, DateTime.Now);

            if (withdrawAccount.GetCurrentBalance().Subtract(debit.Amount).Amount < 0)
            {
                _outputPort?.OutOfFunds();
                return;
            }

            await Withdraw(withdrawAccount, debit)
                .ConfigureAwait(false);

            _outputPort.Ok(debit, withdrawAccount);
            return;
        }

        _outputPort.NotFound();
    }

    private async Task Withdraw(Account account, Debit debit)
    {
        account.Withdraw(debit);

        await _accountRepository
            .Update(account, debit)
            .ConfigureAwait(false);

        await _unitOfWork
            .Save()
            .ConfigureAwait(false);
    }
}
