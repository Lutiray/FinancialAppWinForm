using System;

namespace PersonalFinanceApp
{
    /// <summary>
    /// Class Expenses.
    /// </summary>
    public class Expenses : Transaction
    {
        /// <summary>
        /// Gets or sets the category expenses.
        /// </summary>
        public Category CategoryExpenses { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Expenses"/> class.
        /// </summary>
        /// <param name="categoryExpenses">Expense category.</param>
        /// <param name="amount">The amount of expenses.</param>
        /// <param name="date">Date of expenditure.</param>
        /// <param name="description">description of expenses.</param>
        public Expenses(Category categoryExpenses, decimal amount, DateTime date, string description)
            : base(amount, date)
        {
            CategoryExpenses = categoryExpenses;
            Description = description;
        }
    }
}
