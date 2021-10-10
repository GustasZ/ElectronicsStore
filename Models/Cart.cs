using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsStore.Models
{
    public class Cart
    {
        public int Id { get; set; }

        public List<CartItem> CartItems { get; set; } = new List<CartItem>(); // ??????????

        public string ShoppingUserId { get; set; }
        public ShoppingUser ShoppingUser { get; set; }

        public int Total { get; set; }

        public string GetTotal()
        {
            int total = 0;
            foreach (var item in CartItems)
                total += item.StoreItem.Price * item.Quantity;
            Total = total;
            return $"{Total / 100}.{Total % 100}€"; // displaying 12345 as 123.45€
        }
    }
}
