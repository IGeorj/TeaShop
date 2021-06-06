using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TeaShop_BetaTea.Models
{
    public class BrandModel
    {
        [Key]
        public int BrandId { get; set; }

        [Display(Name = "Бренд")]
        public string Name { get; set; }

        public List<ProductModel> Products { get; set; }
    }
}