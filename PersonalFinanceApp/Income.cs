using System;

namespace PersonalFinanceApp
{
    // <summary>
    /// Class Income.
    /// </summary>
    public class Income : Transaction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Income"/> class.
        /// </summary>
        /// <param name="amount">Income amount.</param>
        /// <param name="date">Data of receipt of income.</param>
        public Income(decimal amount, DateTime date) : base(amount, date)
        {
        }
    }
}