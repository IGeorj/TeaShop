using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeaShop_BetaTea.Models
{
    public class ReviewModel
    {
        [Key]
        public int ReviewId { get; set; }

        [Display(Name = "Пользователь")]
        public string Username { get; set; }

        [Display(Name = "Оценка")]
        public int Rate { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime Date { get; set; }

        [ForeignKey("ProductId")]
        public ProductModel Product { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        public string UserId { get; set; }
    }
}