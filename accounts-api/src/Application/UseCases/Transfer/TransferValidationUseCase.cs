// <copyright file="TransferValidationUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases.Transfer;

using System;
using System.Threading.Tasks;
using Domain.ValueObjects;
using Services;

/// <inheritdoc />
public sealed class TransferValidationUseCase(
    ITransferUseCase useCase,
    Notification notification)
    : ITransferUseCase
{
    private IOutputPort _outputPort = new TransferPresenter();

    /// <inheritdoc />
    public void SetOutputPort(IOutputPort outputPort)
    {
        _outputPort = outputPort;
        useCase.SetOutputPort(outputPort);
    }

    /// <inheritdoc />
    public async Task Execute(Guid originAccountId, Guid destinationAccountId, decimal amount, string currency)
    {
        if (originAccountId == Guid.Empty)
        {
            notification.Add(nameof(originAccountId), "AccountId is required.");
        }

        if (destinationAccountId == Guid.Empty)
        {
            notification.Add(nameof(destinationAccountId), "AccountId is required.");
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
            _outputPort.Invalid();
            return;
        }

        await useCase
            .Execute(originAccountId, destinationAccountId, amount, currency)
            .ConfigureAwait(false);
    }
}
