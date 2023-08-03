namespace UnitTests.CloseAccount;

using System;
using System.Threading.Tasks;
using Application.UseCases.CloseAccount;
using Application.UseCases.GetAccount;
using Application.UseCases.Withdraw;
using Domain;
using Domain.Credits;
using Domain.ValueObjects;
using Infrastructure.DataAccess;
using Xunit;

public sealed class CloseAccountTests : IClassFixture<StandardFixture>
{
    private readonly StandardFixture _fixture;
    public CloseAccountTests(StandardFixture fixture) => _fixture = fixture;

    [Theory]
    [ClassData(typeof(ValidDataSetup))]
    public void IsClosingAllowed_Returns_False_When_Account_Has_Funds(decimal amount)
    {
        Account account = _fixture
            .EntityFactory
            .NewAccount(Guid.NewGuid().ToString(), Currency.Dollar);

        Credit credit = _fixture
            .EntityFactory
            .NewCredit(account, new Money(amount, Currency.Dollar), DateTime.Now);

        account.Deposit(credit);

        bool actual = account.IsClosingAllowed();

        Assert.False(actual);
    }

    [Fact]
    public async Task CloseAccountUseCase_Returns_Closed_Account_Id_When_Account_Has_Zero_Balance()
    {
        var getAccountPresenter = new GetAccountPresenter();
        var closeAccountPresenter = new CloseAccountPresenter();

        var getAccountUseCase = new GetAccountUseCase(_fixture.AccountRepositoryFake);

        var withdrawUseCase = new WithdrawUseCase(
            _fixture.AccountRepositoryFake,
            _fixture.UnitOfWork,
            _fixture.EntityFactory,
            _fixture.TestUserService,
            _fixture.CurrencyExchangeFake);

        var sut = new CloseAccountUseCase(
            _fixture.AccountRepositoryFake,
            _fixture.TestUserService,
            _fixture.UnitOfWork);

        sut.SetOutputPort(closeAccountPresenter);
        getAccountUseCase.SetOutputPort(getAccountPresenter);

        await getAccountUseCase.Execute(SeedData.DefaultAccountId.Id);
        IAccount getAccountDetailOutput = getAccountPresenter.Account!;

        await withdrawUseCase.Execute(
            SeedData.DefaultAccountId.Id,
            getAccountDetailOutput.GetCurrentBalance().Amount,
            getAccountDetailOutput.GetCurrentBalance().Currency.Code);

        await sut.Execute(SeedData.DefaultAccountId.Id);

        Assert.Equal(SeedData.DefaultAccountId.Id, closeAccountPresenter.Account!.AccountId.Id);
    }

    [Fact]
    public void IsClosingAllowed_Returns_True_When_Account_Does_Not_Has_Funds()
    {
        IAccount account = _fixture.EntityFactory
            .NewAccount(Guid.NewGuid().ToString(), Currency.Dollar);

        bool actual = account.IsClosingAllowed();

        Assert.True(actual);
    }
}
