using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsStore.Models
{
    public class ShoppingUser : IdentityUser
    {
        public Cart Cart { get; set; }
    }
}
