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
public sealed class OpenAccountUseCase : IOpenAccountUseCase
{
    private readonly IAccountFactory _accountFactory;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;
    private IOutputPort _outputPort;

    public OpenAccountUseCase(
        IAccountRepository accountRepository,
        IUnitOfWork unitOfWork,
        IUserService userService,
        IAccountFactory accountFactory)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
        _userService = userService;
        _accountFactory = accountFactory;
        _outputPort = new OpenAccountPresenter();
    }

    /// <inheritdoc />
    public void SetOutputPort(IOutputPort outputPort) => _outputPort = outputPort;

    /// <inheritdoc />
    public Task Execute(decimal amount, string currency) =>
        OpenAccount(new Money(amount, new Currency(currency)));

    private async Task OpenAccount(Money amountToDeposit)
    {
        string externalUserId = _userService
            .GetCurrentUserId();

        Account account = _accountFactory
            .NewAccount(externalUserId, amountToDeposit.Currency);

        Credit credit = _accountFactory
            .NewCredit(account, amountToDeposit, DateTime.Now);

        await Deposit(account, credit)
            .ConfigureAwait(false);

        _outputPort?.Ok(account);
    }

    private async Task Deposit(Account account, Credit credit)
    {
        account.Deposit(credit);

        await _accountRepository
            .Add(account, credit)
            .ConfigureAwait(false);

        await _unitOfWork
            .Save()
            .ConfigureAwait(false);
    }
}
