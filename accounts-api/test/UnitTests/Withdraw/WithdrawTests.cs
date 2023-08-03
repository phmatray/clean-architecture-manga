namespace UnitTests.Withdraw;

using System.Threading.Tasks;
using Application.UseCases.Withdraw;
using Domain;
using Infrastructure.DataAccess;
using Xunit;

public sealed class WithdrawTests : IClassFixture<StandardFixture>
{
    private readonly StandardFixture _fixture;

    public WithdrawTests(StandardFixture fixture) => _fixture = fixture;

    [Theory]
    [ClassData(typeof(ValidDataSetup))]
    public async Task Withdraw_Returns_Account(
        decimal amount,
        decimal expectedBalance)
    {
        WithdrawPresenter presenter = new WithdrawPresenter();
        WithdrawUseCase sut = new WithdrawUseCase(
            _fixture.AccountRepositoryFake,
            _fixture.UnitOfWork,
            _fixture.EntityFactory,
            _fixture.TestUserService,
            _fixture.CurrencyExchangeFake);

        sut.SetOutputPort(presenter);

        await sut.Execute(SeedData.DefaultAccountId.Id, amount, "USD");

        Account? actual = presenter.Account!;
        Assert.Equal(expectedBalance, actual.GetCurrentBalance().Amount);
    }
}
