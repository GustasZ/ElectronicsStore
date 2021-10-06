using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsStore.Models
{
    public class StoreItemViewModel
    {
        public StoreItem StoreItem { get; set; }
        public IFormFile FormFile { get; set; }
    }
}
