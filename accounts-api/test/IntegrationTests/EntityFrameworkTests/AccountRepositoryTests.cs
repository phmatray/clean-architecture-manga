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

public sealed class AccountRepositoryTests(StandardFixture fixture)
    : IClassFixture<StandardFixture>
{
    [Fact]
    public async Task Add()
    {
        var accountRepository = new AccountRepository(fixture.Context);

        var account = new Account(
            new AccountId(Guid.NewGuid()),
            SeedData.DefaultExternalUserId,
            Currency.Dollar
        );

        var credit = new Credit(
            new CreditId(Guid.NewGuid()),
            account.AccountId,
            DateTime.Now,
            400,
            "USD"
        );

        await accountRepository.Add(account, credit);
        await fixture.Context.SaveChangesAsync();

        bool hasAnyAccount = fixture
            .Context
            .Accounts
            .Any(e => e.AccountId == account.AccountId);

        bool hasAnyCredit = fixture
            .Context
            .Credits
            .Any(e => e.CreditId == credit.CreditId);

        Assert.True(hasAnyAccount && hasAnyCredit);
    }

    [Fact]
    public async Task Delete()
    {
        var accountRepository = new AccountRepository(fixture.Context);

        var account = new Account(
            new AccountId(Guid.NewGuid()),
            SeedData.DefaultExternalUserId,
            Currency.Dollar
        );

        var credit = new Credit(
            new CreditId(Guid.NewGuid()),
            account.AccountId,
            DateTime.Now,
            400,
            "USD"
        );

        await accountRepository.Add(account, credit);
        await fixture.Context.SaveChangesAsync();

        await accountRepository.Delete(account.AccountId);
        await fixture.Context.SaveChangesAsync();

        bool hasAnyAccount = fixture
            .Context
            .Accounts
            .Any(e => e.AccountId == account.AccountId);

        bool hasAnyCredit = fixture
            .Context
            .Credits
            .Any(e => e.CreditId == credit.CreditId);

        Assert.False(hasAnyAccount && hasAnyCredit);
    }
}
