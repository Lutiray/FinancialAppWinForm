using System.Collections.Generic;

namespace PersonalFinanceApp
{
    public class Category
    {
        public string Name { get; private set; }
        public List<string> Descriptions { get; set; } = new List<string>();

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