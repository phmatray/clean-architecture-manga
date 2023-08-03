namespace UnitTests;

using Infrastructure.CurrencyExchange;
using Infrastructure.DataAccess;
using Infrastructure.DataAccess.Repositories;
using Infrastructure.ExternalAuthentication;

/// <summary>
/// </summary>
public sealed class StandardFixture
{
    public StandardFixture()
    {
        Context = new MangaContextFake();
        AccountRepositoryFake = new AccountRepositoryFake(Context);
        UnitOfWork = new UnitOfWorkFake();
        EntityFactory = new EntityFactory();
        TestUserService = new TestUserService();
        CurrencyExchangeFake = new CurrencyExchangeFake();
    }

    public EntityFactory EntityFactory { get; }

    public MangaContextFake Context { get; }

    public AccountRepositoryFake AccountRepositoryFake { get; }

    public UnitOfWorkFake UnitOfWork { get; }

    public TestUserService TestUserService { get; }

    public CurrencyExchangeFake CurrencyExchangeFake { get; }
}
