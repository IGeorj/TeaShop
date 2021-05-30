using System.Collections.Generic;

namespace TeaShop_BetaTea.Models
{
    public class ProductViewModel
    {
        public IEnumerable<ProductModel> Products { get; set; }
        public IEnumerable<ReviewModel> Reviews { get; set; }
        public FilterRepository Filters { get; set; }
        public ProductModel Product { get; set; }


    }
}