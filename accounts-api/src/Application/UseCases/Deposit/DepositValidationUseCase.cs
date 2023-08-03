// <copyright file="DepositValidationUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases.Deposit;

using System;
using System.Threading.Tasks;
using Domain.ValueObjects;
using Services;

/// <inheritdoc />
public sealed class DepositValidationUseCase : IDepositUseCase
{
    private readonly Notification _notification;
    private readonly IDepositUseCase _useCase;
    private IOutputPort _outputPort;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DepositValidationUseCase" /> class.
    /// </summary>
    /// <param name="useCase"></param>
    /// <param name="notification"></param>
    public DepositValidationUseCase(IDepositUseCase useCase, Notification notification)
    {
        _useCase = useCase;
        _notification = notification;
        _outputPort = new DepositPresenter();
    }

    /// <inheritdoc />
    public void SetOutputPort(IOutputPort outputPort)
    {
        _outputPort = outputPort;
        _useCase.SetOutputPort(outputPort);
    }

    /// <inheritdoc />
    public async Task Execute(Guid accountId, decimal amount, string currency)
    {
        if (accountId == Guid.Empty)
        {
            _notification
                .Add(nameof(accountId), "AccountId is required.");
        }

        if (currency != Currency.Dollar.Code &&
            currency != Currency.Euro.Code &&
            currency != Currency.BritishPound.Code &&
            currency != Currency.Canadian.Code &&
            currency != Currency.Real.Code &&
            currency != Currency.Krona.Code)
        {
            _notification
                .Add(nameof(currency), "Currency is required.");
        }

        if (amount <= 0)
        {
            _notification
                .Add(nameof(amount), "Amount should be positive.");
        }

        if (_notification
            .IsInvalid)
        {
            _outputPort
                .Invalid();
            return;
        }

        await _useCase
            .Execute(accountId, amount, currency)
            .ConfigureAwait(false);
    }
}
