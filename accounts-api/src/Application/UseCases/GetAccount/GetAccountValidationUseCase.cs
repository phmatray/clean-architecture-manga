// <copyright file="GetAccountValidationUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases.GetAccount;

using System;
using System.Threading.Tasks;
using Services;

/// <inheritdoc />
public sealed class GetAccountValidationUseCase(
    IGetAccountUseCase useCase,
    Notification notification)
    : IGetAccountUseCase
{
    private IOutputPort _outputPort = new GetAccountPresenter();

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

        if (notification.IsInvalid)
        {
            _outputPort.Invalid();
            return;
        }

        await useCase
            .Execute(accountId)
            .ConfigureAwait(false);
    }
}
