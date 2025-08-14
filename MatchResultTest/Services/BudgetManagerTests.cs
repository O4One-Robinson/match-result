using System;
using NUnit.Framework;
using NSubstitute;
using MatchResult.Models;
using MatchResult.Repositories;
using MatchResult.Services;

namespace MatchResultTest.Services
{
    [TestFixture]
    public class BudgetManagerTests
    {
        private IBudgetRepository _mockBudgetRepository;
        private BudgetManager _budgetManager;

        [SetUp]
        public void SetUp()
        {
            _mockBudgetRepository = Substitute.For<IBudgetRepository>();
            _budgetManager = new BudgetManager(_mockBudgetRepository);
        }

        [Test]
        public void QueryTotalAmount_ShouldReturnCorrectAmount_ForGivenDateRange()
        {
            // Arrange
            var budgets = new[]
            {
                new Budget { YearMonth = "202507", Amount = 3100 },
                new Budget { YearMonth = "202508", Amount = 310 }
            };
            _mockBudgetRepository.GetAll().Returns(budgets);

            DateTime start = new DateTime(2025, 7, 30);
            DateTime end = new DateTime(2025, 8, 14);

            // Act
            double totalAmount = _budgetManager.QueryTotalAmount(start, end);

            // Assert
            Assert.AreEqual(340, totalAmount);
        }
    }
} 