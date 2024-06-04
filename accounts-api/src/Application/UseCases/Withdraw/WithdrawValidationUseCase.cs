// <copyright file="WithdrawValidationUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases.Withdraw;

using System;
using System.Threading.Tasks;
using Domain.ValueObjects;
using Services;

/// <inheritdoc />
public sealed class WithdrawValidationUseCase(
    IWithdrawUseCase useCase,
    Notification notification)
    : IWithdrawUseCase
{
    private IOutputPort _outputPort = new WithdrawPresenter();

    /// <inheritdoc />
    public void SetOutputPort(IOutputPort outputPort)
    {
        _outputPort = outputPort;
        useCase.SetOutputPort(outputPort);
    }

    /// <inheritdoc />
    public async Task Execute(Guid accountId, decimal amount, string currency)
    {
        if (accountId == Guid.Empty)
        {
            notification.Add(nameof(accountId), "AccountId is required.");
        }

        if (currency != Currency.Dollar.Code &&
            currency != Currency.Euro.Code &&
            currency != Currency.BritishPound.Code &&
            currency != Currency.Canadian.Code &&
            currency != Currency.Real.Code &&
            currency != Currency.Krona.Code)
        {
            notification.Add(nameof(currency), "Currency is required.");
        }

        if (amount <= 0)
        {
            notification.Add(nameof(amount), "Amount should be positive.");
        }

        if (notification.IsInvalid)
        {
            _outputPort?.Invalid();
            return;
        }

        await useCase
            .Execute(accountId, amount, currency)
            .ConfigureAwait(false);
    }
}
