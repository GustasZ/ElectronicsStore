using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsStore.Models
{
    public enum ItemLocation
    {
        [Display(Name = "2-3 days")]
        Store, // the item is in our store, it can be received in 2-3 days
        [Display(Name = "3-4 days")]
        LocalWarehouse, // the item is in a warehouse, but its in our country so it will be received in 3-4 days
        [Display(Name = "7-14 days")]
        EUWarehouse, // the item can be ordered from a warehouse inside the EU, it will take around 7-14 days to receive the item
        [Display(Name = "28+ days")]
        ForeignWarehouse, // the item can be order from a foreign warehouse outside the EU, it will take around 28+ days to receive the item
        [Display(Name = "Unknown")]
        Unavailable // the item is currently unavailable for us to ship
    }
}
