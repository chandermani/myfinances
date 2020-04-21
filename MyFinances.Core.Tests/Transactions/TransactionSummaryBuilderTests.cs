using Bogus;
using FluentAssertions;
using Moq;
using MyFinances.Core.Dependencies;
using MyFinances.Core.Model;
using MyFinances.Core.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyFinances.Core.Tests.Transactions
{
    public class TransactionSummaryBuilderTests
    {
        private Account accountOne = new Account("oswdud", "Account 1", "sample");
        private Account accountTwo = new Account("iwruro", "Account 2", "sample2");
        Dictionary<string, List<Transaction>> accountTransactions = new Dictionary<string, List<Transaction>>();
        private string[] categories = new string[] { "category1", "category2", "category3" };
        private User user = new User("john@doe.com", "john", "john@doe.com");

        private Mock<ITransactionStore> transactionStore;
        private Mock<IUsersStore> usersStore;
        private Mock<ITransactionsImporter> transactionsImporter;

        [Fact]
        public async Task When_requesting_for_transaction_summary_generates_summary_for_accounts_without_querying_truelayer()
        {
            // Arrange
            SetupDependencies();
            TransactionSummaryBuilder target = BuildTarget();

            // Act
            var result = await target.BuildAsync(user.Identifier, DateTime.Now.Date.AddDays(-6), DateTime.Now.Date);

            // Assert
            result.Should().NotBeNull();
            result.Categories.Count.Should().BeInRange(1, categories.Length);

            result.Categories.Sum(c => c.TotalSpend).Should().Be(-(accountTransactions[accountOne.Identifier].Where(t => t.Amount < 0).Sum(t => t.Amount)+
                                                                    accountTransactions[accountTwo.Identifier].Where(t => t.Amount < 0).Sum(t => t.Amount)));

            result.Categories.Sum(c => c.Accounts.Count).Should().Be(result.Categories.Count * 2);
        }

        [Fact]
        public async Task When_requesting_for_transaction_summary_generates_summary_for_accounts_queries_truelayer_if_data_not_present()
        {
            // Arrange
            SetupDependencies();

            transactionStore.SetupSequence(s => s.GetTransactions(user, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new Dictionary<string, IList<Transaction>>())
                .Returns(new Dictionary<string, IList<Transaction>>()
                {
                    { accountOne.Identifier, accountTransactions[accountOne.Identifier]},
                    { accountTwo.Identifier, accountTransactions[accountTwo.Identifier]}
                });


            TransactionSummaryBuilder target = BuildTarget();

            // Act
            var result = await target.BuildAsync(user.Identifier, DateTime.Now.Date.AddDays(-6), DateTime.Now.Date);

            // Assert
            result.Should().NotBeNull();
            result.Categories.Count.Should().BeInRange(1, categories.Length);

            result.Categories.Sum(c => c.TotalSpend).Should().Be(-(accountTransactions[accountOne.Identifier].Where(t => t.Amount < 0).Sum(t => t.Amount) +
                                                                    accountTransactions[accountTwo.Identifier].Where(t => t.Amount < 0).Sum(t => t.Amount)));

            result.Categories.Sum(c => c.Accounts.Count).Should().Be(result.Categories.Count * 2);
        }

        private TransactionSummaryBuilder BuildTarget()
        {
            return new TransactionSummaryBuilder(transactionStore.Object, usersStore.Object, transactionsImporter.Object);
        }

        private void SetupDependencies()
        {
            transactionStore = new Mock<ITransactionStore>();
            usersStore = new Mock<IUsersStore>();
            transactionsImporter = new Mock<ITransactionsImporter>();

            usersStore.Setup(s => s.GetUser(It.IsAny<string>())).Returns(user);

            transactionsImporter.Setup(i => i.ImportTransactionsAsync(It.IsAny<string>()))
                .ReturnsAsync(new TransactionsImportResult(new List<AccountTransactionImportResult>()));

            accountTransactions[accountOne.Identifier] = GenerateTransactions().ToList();
            accountTransactions[accountTwo.Identifier] = GenerateTransactions().ToList();

            transactionStore.Setup(s => s.GetTransactions(user, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new Dictionary<string, IList<Transaction>>()
                {
                    { accountOne.Identifier, accountTransactions[accountOne.Identifier]},
                    { accountTwo.Identifier, accountTransactions[accountTwo.Identifier]}
                });
        }

        private IEnumerable<Transaction> GenerateTransactions(int howMany = 10)
        {
            var fakeTransaction = new Faker<Transaction>()
                .CustomInstantiator(f => new Transaction(f.Finance.Account(), f.Date.Past(), f.PickRandom<string>(categories), f.Finance.Amount(-1000, 1000), "GBP"));
            return fakeTransaction.GenerateForever().Take(howMany).ToList();
        }
    }
}
