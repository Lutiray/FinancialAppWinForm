using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceApp
{
    /// <summary>
    /// Class for transaction.
    /// </summary>
    public class Transaction
    {
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the amount of transaction.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the date of transaction.
        /// </summary>
        public DateTime Date { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Transaction"/> class.
        /// </summary>
        /// <param name="id">Transaction id.</param>
        /// <param name="amount">Transaction amount.</param>
        /// <param name="date">Date of the transaction.</param>
        protected Transaction(int id, decimal amount, DateTime date)
        {
            Id = id;
            Amount = amount;
            Date = date;
        }
    }
}