using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsStore.Models
{
    public class StoreItemInfoViewModel
    {
        public StoreItem StoreItem { get; set; }
        public List<StoreItem> RelatedStoreItems { get; set; } = new List<StoreItem>();
    }
}
