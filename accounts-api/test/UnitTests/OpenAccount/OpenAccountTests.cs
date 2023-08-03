namespace UnitTests.OpenAccount;

using System.Threading.Tasks;
using Application.UseCases.OpenAccount;
using Xunit;

public sealed class OpenAccountTests : IClassFixture<StandardFixture>
{
    private readonly StandardFixture _fixture;
    public OpenAccountTests(StandardFixture fixture) => _fixture = fixture;

    [Theory]
    [ClassData(typeof(ValidDataSetup))]
    public async Task OpenAccount_Returns_Ok(decimal amount, string currency)
    {
        OpenAccountPresenter presenter = new OpenAccountPresenter();

        OpenAccountUseCase sut = new OpenAccountUseCase(
            _fixture.AccountRepositoryFake,
            _fixture.UnitOfWork,
            _fixture.TestUserService,
            _fixture.EntityFactory);

        sut.SetOutputPort(presenter);

        await sut.Execute(amount, currency);

        Assert.NotNull(presenter.Account);
    }
}
