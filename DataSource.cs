using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceApp
{
    public class DataSource
    {
        private readonly string connectionString = "Data Source=localhost;Initial Catalog=IncomeAndOutcome;Integrated Security=True;Connect Timeout=30;Encrypt=False";
        //private readonly string categoriesFilePath = "C:\\Users\\user\\source\\repos\\PersonalFinanceApp\\PersonalFinanceApp\\Categories.txt";

        //private readonly string categoriesFilePath = "C:\\Users\\user\\source\\repos\\PersonalFinanceApp\\PersonalFinanceApp\\Categories.txt";
        private readonly string categoriesFilePath = "..\\PersonalFinanceApp\\Categories.txt";


        public List<Transaction> Transactions { get; private set; }
        public List<Category> Categories { get; private set; }

        private int pageSize = 100;

        public DataSource()
        {
            Transactions = new List<Transaction>();
            Categories = LoadCategoriesFromTextFile();
        }

        private List<Category> LoadCategoriesFromTextFile()
        {
            List<Category> categories = new List<Category>();

            try
            {
                string[] lines = File.ReadAllLines(categoriesFilePath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');

                    if (parts.Length > 0)
                    {
                        string categoryName = parts[0].Trim();

                        Category existingCategory = categories.FirstOrDefault(c => c.Name == categoryName);

                        if (existingCategory != null)
                        {
                            for (int i = 1; i < parts.Length; i++)
                            {
                                string categoryDescription = parts[i].Trim();
                                existingCategory.AddDescription(categoryDescription);
                            }
                        }
                        else
                        {
                            Category newCategory = new Category(categoryName);

                            for (int i = 1; i < parts.Length; i++)
                            {
                                string categoryDescription = parts[i].Trim();
                                newCategory.AddDescription(categoryDescription);
                            }

                            categories.Add(newCategory);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading categories from text file: {ex.Message}");
            }

            return categories;
        }

        public void SaveTransactionToDatabase(Transaction transaction)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string commandText = "INSERT INTO Transactions (Amount, Date, Description_ID, Type, CategoryID) " +
                                         "VALUES (@Amount, @Date, @Description_ID, @Type, @CategoryID); SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        command.Parameters.AddWithValue("@Amount", transaction.Amount);
                        command.Parameters.AddWithValue("@Date", transaction.Date);

                        if (transaction is Income)
                        {
                            command.Parameters.AddWithValue("@Type", "Income");
                            command.Parameters.AddWithValue("@CategoryID", DBNull.Value);
                            command.Parameters.AddWithValue("@Description_ID", DBNull.Value);
                        }
                        else if (transaction is Expenses expensesTransaction)
                        {
                            command.Parameters.AddWithValue("@Type", "Expenses");
                            command.Parameters.AddWithValue("@CategoryID", GetCategoryId(expensesTransaction.CategoryExpenses.Name));
                            command.Parameters.AddWithValue("@Description_ID",
                                !string.IsNullOrEmpty(expensesTransaction.Description)
                                ? (object)GetDescriptionId(expensesTransaction.Description)
                                : DBNull.Value);
                        }

                        int transactionId = Convert.ToInt32(command.ExecuteScalar());
                        transaction.Id = transactionId;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving transaction to SQL Server database: {ex.Message}");
            }
        }


        private int GetIdFromDatabase(string query, string parameterName, string parameterValue)
        {
            int id = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (parameterValue != null)
                        {
                            command.Parameters.AddWithValue(parameterName, parameterValue);
                        }
                        else
                        {
                            Console.WriteLine($"Parameter {parameterName} is null.");
                            return id;
                        }

                        var result = command.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            id = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving ID from SQL Server database: {ex.Message}");
            }

            return id;
        }

        private int GetCategoryId(string category)
        {
            string query = "SELECT CategoryID FROM Categories WHERE Name = @Category";
            return GetIdFromDatabase(query, "@Category", category);
        }

        private int GetDescriptionId(string description)
        {
            string query = "SELECT Description_ID FROM Description WHERE Name = @Description";
            return GetIdFromDatabase(query, "@Description", description);
        }

        public bool IsDatabaseEmpty()
        {
            bool isEmpty = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string commandText = "SELECT COUNT(*) FROM Transactions";

                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        int rowCount = Convert.ToInt32(command.ExecuteScalar());
                        isEmpty = rowCount == 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking if database is empty: {ex.Message}");
            }

            return isEmpty;
        }

        private async Task<List<Transaction>> LoadTransactionsFromDatabaseAsync(int pageSize, int pageNumber)
        {
            List<Transaction> transactions = new List<Transaction>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string commandText = "SELECT * FROM Transactions ORDER BY Date OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        int offset = (pageNumber - 1) * pageSize;
                        command.Parameters.AddWithValue("@Offset", offset);
                        command.Parameters.AddWithValue("@PageSize", pageSize);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                int id = reader.GetInt32(0);
                                decimal amount = reader.GetDecimal(1);
                                DateTime date = reader.GetDateTime(2);

                                string type = reader.GetString(3);
                                if (type == "Income")
                                {
                                    transactions.Add(new Income(id, amount, date));
                                }
                                else if (type == "Expenses")
                                {
                                    int categoryId = reader.IsDBNull(4) ? -1 : reader.GetInt32(4);
                                    int descriptionId = reader.IsDBNull(5) ? -1 : reader.GetInt32(5);

                                    string categoryName = categoryId != -1 ? GetCategoryNameById(categoryId) : null;
                                    Category category = new Category(categoryName);

                                    string description = descriptionId != -1 ? GetDescriptionById(descriptionId) : null;
                                    transactions.Add(new Expenses(category, id, amount, date, description));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading transactions from SQL Server database: {ex.Message}");
            }

            return transactions;
        }


        public async Task LoadAllTransactionsFromDatabaseAsync()
        {
            int pageNumber = 1;
            List<Transaction> allTransactions = new List<Transaction>();

            // Load transactions for each page until no more transactions are found
            while (true)
            {
                List<Transaction> transactions = await LoadTransactionsFromDatabaseAsync(pageSize, pageNumber);

                // Break the loop if no transactions are loaded for the current page
                if (transactions.Count == 0)
                {
                    break;
                }

                // Add transactions to the list
                allTransactions.AddRange(transactions);

                // Move to the next page
                pageNumber++;
            }

            // Add all transactions to the DataSource
            foreach (var transaction in allTransactions)
            {
                AddTransaction(transaction);
            }
        }

        private string GetValueById(string tableName, string idColumnName, int id, string valueColumnName)
        {
            string value = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string commandText = $"SELECT {valueColumnName} FROM {tableName} WHERE {idColumnName} = @Id";

                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);

                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            value = result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving value from SQL Server database: {ex.Message}");
            }

            return value;
        }

        private string GetCategoryNameById(int categoryId)
        {
            return GetValueById("Categories", "CategoryID", categoryId, "Name");
        }

        private string GetDescriptionById(int descriptionId)
        {
            return GetValueById("Description", "Description_ID", descriptionId, "Name");
        }

        public void AddTransaction(Transaction transaction)
        {
            if (transaction.Id == 0)
            {
                SaveTransactionToDatabase(transaction);

                int transactionId = GetTransactionIdFromDatabase(transaction);

                transaction.Id = transactionId;
            }

            Transactions.Add(transaction);
        }

        private int GetTransactionIdFromDatabase(Transaction transaction)
        {
            int transactionId = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string commandText = "SELECT Id FROM Transactions WHERE Amount = @Amount AND Date = @Date";

                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        command.Parameters.AddWithValue("@Amount", transaction.Amount);
                        command.Parameters.AddWithValue("@Date", transaction.Date);

                        object result = command.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            transactionId = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving transaction ID from SQL Server database: {ex.Message}");
            }

            return transactionId;
        }


        public void DeleteTransactionFromDatabase(int transactionId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string commandText = "DELETE FROM Transactions WHERE TransactionId = @TransactionId";

                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        command.Parameters.AddWithValue("@TransactionId", transactionId);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting transaction from SQL Server database: {ex.Message}");
            }
        }
    }
}