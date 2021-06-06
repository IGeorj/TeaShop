using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TeaShop_BetaTea.Models
{
    public class LikeModel
    {
        [Key]
        public int LikeId { get; set; }

        [ForeignKey("ProductId")]
        public ProductModel Product { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        public string UserId { get; set; }
    }
}