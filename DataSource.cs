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
    /// <summary>
    /// Represents the data source for the Personal Finance Application.
    /// </summary>
    public class DataSource
    {
        // Connection string to the SQL Server database.
        private readonly string connectionString = "Data Source=localhost;Initial Catalog=IncomeAndOutcome;Integrated Security=True;Connect Timeout=30;Encrypt=False";

        // Path to the categories text file.
        private readonly string categoriesFilePath = "C:\\Users\\user\\source\\repos\\PersonalFinanceApp\\PersonalFinanceApp\\Categories.txt";

        /// <summary>
        /// Gets the list of transactions.
        /// </summary>
        public List<Transaction> Transactions { get; private set; }

        /// <summary>
        /// Gets the list of categories.
        /// </summary>
        public List<Category> Categories { get; private set; }

        // Page size for loading transactions.
        private int pageSize = 100;

        /// <summary>
        /// Initializes a new instance of the DataSource class.
        /// </summary>
        public DataSource()
        {
            Transactions = new List<Transaction>();
            Categories = LoadCategoriesFromTextFile();
        }

        // Loads categories from the text file.
        /// <summary>
        /// Loads categories from a text file.
        /// </summary>
        /// <returns>The list of categories loaded from the text file.</returns>
        private List<Category> LoadCategoriesFromTextFile()
        {
            // Initialize a new list to store categories.
            List<Category> categories = new List<Category>();

            try
            {
                // Read all lines from the categories text file.
                string[] lines = File.ReadAllLines(categoriesFilePath);

                // Iterate through each line in the text file.
                foreach (string line in lines)
                {
                    // Split the line by comma to separate category name and descriptions.
                    string[] parts = line.Split(',');

                    // Check if there are parts in the line.
                    if (parts.Length > 0)
                    {
                        // Extract the category name from the first part.
                        string categoryName = parts[0].Trim();

                        // Check if the category already exists in the list.
                        Category existingCategory = categories.FirstOrDefault(c => c.Name == categoryName);

                        // If the category already exists, add descriptions to it.
                        if (existingCategory != null)
                        {
                            // Iterate through the parts starting from the second one (descriptions).
                            for (int i = 1; i < parts.Length; i++)
                            {
                                string categoryDescription = parts[i].Trim();
                                existingCategory.AddDescription(categoryDescription);
                            }
                        }
                        // If the category does not exist, create a new category and add it to the list.
                        else
                        {
                            // Create a new category with the extracted name.
                            Category newCategory = new Category(categoryName);

                            // Iterate through the parts starting from the second one (descriptions).
                            for (int i = 1; i < parts.Length; i++)
                            {
                                string categoryDescription = parts[i].Trim();
                                newCategory.AddDescription(categoryDescription);
                            }

                            // Add the new category to the list of categories.
                            categories.Add(newCategory);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during file reading or category loading.
                Console.WriteLine($"Error loading categories from text file: {ex.Message}");
            }

            // Return the list of categories loaded from the text file.
            return categories;
        }

        /// <summary>
        /// Saves a transaction to the SQL Server database.
        /// </summary>
        /// <param name="transaction">The transaction to be saved.</param>
        public void SaveTransactionToDatabase(Transaction transaction)
        {
            try
            {
                // Open a connection to the SQL Server database using the connection string.
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Define the SQL command text to insert a new transaction into the Transactions table.
                    string commandText = "INSERT INTO Transactions (Amount, Date, Description_ID, Type, CategoryID) " +
                                         "VALUES (@Amount, @Date, @Description_ID, @Type, @CategoryID); SELECT SCOPE_IDENTITY();";

                    // Create a SQL command object with the command text and the database connection.
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        // Set the parameter values for the SQL command.
                        command.Parameters.AddWithValue("@Amount", transaction.Amount); // Amount of the transaction
                        command.Parameters.AddWithValue("@Date", transaction.Date); // Date of the transaction

                        // Check the type of transaction (Income or Expenses) and set the corresponding parameters.
                        if (transaction is Income)
                        {
                            // If it's an Income transaction, set the Type parameter to "Income".
                            command.Parameters.AddWithValue("@Type", "Income");

                            // Since Income transactions don't have a category or description, set the CategoryID and Description_ID parameters to DBNull.Value.
                            command.Parameters.AddWithValue("@CategoryID", DBNull.Value); // No category for Income
                            command.Parameters.AddWithValue("@Description_ID", DBNull.Value); // No description for Income
                        }
                        else if (transaction is Expenses expensesTransaction)
                        {
                            // If it's an Expenses transaction, set the Type parameter to "Expenses".
                            command.Parameters.AddWithValue("@Type", "Expenses");

                            // Get the category ID from the database based on the category name of the Expenses transaction.
                            command.Parameters.AddWithValue("@CategoryID", GetCategoryId(expensesTransaction.CategoryExpenses.Name));

                            // Get the description ID from the database based on the description of the Expenses transaction.
                            // If the description is null or empty, set the parameter to DBNull.Value.
                            command.Parameters.AddWithValue("@Description_ID",
                                !string.IsNullOrEmpty(expensesTransaction.Description)
                                ? (object)GetDescriptionId(expensesTransaction.Description)
                                : DBNull.Value);
                        }

                        // Execute the SQL command and retrieve the newly generated transaction ID using SCOPE_IDENTITY().
                        int transactionId = Convert.ToInt32(command.ExecuteScalar());

                        // Update the transaction object's ID with the newly generated transaction ID from the database.
                        transaction.Id = transactionId;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the database operation and display an error message.
                Console.WriteLine($"Error saving transaction to SQL Server database: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves an ID from the database based on the provided query, parameter name, and parameter value.
        /// </summary>
        /// <param name="query">The SQL query to execute.</param>
        /// <param name="parameterName">The name of the parameter in the SQL query.</param>
        /// <param name="parameterValue">The value of the parameter in the SQL query.</param>
        /// <returns>The retrieved ID from the database.</returns>
        private int GetIdFromDatabase(string query, string parameterName, string parameterValue)
        {
            // Initialize the ID to a default value of -1
            int id = -1;

            try
            {
                // Establish a connection to the SQL Server database using the provided connection string
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the database connection
                    connection.Open();

                    // Create a new SQL command with the provided query and connection
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Check if the parameter value is not null
                        if (parameterValue != null)
                        {
                            // Add the parameter to the SQL command with its name and value
                            command.Parameters.AddWithValue(parameterName, parameterValue);
                        }
                        else
                        {
                            // If the parameter value is null, write a message to the console and return the default ID
                            Console.WriteLine($"Parameter {parameterName} is null.");
                            return id;
                        }

                        // Execute the SQL command and retrieve the scalar result (single value)
                        var result = command.ExecuteScalar();

                        // Check if the result is not null and not DBNull.Value
                        if (result != null && result != DBNull.Value)
                        {
                            // Convert the result to an integer and assign it to the ID variable
                            id = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during database access and display an error message
                Console.WriteLine($"Error retrieving ID from SQL Server database: {ex.Message}");
            }

            // Return the retrieved ID (or the default value if an error occurred)
            return id;
        }

        /// <summary>
        /// Retrieves the ID of a category from the database based on its name.
        /// </summary>
        /// <param name="category">The name of the category.</param>
        /// <returns>The ID of the category.</returns>
        private int GetCategoryId(string category)
        {
            // Define the SQL query to retrieve the CategoryID from the Categories table based on the category name.
            string query = "SELECT CategoryID FROM Categories WHERE Name = @Category";

            // Call the GetIdFromDatabase method to execute the query and retrieve the ID.
            // Pass the query, parameter name, and category name to the GetIdFromDatabase method.
            return GetIdFromDatabase(query, "@Category", category);
        }

        /// <summary>
        /// Retrieves the ID of a description from the database based on its name.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>The ID of the description.</returns>
        private int GetDescriptionId(string description)
        {
            // Define the SQL query to retrieve the Description_ID from the Description table based on the description.
            string query = "SELECT Description_ID FROM Description WHERE Name = @Description";

            // Call the GetIdFromDatabase method to execute the query and retrieve the ID.
            // Pass the query, parameter name, and description to the GetIdFromDatabase method.
            return GetIdFromDatabase(query, "@Description", description);
        }

        /// <summary>
        /// Checks if the database table containing transactions is empty.
        /// </summary>
        /// <returns>True if the database is empty, otherwise false.</returns>
        public bool IsDatabaseEmpty()
        {
            // Initialize isEmpty flag to false
            bool isEmpty = false;

            try
            {
                // Create a new SqlConnection using the connection string
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the database connection
                    connection.Open();

                    // Define the SQL command text to count rows in the Transactions table
                    string commandText = "SELECT COUNT(*) FROM Transactions";

                    // Create a new SqlCommand with the command text and connection
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        // Execute the command and get the row count
                        int rowCount = Convert.ToInt32(command.ExecuteScalar());

                        // Set isEmpty to true if the row count is 0
                        isEmpty = rowCount == 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // If an exception occurs, write the error message to the console
                Console.WriteLine($"Error checking if database is empty: {ex.Message}");
            }

            // Return the isEmpty flag
            return isEmpty;
        }

        /// <summary>
        /// Asynchronously loads transactions from the database based on pagination.
        /// </summary>
        /// <param name="pageSize">The number of transactions to retrieve per page.</param>
        /// <param name="pageNumber">The current page number.</param>
        /// <returns>A list of transactions loaded from the database.</returns>
        private async Task<List<Transaction>> LoadTransactionsFromDatabaseAsync(int pageSize, int pageNumber)
        {
            // Initialize an empty list to store the loaded transactions
            List<Transaction> transactions = new List<Transaction>();

            try
            {
                // Create a new SqlConnection using the connection string
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the database connection asynchronously
                    await connection.OpenAsync();

                    // SQL command text to retrieve transactions with pagination
                    string commandText = "SELECT * FROM Transactions ORDER BY Date OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

                    // Create a new SqlCommand with the command text and SqlConnection
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        // Calculate the offset based on the page number and page size
                        int offset = (pageNumber - 1) * pageSize;

                        // Set the parameters for OFFSET and FETCH NEXT
                        command.Parameters.AddWithValue("@Offset", offset);
                        command.Parameters.AddWithValue("@PageSize", pageSize);

                        // Execute the command and retrieve data asynchronously
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            // Read each row of data from the SqlDataReader
                            while (await reader.ReadAsync())
                            {
                                // Extract common transaction data
                                int id = reader.GetInt32(0);
                                decimal amount = reader.GetDecimal(1);
                                DateTime date = reader.GetDateTime(2);

                                // Extract the type of transaction (Income or Expenses)
                                string type = reader.GetString(3);

                                // Create the appropriate transaction object based on the type
                                if (type == "Income")
                                {
                                    transactions.Add(new Income(id, amount, date));
                                }
                                else if (type == "Expenses")
                                {
                                    // Extract additional data for Expenses transactions
                                    int categoryId = reader.IsDBNull(4) ? -1 : reader.GetInt32(4);
                                    int descriptionId = reader.IsDBNull(5) ? -1 : reader.GetInt32(5);

                                    // Retrieve category name and create a Category object
                                    string categoryName = categoryId != -1 ? GetCategoryNameById(categoryId) : null;
                                    Category category = new Category(categoryName);

                                    // Retrieve description and create Expenses transaction object
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
                // Handle any exceptions that occur during the data retrieval process
                Console.WriteLine($"Error loading transactions from SQL Server database: {ex.Message}");
            }

            // Return the list of transactions loaded from the database
            return transactions;
        }

        /// <summary>
        /// Asynchronously loads all transactions from the database.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task LoadAllTransactionsFromDatabaseAsync()
        {
            // Initialize page number for pagination
            int pageNumber = 1;

            // Initialize list to store all transactions
            List<Transaction> allTransactions = new List<Transaction>();

            // Load transactions for each page until no more transactions are found
            while (true)
            {
                // Load transactions for the current page asynchronously
                List<Transaction> transactions = await LoadTransactionsFromDatabaseAsync(pageSize, pageNumber);

                // Break the loop if no transactions are loaded for the current page
                if (transactions.Count == 0)
                {
                    break;
                }

                // Add transactions to the list of all transactions
                allTransactions.AddRange(transactions);

                // Move to the next page for the next iteration
                pageNumber++;
            }

            // Add all transactions to the DataSource
            foreach (var transaction in allTransactions)
            {
                AddTransaction(transaction);
            }
        }

        /// <summary>
        /// Retrieves a value from the database by its ID.
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        /// <param name="idColumnName">The name of the ID column.</param>
        /// <param name="id">The ID value.</param>
        /// <param name="valueColumnName">The name of the value column.</param>
        /// <returns>The retrieved value.</returns>
        private string GetValueById(string tableName, string idColumnName, int id, string valueColumnName)
        {
            string value = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Construct the SQL command to retrieve the value based on the provided parameters
                    string commandText = $"SELECT {valueColumnName} FROM {tableName} WHERE {idColumnName} = @Id";

                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        // Add the ID parameter to the SQL command
                        command.Parameters.AddWithValue("@Id", id);

                        // Execute the SQL command and retrieve the result
                        object result = command.ExecuteScalar();

                        // Check if the result is not null
                        if (result != null)
                        {
                            // Convert the result to a string and assign it to the value variable
                            value = result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the database operation
                Console.WriteLine($"Error retrieving value from SQL Server database: {ex.Message}");
            }

            // Return the retrieved value
            return value;
        }

        /// <summary>
        /// Retrieves the category name from the database based on the provided category ID.
        /// </summary>
        /// <param name="categoryId">The ID of the category.</param>
        /// <returns>The name of the category.</returns>
        private string GetCategoryNameById(int categoryId)
        {
            // Call the GetValueById method to retrieve the category name from the Categories table
            return GetValueById("Categories", "CategoryID", categoryId, "Name");
        }

        /// <summary>
        /// Retrieves the description from the database based on the provided description ID.
        /// </summary>
        /// <param name="descriptionId">The ID of the description.</param>
        /// <returns>The description.</returns>
        private string GetDescriptionById(int descriptionId)
        {
            // Call the GetValueById method to retrieve the description from the Description table
            return GetValueById("Description", "Description_ID", descriptionId, "Name");
        }

        /// <summary>
        /// Adds a transaction to the list of transactions and saves it to the database if it's a new transaction.
        /// </summary>
        /// <param name="transaction">The transaction to add.</param>
        public void AddTransaction(Transaction transaction)
        {
            // If the transaction ID is 0, it means it's a new transaction that hasn't been saved to the database yet
            if (transaction.Id == 0)
            {
                // Save the transaction to the database
                SaveTransactionToDatabase(transaction);

                // Get the ID assigned to the transaction from the database
                int transactionId = GetTransactionIdFromDatabase(transaction);

                // Assign the retrieved ID to the transaction object
                transaction.Id = transactionId;
            }

            // Add the transaction to the list of transactions
            Transactions.Add(transaction);
        }

        /// <summary>
        /// Retrieves the ID of a transaction from the database based on its amount and date.
        /// </summary>
        /// <param name="transaction">The transaction to retrieve the ID for.</param>
        /// <returns>The ID of the transaction if found; otherwise, 0.</returns>
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
                        // Set parameters for the SQL query
                        command.Parameters.AddWithValue("@Amount", transaction.Amount);
                        command.Parameters.AddWithValue("@Date", transaction.Date);

                        // Execute the query and retrieve the result
                        object result = command.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            // Convert the result to an integer (transaction ID)
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

        /// <summary>
        /// Deletes a transaction from the database based on its ID.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to delete.</param>
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
                        // Set the parameter for the SQL query
                        command.Parameters.AddWithValue("@TransactionId", transactionId);

                        // Execute the SQL query to delete the transaction
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