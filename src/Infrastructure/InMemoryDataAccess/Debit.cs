namespace Infrastructure.InMemoryDataAccess
{
    using System;
    using Domain.Accounts;
    using Domain.ValueObjects;

    public class Debit : Domain.Accounts.Debit
    {
        public Debit(
            IAccount account,
            PositiveMoney amountToWithdraw,
            DateTime transactionDate)
        {
            AccountId = account.Id;
            Amount = amountToWithdraw;
            TransactionDate = transactionDate;
        }

        protected Debit()
        {
        }

        public AccountId AccountId { get; protected set; }
    }
}