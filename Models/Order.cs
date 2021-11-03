using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsStore.Models
{
    public class Order
    {
        public int Id { get; set; }
        public List<OrderItem> OrderItems { get; init; } = new List<OrderItem>();
        public string ShoppingUserId { get; init; }
        public ShoppingUser ShoppingUser { get; init; }

        public ShippingAddress ShippingAddress { get; set; }

        public int Total { get; init; }
        public OrderStatusEnum OrderStatus { get; set; }

    }
}
