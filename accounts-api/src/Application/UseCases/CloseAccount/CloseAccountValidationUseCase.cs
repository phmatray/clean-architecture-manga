// <copyright file="CloseAccountValidationUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases.CloseAccount;

using System;
using System.Threading.Tasks;
using Services;

/// <inheritdoc />
public sealed class CloseAccountValidationUseCase(
    ICloseAccountUseCase useCase,
    Notification notification)
    : ICloseAccountUseCase
{
    private IOutputPort _outputPort = new CloseAccountPresenter();

    /// <inheritdoc />
    public void SetOutputPort(IOutputPort outputPort)
    {
        _outputPort = outputPort;
        useCase.SetOutputPort(outputPort);
    }

    /// <inheritdoc />
    public async Task Execute(Guid accountId)
    {
        if (accountId == Guid.Empty)
        {
            notification.Add(nameof(accountId), "AccountId is required.");
        }

        if (!notification.IsValid)
        {
            _outputPort.Invalid();
            return;
        }

        await useCase
            .Execute(accountId)
            .ConfigureAwait(false);
    }
}
