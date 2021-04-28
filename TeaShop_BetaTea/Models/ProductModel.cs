using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TeaShop_BetaTea.Models
{
    public class ProductModel
    {
        [Key]
        public int ProductId { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public int ReviewId { get; set; }
        [ForeignKey("CategoryId")]
        public CategoryModel Category { get; set; }
        [ForeignKey("ReviewId")]
        public ReviewModel Review { get; set; }

    }
}