using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using MyFinances.Core.Importer;
using MyFinances.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyFinances.Core.Tests.Importer
{
    // TODO: Missing failing tests
    public class TransactionImporterTests
    {
        private DateTime now = new DateTime(2020, 4, 20, 6, 47, 00);
        private Account userAccountOne = new Account("294", "User account 1", "Business");
        private Account userAccountTwo = new Account("296", "User account 2", "Business");
        private DataImportOptions importOptions = new DataImportOptions(maxHistoricalTransactionToRetrieveInYears: 3);

        private Mock<ITransactionStore> transactionsStore;
        private Mock<IBankDataProvider> bankDataProvider;
        private Mock<IClock> clock;
        private Mock<IOptionsMonitor<DataImportOptions>> dataImportOptions;

        [Fact]
        public async Task When_requesting_transaction_import_for_user_with_single_account_all_account_transactions_are_imported_for_default_time_period()
        {
            // Arrange
            SetupDependencies();
            TransactionsImporter target = BuildTarget();
            var user = new User("asdas", "john", "john@doe.com");
            // Act
            var result = await target.ImportTransactionsAsync(user);

            // Assert
            result.AccountsImported.Count.Should().Be(1);
            result.AccountsImported.First().TransactionsImported.Should().Be(15);
            bankDataProvider.Verify(s => s.GetAccountTransactionsAsync(userAccountOne, now.Date.AddYears(-1).AddDays(1), now.Date), Times.Once);
            bankDataProvider.Verify(s => s.GetAccountTransactionsAsync(userAccountOne, now.Date.AddYears(-2).AddDays(1), now.Date.AddYears(-1)), Times.Once);
            bankDataProvider.Verify(s => s.GetAccountTransactionsAsync(userAccountOne, now.Date.AddYears(-3).AddDays(1), now.Date.AddYears(-2)), Times.Once);
            transactionsStore.Verify(s => s.AddAccount(user, userAccountOne), Times.Once);
            transactionsStore.Verify(s => s.AddTransactions(userAccountOne,It.IsAny<IList<Transaction>>()), Times.Exactly(3));
        }

        [Fact]
        public async Task When_requesting_transaction_import_for_user_with_multiple_accounts_all_account_transactions_are_imported_for_default_time_period()
        {
            // Arrange
            SetupDependencies();
            TransactionsImporter target = BuildTarget();
            SetupUserAccountTwo();
            var user = new User("asdas", "john", "john@doe.com");

            // Act
            var result = await target.ImportTransactionsAsync(user);

            // Assert
            result.AccountsImported.Count.Should().Be(2);
            result.AccountsImported.First().TransactionsImported.Should().Be(15);
            result.AccountsImported.Skip(1).First().TransactionsImported.Should().Be(13);
            bankDataProvider.Verify(s => s.GetAccountTransactionsAsync(userAccountOne, now.Date.AddYears(-1).AddDays(1), now.Date), Times.Once);
            bankDataProvider.Verify(s => s.GetAccountTransactionsAsync(userAccountOne, now.Date.AddYears(-2).AddDays(1), now.Date.AddYears(-1)), Times.Once);
            bankDataProvider.Verify(s => s.GetAccountTransactionsAsync(userAccountOne, now.Date.AddYears(-3).AddDays(1), now.Date.AddYears(-2)), Times.Once);
            bankDataProvider.Verify(s => s.GetAccountTransactionsAsync(userAccountTwo, now.Date.AddYears(-1).AddDays(1), now.Date), Times.Once);
            bankDataProvider.Verify(s => s.GetAccountTransactionsAsync(userAccountTwo, now.Date.AddYears(-2).AddDays(1), now.Date.AddYears(-1)), Times.Once);
            bankDataProvider.Verify(s => s.GetAccountTransactionsAsync(userAccountTwo, now.Date.AddYears(-3).AddDays(1), now.Date.AddYears(-2)), Times.Once);
            transactionsStore.Verify(s => s.AddAccount(user, userAccountOne), Times.Once);
            transactionsStore.Verify(s => s.AddTransactions(userAccountOne, It.IsAny<IList<Transaction>>()), Times.Exactly(3));
            transactionsStore.Verify(s => s.AddAccount(user, userAccountTwo), Times.Once);
            transactionsStore.Verify(s => s.AddTransactions(userAccountTwo, It.IsAny<IList<Transaction>>()), Times.Exactly(3));
        }

        [Theory]
        [InlineData(1,5)]
        [InlineData(2,10)]
        [InlineData(3,15)]
        [InlineData(4, 20)]
        public async Task When_requesting_transaction_correct_number_of_transactions_are_imported_for_default_period(int defaultPeriod, int transactionsImported)
        {
            // Arrange
            SetupDependencies();
            dataImportOptions.Setup(o => o.CurrentValue).Returns(new DataImportOptions(defaultPeriod));
            TransactionsImporter target = BuildTarget();
            var user = new User("asdas", "john", "john@doe.com");
            // Act
            var result = await target.ImportTransactionsAsync(user);

            // Assert
            result.AccountsImported.Count.Should().Be(1);
            result.AccountsImported.First().TransactionsImported.Should().Be(transactionsImported);
        }

        private void SetupUserAccountTwo()
        {
            bankDataProvider.Setup(p => p.GetUserAccountsAsync(It.IsAny<User>())).ReturnsAsync(new List<Account>() { userAccountOne,userAccountTwo });

            bankDataProvider.Setup(p => p.GetAccountTransactionsAsync(userAccountTwo, now.Date.AddYears(-1).AddDays(1), now.Date))
                .ReturnsAsync(BuildAccountTransactions(now.Date.AddYears(-1).AddDays(1), now.Date,10));
            bankDataProvider.Setup(p => p.GetAccountTransactionsAsync(userAccountTwo, now.Date.AddYears(-2).AddDays(1), now.Date.AddYears(-1)))
                .ReturnsAsync(BuildAccountTransactions(now.Date.AddYears(-2).AddDays(1), now.Date.AddYears(-1),3));
            bankDataProvider.Setup(p => p.GetAccountTransactionsAsync(userAccountTwo, now.Date.AddYears(-3).AddDays(1), now.Date.AddYears(-2)))
                .ReturnsAsync(BuildAccountTransactions(now.Date.AddYears(-3).AddDays(1), now.Date.AddYears(-2),0));
            bankDataProvider.Setup(p => p.GetAccountTransactionsAsync(userAccountTwo, now.Date.AddYears(-4).AddDays(1), now.Date.AddYears(-3)))
                .ReturnsAsync(BuildAccountTransactions(now.Date.AddYears(-4).AddDays(1), now.Date.AddYears(-3),8));
        }

        private TransactionsImporter BuildTarget()
        {
            return new TransactionsImporter(transactionsStore.Object, bankDataProvider.Object, dataImportOptions.Object, clock.Object);
        }

        private void SetupDependencies()
        {
            transactionsStore = new Mock<ITransactionStore>();
            bankDataProvider = new Mock<IBankDataProvider>();
            clock = new Mock<IClock>();
            dataImportOptions = new Mock<IOptionsMonitor<DataImportOptions>>();

            clock.Setup(c => c.Now).Returns(now);
            dataImportOptions.Setup(o => o.CurrentValue).Returns(importOptions);

            bankDataProvider.Setup(p => p.GetUserAccountsAsync(It.IsAny<User>())).ReturnsAsync(new List<Account>() { userAccountOne });

            bankDataProvider.Setup(p => p.GetAccountTransactionsAsync(userAccountOne, now.Date.AddYears(-1).AddDays(1), now.Date))
                .ReturnsAsync(BuildAccountTransactions(now.Date.AddYears(-1).AddDays(1), now.Date));
            bankDataProvider.Setup(p => p.GetAccountTransactionsAsync(userAccountOne, now.Date.AddYears(-2).AddDays(1), now.Date.AddYears(-1)))
                .ReturnsAsync(BuildAccountTransactions(now.Date.AddYears(-2).AddDays(1), now.Date.AddYears(-1)));
            bankDataProvider.Setup(p => p.GetAccountTransactionsAsync(userAccountOne, now.Date.AddYears(-3).AddDays(1), now.Date.AddYears(-2)))
                .ReturnsAsync(BuildAccountTransactions(now.Date.AddYears(-3).AddDays(1), now.Date.AddYears(-2)));
            bankDataProvider.Setup(p => p.GetAccountTransactionsAsync(userAccountOne, now.Date.AddYears(-4).AddDays(1), now.Date.AddYears(-3)))
                .ReturnsAsync(BuildAccountTransactions(now.Date.AddYears(-4).AddDays(1), now.Date.AddYears(-3)));
        }

        private IList<Transaction> BuildAccountTransactions(DateTime from, DateTime to, int howMany=5)
        {
            var fakeTransaction = new Faker<Transaction>()
                .CustomInstantiator(f => new Transaction(f.Finance.Account(), f.Date.Between(from, to), f.Random.String(5), f.Finance.Amount(-1000, 1000), "GBP"));
            return fakeTransaction.GenerateForever().Take(howMany).ToList();
        }
    }
}
