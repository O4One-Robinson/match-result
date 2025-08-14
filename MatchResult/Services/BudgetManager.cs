using System;
using System.Linq;
using MatchResult.Models;
using MatchResult.Repositories;

namespace MatchResult.Services
{
    public class BudgetManager
    {
        private readonly IBudgetRepository _budgetRepository;

        public BudgetManager(IBudgetRepository budgetRepository)
        {
            _budgetRepository = budgetRepository;
        }

        public double QueryTotalAmount(DateTime start, DateTime end)
        {
            var budgets = _budgetRepository.GetAll();
            double totalAmount = 0;

            foreach (var budget in budgets)
            {
                DateTime budgetDate = DateTime.ParseExact(budget.YearMonth, "yyyyMM", null);
                DateTime budgetStart = new DateTime(budgetDate.Year, budgetDate.Month, 1);
                DateTime budgetEnd = budgetStart.AddMonths(1).AddDays(-1);

                if (end < budgetStart || start > budgetEnd)
                {
                    continue; // Skip budgets outside the range
                }

                DateTime effectiveStart = start > budgetStart ? start : budgetStart;
                DateTime effectiveEnd = end < budgetEnd ? end : budgetEnd;

                int daysInMonth = DateTime.DaysInMonth(budgetDate.Year, budgetDate.Month);
                int daysInRange = (effectiveEnd - effectiveStart).Days + 1;

                double monthlyAmount = (double)budget.Amount / daysInMonth * daysInRange;
                totalAmount += monthlyAmount;
            }

            return totalAmount;
        }
    }
} 