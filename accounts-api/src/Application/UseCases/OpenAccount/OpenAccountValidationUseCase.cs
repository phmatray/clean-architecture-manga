// <copyright file="OpenAccountValidationUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases.OpenAccount;

using System.Threading.Tasks;
using Domain.ValueObjects;
using Services;

/// <inheritdoc />
public sealed class OpenAccountValidationUseCase : IOpenAccountUseCase
{
    private readonly Notification _notification;
    private readonly IOpenAccountUseCase _useCase;
    private IOutputPort _outputPort;

    public OpenAccountValidationUseCase(IOpenAccountUseCase useCase, Notification notification)
    {
        _useCase = useCase;
        _notification = notification;
        _outputPort = new OpenAccountPresenter();
    }

    /// <inheritdoc />
    public void SetOutputPort(IOutputPort outputPort)
    {
        _outputPort = outputPort;
        _useCase.SetOutputPort(outputPort);
    }

    /// <inheritdoc />
    public async Task Execute(decimal amount, string currency)
    {
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
            .Execute(amount, currency)
            .ConfigureAwait(false);
    }
}
