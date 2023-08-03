namespace UnitTests.Deposit;

using System.Threading.Tasks;
using Application.Services;
using Application.UseCases.Deposit;
using Domain.Credits;
using Domain.ValueObjects;
using Infrastructure.DataAccess;
using Xunit;

public sealed class DepositTests : IClassFixture<StandardFixture>
{
    private readonly StandardFixture _fixture;

    public DepositTests(StandardFixture fixture) => _fixture = fixture;

    [Theory]
    [ClassData(typeof(ValidDataSetup))]
    public async Task DepositUseCase_Returns_Transaction(decimal amount)
    {
        DepositPresenter presenter = new DepositPresenter();
        DepositUseCase sut = new DepositUseCase(
            _fixture.AccountRepositoryFake,
            _fixture.UnitOfWork,
            _fixture.EntityFactory,
            _fixture.CurrencyExchangeFake);

        sut.SetOutputPort(presenter);

        await sut.Execute(
            SeedData.DefaultAccountId.Id,
            amount,
            Currency.Dollar.Code);

        Credit? output = presenter.Credit!;
        Assert.Equal(amount, output.Amount.Amount);
    }

    [Theory]
    [ClassData(typeof(InvalidDataSetup))]
    public async Task DepositUseCase_Returns_Invalid_When_Negative_Amount(decimal amount)
    {
        Notification notification = new Notification();
        DepositPresenter presenter = new DepositPresenter();

        DepositUseCase depositUseCase = new DepositUseCase(
            _fixture.AccountRepositoryFake,
            _fixture.UnitOfWork,
            _fixture.EntityFactory,
            _fixture.CurrencyExchangeFake);

        DepositValidationUseCase sut = new DepositValidationUseCase(
            depositUseCase, notification);

        sut.SetOutputPort(presenter);

        await sut.Execute(
            SeedData.DefaultAccountId.Id,
            amount,
            Currency.Dollar.Code);

        Assert.True(presenter.InvalidOutput);
    }
}
