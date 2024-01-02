using System;

namespace PersonalFinanceApp
{
    public class Expenses : Transaction
    {
        public Category ExpensesCategory { get; set; }
        public string Description { get; set; }

        public Expenses(Category expencesCategory, decimal amount, DateTime date, string description)
            : base(amount, date)
        {
            ExpensesCategory = expencesCategory;
            Description = description;
        }
    }
}
