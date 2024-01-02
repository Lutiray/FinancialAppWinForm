using System.Collections.Generic;

namespace PersonalFinanceApp
{
    public class Category
    {
        public string Name { get; set; }
        public List<string> Descriptions { get; } = new List<string>();

        public Category(string name)
        {
            Name = name;
        }

        public void AddDescription(string description)
        {
            Descriptions.Add(description);
        }
    }
}