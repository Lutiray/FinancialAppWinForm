using System.Collections.Generic;

namespace PersonalFinanceApp
{
    /// <summary>
    /// The class that represents a category of transactions.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Get or set the name of the category.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Get or set the list of descriptions for the category.
        /// </summary>
        public List<string> Descriptions { get; set; } = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Category"/> class.
        /// </summary>
        /// <param name="name">Имя категории.</param>
        public Category(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Adds the description to the list of descriptions.
        /// </summary>
        /// <param name="description">Desription to add.</param>
        public void AddDescription(string description)
        {
            Descriptions.Add(description);
        }
    }
}