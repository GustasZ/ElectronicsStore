using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsStore.Models
{
    public interface IStoreItem
    {
        int Id { get; set; }
        string ProductCode { get; set; }
        string Name { get; set; }
        int WarrantyInMonths { get; set; }
        int AvailableQuantity { get; set; } //even if the item is digital, you can only have finite number of keys for sale
        int Price { get; set; }
        ItemLocation itemLocation { get; set; }
        string ImagePath { get; set; }
        string Description { get; set; }
    }
}
