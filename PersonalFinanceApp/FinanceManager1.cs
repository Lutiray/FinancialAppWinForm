/*using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace PersonalFinanceApp
{
    public class FinanceManager
    {
        public List<Transaction> Transactions { get; private set; }
        public List<Category> Categories { get; private set; }
        string filePath = "C:\\Users\\user\\source\\repos\\PersonalFinanceApp\\PersonalFinanceApp\\Categories.txt";

        public FinanceManager()
        {
            Transactions = new List<Transaction>();
            Categories = LoadCategoriesFromFile(filePath);
        }
        
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

                        Category existingCategory = categories.FirstOrDefault(c => c.Name == categoryName);

                        if (existingCategory != null)
                        {
                            for(int i = 0; i < parts.Length; i++)
                            {
                                string categoryDescription = parts[i].Trim();
                                existingCategory.AddDescription(categoryDescription);
                            }
                        }
                        
                        else
                        {
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

        public void AddTransaction (Transaction transaction)
        {
            Transactions.Add(transaction);
        }

        public List<Income> GetIncomes()
        {
            return Transactions.OfType<Income>().ToList();
        }

        public List<Outcome> GetOutcomes()
        {
            return Transactions.OfType<Outcome>().ToList();
        }
    }
}*/