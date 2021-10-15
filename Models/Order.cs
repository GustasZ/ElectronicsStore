using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsStore.Models
{
    public class Order
    {
        public int Id { get; set; }
        public List<CartItem> CartItems { get; init; } = new List<CartItem>();
        public string ShoppingUserId { get; init; }
        public ShoppingUser ShoppingUser { get; init; }

        public int Total { get; init; }
        public OrderStatusEnum OrderStatus { get; set; }

    }
}
