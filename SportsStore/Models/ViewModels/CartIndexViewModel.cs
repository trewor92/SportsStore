using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models.ViewModels
{
    public class CartIndexViewModel
    {
        public string ReturnUrl { get; set; }
        public Cart Cart { get; set; }
    }
}
