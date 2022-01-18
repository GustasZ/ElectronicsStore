using System.Collections.Generic;

namespace ElectronicsStore.Models
{
    public class UserViewModel
    {
        public ShoppingUser ShoppingUser { get; set; }
        public IEnumerable<string> CurrentRoles { get; set; }
        public IEnumerable<string> AvailableRoles { get; set; }
    }
}
