using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsStore.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public DateTime DateCreated { get; set; }

        public StoreItem StoreItem { get; set; }

        public Order Order { get; set; }

    }
}
