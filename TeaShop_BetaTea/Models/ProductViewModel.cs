using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeaShop_BetaTea.Models
{
    public class ProductViewModel
    {
        public IEnumerable<ProductModel> Products { get; set; }
        public IEnumerable<ReviewModel> Reviews { get; set; }
    }
}