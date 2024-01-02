using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace PersonalFinanceApp
{
    public class FinanceManager
    {
        private string connectionString = "\"Data Source=localhost;Initial Catalog=IncomeAndOutcome;Integrated Security=True;\""; 
        private string categoriesFilePath = "C:\\Users\\user\\source\\repos\\PersonalFinanceApp\\PersonalFinanceApp\\Categories.txt";

        public List<Transaction> Transactions { get; private set; }
        public List<Category> Categories { get; private set; }

        public FinanceManager()
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

                    if (parts.Length >= 1)
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

        private void SaveCategoriesToDatabase()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (Category category in Categories)
                    {
                        foreach (string description in category.Descriptions)
                        {
                            string commandText = "INSERT INTO Categories (Name, Description) " +
                                                 "VALUES (@Name, @Description)";

                            using (SqlCommand command = new SqlCommand(commandText, connection))
                            {
                                command.Parameters.AddWithValue("@Name", category.Name);
                                command.Parameters.AddWithValue("@Description", description);

                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving categories to database: {ex.Message}");
            }
        }

        public void AddTransaction(Transaction transaction)
        {
            Transactions.Add(transaction);
            SaveTransactionToDatabase(transaction);
        }

        private void SaveTransactionToDatabase(Transaction transaction)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string commandText = "INSERT INTO Transactions (Amount, Date, Description, Type) " +
                                         "VALUES (@Amount, @Date, @Description, @Type)";

                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        command.Parameters.AddWithValue("@Amount", transaction.Amount);
                        command.Parameters.AddWithValue("@Date", transaction.Date);
                        command.Parameters.AddWithValue("@Description", (transaction is Expenses expenses) ? expenses.Description : "");
                        command.Parameters.AddWithValue("@Type", (transaction is Income) ? "Income" : "Expenses");

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving transaction to database: {ex.Message}");
            }
        }
    }
}
