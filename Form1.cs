using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms.DataVisualization.Charting;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace PersonalFinanceApp
{
    /// <summary>
    /// The main form of the application.
    /// </summary>
    public partial class Form1 : Form
    {
        // The data source of the application.
        private DataSource dataSource;
        // Indicates whether the initial balance has been set.
        //private bool isInitialBalanceSet = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            dataSource = new DataSource();

            // Initialize categories for the combo box.
            List<Category> categories = new List<Category>();
            categories.Add(new Category(""));
            categories.AddRange(dataSource.Categories);
            //dataSource.LoadTransactionsFromDatabase();

            // Set data source for the categories combo box.
            cmbCategories.DataSource = dataSource.Categories;
            cmbCategories.DisplayMember = "Name";

            // Attach event handler for category selection change.
            cmbCategories.SelectedIndexChanged += cmbCategories_SelectedIndexChanged;

            // Set combo box styles.
            cmbDescription.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategories.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        /// <summary>
        /// Handler for the form load event.
        /// </summary>
        private async void Form1_Load(object sender, EventArgs e)
        {
            //Load previous transaction from database, if database is not empty.
            if (!dataSource.IsDatabaseEmpty())
            {
                await dataSource.LoadAllTransactionsFromDatabaseAsync();
            }
            // Load the transaction history into the data grid view.
            PopulateTransactionHistory();
            //Initialize the balance.
            UpdateBalanceTextBox();
            // Show the expenses pie chart.
            DisplayExpensesPieChart();
        }

        /// <summary>
        /// Handler for the income button click event.
        /// </summary>
        private void btnIncome_Click(object sender, EventArgs e)
        {
            // Check and parse the input amount.
            if (IsValidAmountInput())
            {
                decimal amount = decimal.Parse(txtbAmount.Text);
                DateTime date = dtpDate.Value;

                // Create a new income transaction.
                Income newIncome = new Income(0, amount, date);

                // Handle the transaction, update UI.
                HandleTransaction(newIncome, DateTime.Now);
            }
        }

        /// <summary>
        /// Handler for the expenses button click event.
        /// </summary>
        private void btnExpenses_Click(object sender, EventArgs e)
        {
            // Check and parse the input amount.
            if (IsValidAmountInput())
            {
                decimal amount = decimal.Parse(txtbAmount.Text);
                Category categories = (Category)cmbCategories.SelectedItem;
                string selectedDescription = cmbDescription.SelectedItem?.ToString();
                DateTime date = dtpDate.Value;

                // Create a new expenses transaction.
                Expenses newExpenses = new Expenses(categories, 0, amount, date, selectedDescription);

                // Handle the transaction, update UI.
                HandleTransaction(newExpenses, DateTime.Now);
                DisplayExpensesPieChart();
            }
        }

        /// <summary>
        /// Checks if the entered amount is a valid positive decimal value.
        /// </summary>
        /// <returns>True if the amount is valid; otherwise, false.</returns>
        private bool IsValidAmountInput()
        {
            if (!decimal.TryParse(txtbAmount.Text, out decimal amount) || amount <= 0)
            {
                ShowErrorMessage("Enter the correct value for the amount.");
                txtbAmount.Clear();
                txtbAmount.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks the date of the transaction, adds it to the data source, and updates the balance and UI.
        /// </summary>
        private void HandleTransaction(Transaction transaction, DateTime maxDate)
        {
            // Check if the date of the transaction is not greater than the current date.
            if (transaction.Date > maxDate)
            {
                ShowErrorMessage("The selected data cannot be greater then the current data.");
                return;
            }

            // Add the transaction to the data source.
            dataSource.AddTransaction(transaction);

            // Update the balance text box.
            UpdateBalanceTextBox();

            // Populate the transaction history.
            PopulateTransactionHistory();

            // Clear input fields after successful transaction.
            ClearInputFields();

            // Reset category selection in the combo box.
            ResetCategorySelection();
        }

        /// <summary>
        /// Resets the selected category in the combo box.
        /// </summary>
        private void ResetCategorySelection()
        {
            cmbCategories.SelectedIndex = 0;
        }

        /// <summary>
        /// Clears the input fields for amount and date.
        /// </summary>
        private void ClearInputFields()
        {
            txtbAmount.Clear();
            txtbAmount.Focus();
            dtpDate.Value = DateTime.Now;
        }

        private void txtDate_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handler for the category selection change event.
        /// Updates the description combo box based on the selected category.
        /// </summary>
        private void cmbCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDescriptionComboBox();
        }

        /// <summary>
        /// Updates the description combo box based on the selected category.
        /// </summary>
        private void UpdateDescriptionComboBox()
        {
            // Get the currently selected category from the combo box.
            Category selectedCategory = (Category)cmbCategories.SelectedItem;

            // Clear existing items in the description combo box.
            cmbDescription.Items.Clear();

            // Check if a category is selected.
            if (selectedCategory != null)
            {
                // Iterate through each description in the selected category.
                foreach (string description in selectedCategory.Descriptions)
                {
                    cmbDescription.Items.Add(description);
                }
            }
        }

        /// <summary>
        /// Handler for the selected index change event of the description combo box.
        /// </summary>
        private void cmbDescription_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handler for the text change event of the balance text box.
        /// </summary>
        private void txtBalance_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Updates the balance text box with the calculated balance from the transactions.
        /// </summary>
        /// <param name="transactions">The list of transactions to consider (defaults to all transactions).</param>
        private void UpdateBalanceTextBox(List<Transaction> transactions = null)
        {
            // Select the list of transactions to display based on the provided parameter.
            List<Transaction> transactionsToDisplay = transactions ?? dataSource.Transactions;

            // Calculate the sum of incomes and expenses from the selected transactions.
            decimal incomeSum = transactionsToDisplay?.OfType<Income>().Sum(i => i.Amount) ?? 0;
            decimal expensesSum = transactionsToDisplay?.OfType<Expenses>().Sum(o => o.Amount) ?? 0;

            // Calculate the balance.
            decimal balance = incomeSum - expensesSum;

            // Update the balance text box with the formatted balance.
            txtBalance.Text = $"Bilance: {balance.ToString("C2", new System.Globalization.CultureInfo("cs-CZ"))}";

        }

        /// <summary>
        /// Handler for the value change event of the date time picker for date selection.
        /// </summary>
        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handler for the text change event of the amount text box.
        /// </summary>
        private void txtbAmount_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handler for the cell content click event of the transaction history data grid view.
        /// </summary>
        private void dgvHistory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        /// <summary>
        /// Populates the transaction history data grid view based on the provided transactions.
        /// </summary>
        /// <param name="transactions">The list of transactions to display (defaults to all transactions).</param>
        private void PopulateTransactionHistory(List<Transaction> transactions = null)
        {
            // Clear existing rows in the data grid view.
            dgvHistory.Rows.Clear();
            // Initialize the structure of the data grid view.
            InitializeDgvHistory();

            // Update the rich text box with relevant information.
            UpdateRichTextBox();

            // Select the list of transactions to display based on the provided parameter.
            List<Transaction> transactionsToDisplay = transactions ?? dataSource.Transactions;
            // Dictionary to group transactions by month.
            Dictionary<DateTime, List<Transaction>> transactionsByMonth = new Dictionary<DateTime, List<Transaction>>();

            // Iterate through each transaction and group by month.
            foreach (Transaction transaction in transactionsToDisplay)
            {
                DateTime monthKey = new DateTime(transaction.Date.Year, transaction.Date.Month, 1);

                if (!transactionsByMonth.ContainsKey(monthKey))
                {
                    transactionsByMonth[monthKey] = new List<Transaction>();
                }

                transactionsByMonth[monthKey].Add(transaction);
            }

            // Iterate through each month's transactions and add rows to the data grid view.
            foreach (var monthTransactionsPair in transactionsByMonth.OrderByDescending(pair => pair.Key))
            {
                dgvHistory.Rows.Add("", monthTransactionsPair.Key.ToString("MMMM yyyy", new System.Globalization.CultureInfo("cs-CZ")), "", "", "", "");

                foreach (Transaction transaction in monthTransactionsPair.Value)
                {
                    // Add a row for each transaction in the month.
                    AddTransactionRow(transaction);
                }
            }
        }

        /// <summary>
        /// Adds a new row to the transaction history data grid view based on the provided transaction.
        /// </summary>
        /// <param name="transaction">The transaction to be added to the data grid view.</param>
        private void AddTransactionRow(Transaction transaction)
        {
            // Determine the type of the transaction (Income or Expenses).
            string type = (transaction is Income) ? "Příjmy" : "Výdaje";

            // Extract common attributes from the transaction.
            int transactionId = transaction.Id;
            decimal amount = transaction.Amount;
            DateTime date = transaction.Date;
            string category = "";
            string description = "";

            // Extract specific attributes for Expenses transactions.
            if (transaction is Expenses expenses)
            {
                category = (transaction is Income) ? "" : expenses.CategoryExpenses.Name;
                description = expenses.Description;
            }

            // Determine the color for the amount based on the transaction type.
            Color amountColor = (transaction is Income) ? Color.Green : Color.Red;

            // Create a new row for the data grid view.
            DataGridViewRow row = new DataGridViewRow();

            // Create cells with relevant data for the row.
            row.CreateCells(dgvHistory, transactionId, type, amount.ToString("C2", new System.Globalization.CultureInfo("cs-CZ")), date, category, description);

            // Set the text color for the amount cell based on the transaction type.
            row.Cells[1].Style.ForeColor = amountColor;

            // Add the transaction ID to a hidden cell (column index 0)
            DataGridViewTextBoxCell idCell = new DataGridViewTextBoxCell();
            idCell.Value = transactionId;
            row.Cells[0] = idCell;

            // Hide the transaction ID column
            //Columns[0].Visible = false;

            // Add the row to the data grid view.
            dgvHistory.Rows.Add(row);
        }

        /// <summary>
        /// Initializes the structure of the transaction history data grid view.
        /// </summary>
        private void InitializeDgvHistory()
        {
            // Clear existing columns in the data grid view.
            dgvHistory.Columns.Clear();

            // Add a hidden column for transaction ID
            dgvHistory.Columns.Add("IdColumn", "ID");
            dgvHistory.Columns["IdColumn"].Visible = false;
            // Add columns for different attributes in the data grid view.
            dgvHistory.Columns.Add("TypeColumn", "Typ");
            dgvHistory.Columns.Add("AmountColumn", "Částka");
            dgvHistory.Columns.Add("DateColumn", "Date");
            dgvHistory.Columns.Add("CategoryColumn", "Kategorie");
            dgvHistory.Columns.Add("DescriptionColumn", "Popís");
        }

        /// <summary>
        /// Handler for the delete transaction button click event.
        /// </summary>
        private void btnDeleteTransaction_Click(object sender, EventArgs e)
        {
            // Check if any row is selected in the data grid view.
            if (dgvHistory.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvHistory.SelectedRows[0];

                // Check if the selected row represents a transaction by cheking if the amount cell is not empty.
                if (!string.IsNullOrEmpty(selectedRow.Cells[1].Value?.ToString()))
                {
                    int selectedIndex = selectedRow.Index;
                    int monthRowCount = 0;

                    // Count the number of rows representing months above the selected row.
                    for (int i = 0; i < selectedIndex; i++)
                    {
                        if (string.IsNullOrEmpty(dgvHistory.Rows[i].Cells[1].Value?.ToString()))
                        {
                            monthRowCount++;
                        }
                    }
                    // Adjust the index of the transaction based on the number of rows representing months.
                    int adjustedIndex = Math.Max(0, selectedIndex - monthRowCount);

                    // Retrieve the transaction ID from the hidden ID column.
                    int transactionId = Convert.ToInt32(dgvHistory.Rows[selectedRow.Index].Cells["IdColumn"].Value);

                    // Remove the transaction from the data source based on the adjusted index.
                    if (adjustedIndex >= 0 && adjustedIndex < dataSource.Transactions.Count)
                    {
                        dataSource.Transactions.RemoveAt(adjustedIndex);

                        // Remove the transaction from the database
                        dataSource.DeleteTransactionFromDatabase(transactionId);
                    }

                    //Test: Check deleted transaction
                    /*foreach (var transaction in dataSource.Transactions)
                    {
                        Console.WriteLine(transaction.Amount);
                    }*/

                    // Update various components after deleting the transaction.
                    PopulateTransactionHistory();
                    DisplayExpensesPieChart();
                    UpdateBalanceTextBox();
                    UpdateRichTextBox();
                }
                else
                {
                    ShowErrorMessage("Cannot delete a month. Please select a transaction to delete.");
                }
            }
            else
            {
                ShowErrorMessage("Select a transaction to delete.");
            }
        }

        /// <summary>
        /// Displays an error message in a message box.
        /// </summary>
        /// <param name="errorMessage">The error message to be displayed.</param>
        private void ShowErrorMessage(string errorMessage)
        {
            MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void chartExpenses_Click_1(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Displays the expenses pie chart based on the provided transactions.
        /// </summary>
        /// <param name="transactions">The list of transactions to consider (defaults to all transactions).</param>
        private void DisplayExpensesPieChart(List<Transaction> transactions = null)
        {
            // Clear existing series and chart areas in the expenses pie chart.
            chartExpenses.Series.Clear();
            chartExpenses.ChartAreas.Clear();

            // Create a new chart area for the expenses pie chart.
            ChartArea chartArea = new ChartArea("ExpensesChartArea");
            chartExpenses.ChartAreas.Add(chartArea);

            // Create a new series for the expenses pie chart.
            Series series = new Series("ExpensesSeries");
            series.ChartType = SeriesChartType.Pie;
            series.ChartArea = "ExpensesChartArea";

            // Select the list of transactions to display based on the provided parameter.
            List<Transaction> transactionsToDisplay = transactions ?? dataSource.Transactions;

            // Group Expenses transactions by category and calculate total amounts.
            var expensesByCategory = transactionsToDisplay
                .OfType<Expenses>()
                .GroupBy(o => o.CategoryExpenses.Name)
                .Select(group => new
                {
                    CategoryName = group.Key,
                    TotalAmount = group.Sum(o => o.Amount)
                });

            // Add data points to the series based on grouped expenses.
            foreach (var item in expensesByCategory)
            {
                series.Points.AddXY(item.CategoryName, item.TotalAmount);
            }

            // Add the series to the expenses pie chart.
            chartExpenses.Series.Add(series);

            // Add a legend to the expenses pie chart if it doesn't exist.
            if (!chartExpenses.Legends.Any(l => l.Name == "ExpensesLegend"))
            {
                chartExpenses.Legends.Add(new Legend("ExpensesLegend"));
                chartExpenses.Legends["ExpensesLegend"].Docking = Docking.Right;
                chartExpenses.Legends["ExpensesLegend"].Alignment = StringAlignment.Center;
            }

            // Set the inner plot position for the expenses pie chart.
            chartExpenses.ChartAreas["ExpensesChartArea"].InnerPlotPosition = new ElementPosition(5, 5, 90, 90);

            // Force the chart to redraw.
            chartExpenses.Invalidate();
        }

        private void rtxtIncomeAndExpenses_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Updates the content of the RichTextBox displaying the summary of income and expenses.
        /// </summary>
        /// <param name="transactions">The list of transactions to consider (defaults to all transactions).</param>
        private void UpdateRichTextBox(List<Transaction> transactions = null)
        {
            // Select the list of transactions to display based on the provided parameter.
            List<Transaction> transactionsToDisplay = transactions ?? dataSource.Transactions;

            // Calculate the total income and expenses.
            decimal incomeSum = transactionsToDisplay?.OfType<Income>().Sum(i => i.Amount) ?? 0;
            decimal expensesSum = transactionsToDisplay?.OfType<Expenses>().Sum(o => o.Amount) ?? 0;

            // Generate a summary text for the RichTextBox.
            string summaryText = $" {incomeSum.ToString("C2", new System.Globalization.CultureInfo("cs-CZ"))}\n" +
                                 $"- {expensesSum.ToString("C2", new System.Globalization.CultureInfo("cs-CZ"))}";

            // Clear existing content and set the summary text in the RichTextBox.
            rtxtIncomeAndExpenses.Clear();
            rtxtIncomeAndExpenses.Text = summaryText;
        }

        /// <summary>
        /// Handler for the value changed event of the start date DateTimePicker.
        /// </summary>
        private void dtpStartDate_ValueChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handler for the value changed event of the end date DateTimePicker.
        /// </summary>
        private void dtpEndDate_ValueChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handler for the click event of the "Show Transactions" button.
        /// </summary>
        private void btnShowTransaction_Click(object sender, EventArgs e)
        {
            // Set start and end date times, then display data for the selected period.
            SetStartAndEndDateTimes();
            DisplayDataForSelectedPeriod();
        }

        /// <summary>
        /// Displays data in various components based on the selected date range.
        /// </summary>
        private void DisplayDataForSelectedPeriod()
        {
            // Retrieve the start and end dates from the DateTimePicker controls.
            DateTime startDate = dtpStartDate.Value;
            DateTime endDate = dtpEndDate.Value;

            // Check if the start date is greater than the end date.
            if (startDate > endDate)
            {
                ShowErrorMessage("The start date cannot be greater than the end date.");
                return;
            }

            // Filter transactions based on the selected date range.
            var filteredTransactions = dataSource.Transactions
                .Where(t => t.Date >= startDate && t.Date <= endDate)
                .ToList();

            // Populate various components with the filtered transactions.
            PopulateTransactionHistory(filteredTransactions);
            DisplayExpensesPieChart(filteredTransactions);
            UpdateBalanceTextBox(filteredTransactions);
            UpdateRichTextBox(filteredTransactions);
        }

        /// <summary>
        /// Sets the time component of the start and end dates to the beginning and end of the day, respectively.
        /// </summary>
        private void SetStartAndEndDateTimes()
        {
            // Set the time component of the start date to the beginning of the day.
            dtpStartDate.Value = new DateTime(dtpStartDate.Value.Year, dtpStartDate.Value.Month, dtpStartDate.Value.Day, 0, 0, 0);

            // Set the time component of the end date to the end of the day.
            dtpEndDate.Value = new DateTime(dtpEndDate.Value.Year, dtpEndDate.Value.Month, dtpEndDate.Value.Day, 23, 59, 59);
        }

        /// <summary>
        /// Handler for the click event of the "Reset Date" button.
        /// </summary>
        private void btnResetDate_Click(object sender, EventArgs e)
        {
            // Reset start and end dates, then display data for the selected period.
            dtpStartDate.Value = DateTimePicker.MinimumDateTime;
            dtpEndDate.Value = DateTime.Now;
            DisplayDataForSelectedPeriod();
        }
    }
}
