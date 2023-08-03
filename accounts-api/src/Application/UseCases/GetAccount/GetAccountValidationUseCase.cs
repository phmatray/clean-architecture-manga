// <copyright file="GetAccountValidationUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases.GetAccount;

using System;
using System.Threading.Tasks;
using Services;

/// <inheritdoc />
public sealed class GetAccountValidationUseCase : IGetAccountUseCase
{
    private readonly Notification _notification;
    private readonly IGetAccountUseCase _useCase;
    private IOutputPort _outputPort;

    /// <summary>
    ///     Initializes a new instance of the <see cref="GetAccountValidationUseCase" /> class.
    /// </summary>
    public GetAccountValidationUseCase(IGetAccountUseCase useCase, Notification notification)
    {
        _useCase = useCase;
        _notification = notification;
        _outputPort = new GetAccountPresenter();
    }

    /// <inheritdoc />
    public void SetOutputPort(IOutputPort outputPort)
    {
        _outputPort = outputPort;
        _useCase.SetOutputPort(outputPort);
    }

    /// <inheritdoc />
    public async Task Execute(Guid accountId)
    {
        if (accountId == Guid.Empty)
        {
            _notification
                .Add(nameof(accountId), "AccountId is required.");
        }

        if (_notification
            .IsInvalid)
        {
            _outputPort.Invalid();
            return;
        }

        await _useCase
            .Execute(accountId)
            .ConfigureAwait(false);
    }
}
