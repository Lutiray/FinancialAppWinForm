using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceApp
{
    public class Transaction
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        protected Transaction(decimal amount, DateTime date)
        {
            Amount = amount;
            Date = date;
        }
    }
}