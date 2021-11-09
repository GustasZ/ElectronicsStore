using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsStore.Models
{
    public class CategoryViewModel
    {
        public Category TopCategory { get; set; }
       // public List<Category> Categories { get; set; } = new List<Category>();
        public List<Category> SubCategories { get; set; } = new List<Category>();

 /*       public void AddCategories(List<Category> categories)
        {
            foreach (var category in categories)
                Categories.Add(category);
        }*/
        public void AddSubCategories(List<Category> categories)
        {
            foreach (var category in categories)
                SubCategories.Add(category);
        }
    }
}
