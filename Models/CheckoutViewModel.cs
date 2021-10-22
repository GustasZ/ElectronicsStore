using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsStore.Models
{
    public class CheckoutViewModel
    {
        public ShippingAddress ShippingAddress { get; set; }
        public Cart Cart { get; set; }
    }
}
