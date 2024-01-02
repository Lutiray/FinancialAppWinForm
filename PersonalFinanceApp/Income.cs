using System;

namespace PersonalFinanceApp
{
    public class Income : Transaction
    {
        public Income(decimal amount, DateTime date) : base(amount, date)
        {
        }
    }
}