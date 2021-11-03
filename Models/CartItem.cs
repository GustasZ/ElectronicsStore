using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsStore.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public DateTime DateCreated { get; set; }

        public int StoreItemId { get; set; }
        public StoreItem StoreItem { get; set; }

        public int CartId { get; set; }
        public Cart Cart { get; set; }

        public string GetPrice()
        {
        //    var totalPrice = StoreItem.Price * Quantity;
            return $"{StoreItem.Price / 100}.{StoreItem.Price % 100}€"; // displaying 12345 as 123.45€
        }
    }
}
