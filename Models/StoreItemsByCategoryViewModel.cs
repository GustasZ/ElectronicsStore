using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsStore.Models
{
    public class StoreItemsByCategoryViewModel
    {
        public List<StoreItem> StoreItems { get; set; } = new List<StoreItem>();
        public List<Category> CategoryPath { get; set; } = new List<Category>();
        public List<Category> ChildCategories { get; set; } = new List<Category>();
    }
}
