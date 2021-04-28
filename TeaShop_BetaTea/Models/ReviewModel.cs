using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TeaShop_BetaTea.Models
{
    public class ReviewModel
    {
        [Key]
        public int ReviewId { get; set; }
        public string Username { get; set; }
        public int Rate { get; set; }
        public string Description { get; set; }
    }
}