using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsStore.Models
{
    public class StoreItemViewModel
    {
        public StoreItem StoreItem { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public int SelectedCategory { get; set; }
        public IFormFile FormFile { get; set; }
    }
}
