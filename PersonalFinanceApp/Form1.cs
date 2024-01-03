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

namespace PersonalFinanceApp
{
    public partial class btnDeleteTransaction : Form
    {
        private FinanceManager managerFinance;
        private bool isInitialBalanceSet = false;

        public btnDeleteTransaction()
        {
            InitializeComponent();
            managerFinance = new FinanceManager();

            List<Category> categories = new List<Category>();
            categories.Add(new Category("")); 
            categories.AddRange(managerFinance.Categories);

            cmbCategories.DataSource = managerFinance.Categories;
            cmbCategories.DisplayMember = "Name";

            cmbCategories.SelectedIndexChanged += cmbCategories_SelectedIndexChanged;
            //btnDeleteTransaction.Click += btnDeleteTransaction_Click;

            cmbDescription.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategories.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void btnIncome_Click(object sender, EventArgs e)
        {
            if (IsValidAmountInput())
            {
                decimal amount = decimal.Parse(txtbAmount.Text);
                DateTime date = dtpDate.Value;

                Income newIncome = new Income(amount, date);

                managerFinance.AddTransaction(newIncome);

                UpdateBalanceTextBox();
                PopulateTransactionHistory();

                ClearInputFields();
                ResetCategorySelection();
            }
        }
        
        private void btnExpenses_Click(object sender, EventArgs e)
        {
            if (IsValidAmountInput())
            {
                decimal amount = decimal.Parse(txtbAmount.Text);
                Category categories = (Category)cmbCategories.SelectedItem;
                string selectedDescription = cmbDescription.SelectedItem?.ToString();
                DateTime date = dtpDate.Value;

                Expenses newExpences = new Expenses(categories, amount, date, selectedDescription);

                managerFinance.AddTransaction(newExpences);

                UpdateBalanceTextBox();
                PopulateTransactionHistory();
                DisplayExpensesPieChart();

                ClearInputFields();
                ResetCategorySelection();
            } 
        }

        private void ResetCategorySelection()
        {
            cmbCategories.SelectedIndex = 0;
        }

        private void ClearInputFields()
        {
            txtbAmount.Clear();
            txtbAmount.Focus();
            dtpDate.Value = DateTime.Now;
        }

        private bool IsValidAmountInput()
        {
            if (!decimal.TryParse(txtbAmount.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Zadejte správnou hodnotu pro částku.", "Chyba při zadávání", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtbAmount.Clear(); 
                txtbAmount.Focus(); 
                return false;
            }

            return true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeBalance();
            PopulateTransactionHistory();
            DisplayExpensesPieChart();
        }

        private void txtDate_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void cmbCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDescriptionComboBox();
        }

        private void UpdateDescriptionComboBox()
        {
            Category selectedCategory = (Category)cmbCategories.SelectedItem;

            cmbDescription.Items.Clear();

            if(selectedCategory != null )
            {
                bool firstInteration = true;

                foreach (string description in selectedCategory.Descriptions)
                {
                    if (!firstInteration)
                    {
                        cmbDescription.Items.Add(description);
                    }
                    else
                    {
                        firstInteration = false;
                    }
                }
            }
        }

        private void cmbDescription_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtBalance_TextChanged(object sender, EventArgs e)
        {

        }
        
        private void InitializeBalance()
        {
            if (!isInitialBalanceSet)
            {
                txtBalance.Text = "Balance: 0";
                isInitialBalanceSet = true;
            }
        }

        private void UpdateBalanceTextBox(List<Transaction> transactions = null)
        {
            List<Transaction> transactionsToDisplay = transactions ?? managerFinance.Transactions;

            decimal incomeSum = transactionsToDisplay?.OfType<Income>().Sum(i => i.Amount) ?? 0;
            decimal expensesSum = transactionsToDisplay?.OfType<Expenses>().Sum(o => o.Amount) ?? 0;

            decimal balance = incomeSum - expensesSum;

            txtBalance.Text = $"Баланс: {balance.ToString("C2", new System.Globalization.CultureInfo("cs-CZ"))}";

        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtbAmount_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void dgvHistory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void PopulateTransactionHistory(List<Transaction> transactions = null)
        {
            dgvHistory.Rows.Clear();
            InitializeDgvHistory();

            UpdateRichTextBox();

            List<Transaction> transactionsToDisplay = transactions ?? managerFinance.Transactions;

            foreach (Transaction transaction in transactionsToDisplay)
            {
                string type = (transaction is Income) ? "Příjmy" : "Výdaje";
                decimal amount = transaction.Amount;
                DateTime date = transaction.Date;
                string description = (transaction is Expenses expenses) ? expenses.Description : "";

                dgvHistory.Rows.Add(type, amount.ToString("C2", new System.Globalization.CultureInfo("cs-CZ")), date, description);
            }
        }

        private void btnDeleteTransaction_Click(object sender, EventArgs e)
        {
            if (dgvHistory.SelectedRows.Count > 0)
            {
                int selectedIndex = dgvHistory.SelectedRows[0].Index;
                DataGridViewRow selectedRow = dgvHistory.Rows[selectedIndex];

                foreach (DataGridViewColumn column in dgvHistory.Columns)
                {
                    Console.WriteLine($"Column Name: {column.Name}");
                }
                // Получите данные транзакции из DataGridView
                string type = selectedRow.Cells["Type"].Value.ToString();
                decimal amount = decimal.Parse(selectedRow.Cells["Amount"].Value.ToString());
                DateTime date = DateTime.Parse(selectedRow.Cells["DateColumn"].Value.ToString());
                string description = selectedRow.Cells["DescriptionColumn"].Value.ToString();

                // Создайте объект транзакции на основе полученных данных
                Transaction transactionToDelete;
                if (type == "Příjmy")
                {
                    transactionToDelete = new Income(amount, date);
                }
                else
                {
                    // При удалении расхода, вам также потребуется имя категории
                    string categoryName = selectedRow.Cells["CategoryColumn"].Value.ToString();
                    Category category = managerFinance.Categories.FirstOrDefault(c => c.Name == categoryName);

                    if (category != null)
                    {
                        transactionToDelete = new Expenses(category, amount, date, description);
                    }
                    else
                    {
                        // Обработайте ситуацию, если категория не найдена
                        return;
                    }
                }

                // Удалите транзакцию из списка и базы данных
                managerFinance.Transactions.Remove(transactionToDelete);
                managerFinance.SaveTransactionsToDatabase();

                // Обновите DataGridView и баланс
                PopulateTransactionHistory();
                UpdateBalanceTextBox();
            }
            else
            {
                MessageBox.Show("Выберите транзакцию для удаления.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void InitializeDgvHistory()
        {
            dgvHistory.Columns.Clear();

            dgvHistory.Columns.Add("TypeColumn", "Typ");
            dgvHistory.Columns.Add("AmountColumn", "Částka");
            dgvHistory.Columns.Add("DateColumn", "Date");
            dgvHistory.Columns.Add("DescriptionColumn", "Popís");
        }

        private void chartExpenses_Click_1(object sender, EventArgs e)
        {

        }

        private void DisplayExpensesPieChart(List<Transaction> transactions = null)
        {
            chartExpenses.Series.Clear();
            chartExpenses.ChartAreas.Clear();

            ChartArea chartArea = new ChartArea("ExpensesChartArea");
            chartExpenses.ChartAreas.Add(chartArea);

            Series series = new Series("ExpensesSeries");
            series.ChartType = SeriesChartType.Pie;
            series.ChartArea = "ExpensesChartArea";

            List<Transaction> transactionsToDisplay = transactions ?? managerFinance.Transactions;

            var expensesByCategory = transactionsToDisplay
                .OfType<Expenses>()
                .GroupBy(o => o.ExpensesCategory.Name)
                .Select(group => new
                {
                    CategoryName = group.Key,
                    TotalAmount = group.Sum(o => o.Amount)
                });

            foreach (var item in expensesByCategory)
            {
                series.Points.AddXY(item.CategoryName, item.TotalAmount);
            }

            chartExpenses.Series.Add(series);

            if (!chartExpenses.Legends.Any(l => l.Name == "ExpensesLegend"))
            {
                chartExpenses.Legends.Add(new Legend("ExpensesLegend"));
                chartExpenses.Legends["ExpensesLegend"].Docking = Docking.Right;
                chartExpenses.Legends["ExpensesLegend"].Alignment = StringAlignment.Center;
            }

            chartExpenses.ChartAreas["ExpensesChartArea"].InnerPlotPosition = new ElementPosition(5, 5, 90, 90); 

            chartExpenses.Invalidate();
        }

        private void rtxtIncomeAndExpenses_TextChanged(object sender, EventArgs e)
        {

        }

        private void UpdateRichTextBox(List<Transaction> transactions = null)
        {
            List<Transaction> transactionsToDisplay = transactions ?? managerFinance.Transactions;

            decimal incomeSum = transactionsToDisplay?.OfType<Income>().Sum(i => i.Amount) ?? 0;
            decimal expensesSum = transactionsToDisplay?.OfType<Expenses>().Sum(o => o.Amount) ?? 0;

            string summaryText = $" {incomeSum.ToString("C2", new System.Globalization.CultureInfo("cs-CZ"))}\n" +
                                 $"- {expensesSum.ToString("C2", new System.Globalization.CultureInfo("cs-CZ"))}";

            rtxtIncomeAndExpenses.Clear();
            rtxtIncomeAndExpenses.Text = summaryText;
        }

        private void dtpStartDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dtpEndDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnShowTransaction_Click(object sender, EventArgs e)
        {
            SetStartAndEndDateTimes();
            DisplayDataForSelectedPeriod();
        }

        private void DisplayDataForSelectedPeriod()
        {
            DateTime startDate = dtpStartDate.Value;
            DateTime endDate = dtpEndDate.Value;

            var filteredTransactions = managerFinance.Transactions
                .Where(t => t.Date >= startDate && t.Date <= endDate)
                .ToList();

            Console.WriteLine($"Количество транзакций в выбранном периоде: {filteredTransactions.Count}");

            PopulateTransactionHistory(filteredTransactions);
            DisplayExpensesPieChart(filteredTransactions);
            UpdateBalanceTextBox(filteredTransactions);
            UpdateRichTextBox(filteredTransactions);
        }

        private void SetStartAndEndDateTimes()
        {
            dtpStartDate.Value = new DateTime(dtpStartDate.Value.Year, dtpStartDate.Value.Month, dtpStartDate.Value.Day, 0, 0, 0);

            dtpEndDate.Value = new DateTime(dtpEndDate.Value.Year, dtpEndDate.Value.Month, dtpEndDate.Value.Day, 23, 59, 59);
        }
    }
}
