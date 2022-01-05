using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsStore.Models
{
    public class StoreItem : IStoreItem
    {
        [Key]
        public int Id { get; set; }
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public int WarrantyInMonths { get; set; }
        public int AvailableQuantity { get; set; }
        public int Price { get; set; }
        [DisplayName("Shipping Time")]
        public ItemLocation itemLocation { get; set; }
        public Category Category { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public List<MoreInfo> MoreItemInfo { get; set; } = new List<MoreInfo>();
        public List<CartItem> CartItems { get; set; }

        public string GetPrice()
        {
            return $"{Price / 100}.{Price % 100}€"; // displaying 12345 as 123.45€
        }
        public string GetQuantity() // no need to show the exact quantity at all times to the user
        {
            if (AvailableQuantity > 5)
                return "5+";
            else
                return AvailableQuantity.ToString();
        }
    }
}
