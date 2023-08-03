namespace IntegrationTests.EntityFrameworkTests;

using System;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Credits;
using Domain.ValueObjects;
using Infrastructure.DataAccess;
using Infrastructure.DataAccess.Repositories;
using Xunit;

public sealed class AccountRepositoryTests : IClassFixture<StandardFixture>
{
    private readonly StandardFixture _fixture;
    public AccountRepositoryTests(StandardFixture fixture) => _fixture = fixture;

    [Fact]
    public async Task Add()
    {
        AccountRepository accountRepository = new AccountRepository(_fixture.Context);

        Account account = new Account(
            new AccountId(Guid.NewGuid()),
            SeedData.DefaultExternalUserId,
            Currency.Dollar
        );

        Credit credit = new Credit(
            new CreditId(Guid.NewGuid()),
            account.AccountId,
            DateTime.Now,
            400,
            "USD"
        );

        await accountRepository
            .Add(account, credit)
            .ConfigureAwait(false);

        await _fixture
            .Context
            .SaveChangesAsync()
            .ConfigureAwait(false);

        bool hasAnyAccount = _fixture
            .Context
            .Accounts
            .Any(e => e.AccountId == account.AccountId);

        bool hasAnyCredit = _fixture
            .Context
            .Credits
            .Any(e => e.CreditId == credit.CreditId);

        Assert.True(hasAnyAccount && hasAnyCredit);
    }

    [Fact]
    public async Task Delete()
    {
        AccountRepository accountRepository = new AccountRepository(_fixture.Context);

        Account account = new Account(
            new AccountId(Guid.NewGuid()),
            SeedData.DefaultExternalUserId,
            Currency.Dollar
        );

        Credit credit = new Credit(
            new CreditId(Guid.NewGuid()),
            account.AccountId,
            DateTime.Now,
            400,
            "USD"
        );

        await accountRepository
            .Add(account, credit)
            .ConfigureAwait(false);

        await _fixture
            .Context
            .SaveChangesAsync()
            .ConfigureAwait(false);

        await accountRepository
            .Delete(account.AccountId)
            .ConfigureAwait(false);

        await _fixture
            .Context
            .SaveChangesAsync()
            .ConfigureAwait(false);

        bool hasAnyAccount = _fixture
            .Context
            .Accounts
            .Any(e => e.AccountId == account.AccountId);

        bool hasAnyCredit = _fixture
            .Context
            .Credits
            .Any(e => e.CreditId == credit.CreditId);

        Assert.False(hasAnyAccount && hasAnyCredit);
    }
}
