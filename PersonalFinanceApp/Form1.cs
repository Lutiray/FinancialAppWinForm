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
    public partial class Form1 : Form
    {
        private FinanceManager managerFinance;
        private bool isInitialBalanceSet = false;

        public Form1()
        {
            InitializeComponent();
            managerFinance = new FinanceManager();

            List<Category> categories = new List<Category>();
            categories.Add(new Category("")); 
            categories.AddRange(managerFinance.Categories);

            cmbCategories.DataSource = managerFinance.Categories;
            cmbCategories.DisplayMember = "Name";

            cmbCategories.SelectedIndexChanged += cmbCategories_SelectedIndexChanged;

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

        private void UpdateBalanceTextBox()
        {
            decimal incomeSum = managerFinance.Transactions?.OfType<Income>().Sum(i => i.Amount) ?? 0;
            decimal expensesSum = managerFinance.Transactions?.OfType<Expenses>().Sum(o => o.Amount) ?? 0;

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

        private void UpdateRichTextBox()
        {
            decimal incomeSum = managerFinance.Transactions?.OfType<Income>().Sum(i => i.Amount) ?? 0;
            decimal expensesSum = managerFinance.Transactions?.OfType<Expenses>().Sum(o => o.Amount) ?? 0;

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
            DisplayDataForSelectedPeriod();
        }

        private void DisplayDataForSelectedPeriod()
        {
            DateTime startDate = dtpStartDate.Value;
            DateTime endDate = dtpEndDate.Value;

            // Фильтруйте данные по выбранному периоду
            var filteredTransactions = managerFinance.Transactions
                .Where(t => t.Date >= startDate && t.Date <= endDate)
                .ToList();

            Console.WriteLine($"Количество транзакций в выбранном периоде: {filteredTransactions.Count}");

            // Обновите отображение в DataGrid и на круговой диаграмме
            PopulateTransactionHistory(filteredTransactions);
            DisplayExpensesPieChart(filteredTransactions);
        }
    }
}
