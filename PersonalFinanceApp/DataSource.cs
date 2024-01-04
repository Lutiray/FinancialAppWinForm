using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace PersonalFinanceApp
{
    /// <summary>
    /// Class for storing data about transactions and categories
    /// </summary>
    public class DataSource
    {
        /// <summary>
        /// Get list of all transactions
        /// </summary>
        public List<Transaction> Transactions { get; private set; }

        /// <summary>
        /// Get list of all categories
        /// </summary>
        public List<Category> Categories { get; private set; }

        /// <summary>
        /// The path to file with categories
        /// </summary>
        readonly string filePath = "C:\\Users\\user\\source\\repos\\PersonalFinanceApp\\PersonalFinanceApp\\Categories.txt";

        /// <summary>
        /// Initialize new instance of DataSource class
        /// </summary>
        public DataSource()
        {
            Transactions = new List<Transaction>();
            Categories = LoadCategoriesFromFile(filePath);
        }

        /// <summary>
        /// Load categories from file and return list of them
        /// </summary>
        /// <param name="filePath">The path to the file with categories.</param>
        /// <returns>The list of dowloaded categories.</returns>
        private List<Category> LoadCategoriesFromFile(string filePath)
        {
            List<Category> categories = new List<Category>();

            try
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');

                    if (parts.Length >= 1)
                    {
                        string categoryName = parts[0].Trim();

                        // Search for existing category with the same name
                        Category existingCategory = categories.FirstOrDefault(c => c.Name == categoryName);

                        if (existingCategory != null)
                        {
                            // If category with the same name exists, add descriptions to it
                            for (int i = 0; i < parts.Length; i++)
                            {
                                string categoryDescription = parts[i].Trim();
                                existingCategory.AddDescription(categoryDescription);
                            }
                        }
                        
                        else
                        {
                            // If category with the same name doesn't exist, create new category
                            Category newCategory = new Category(categoryName);

                            for (int i = 0; i < parts.Length; i++)
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
                Console.WriteLine($"Chyba při stahování kategorií: {ex.Message}");
            }

            return categories;
        }

        /// <summary>
        /// Add new transaction to the list of transactions.
        /// </summary>
        /// <param name="transaction">Added transaction.</param>
        public void AddTransaction (Transaction transaction)
        {
            Transactions.Add(transaction);
        }
    }
}