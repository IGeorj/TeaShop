using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeaShop_BetaTea.Models
{
    public class OrderModel
    {
        [Key]
        public int OrderId { get; set; }
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Display(Name = "Телефон")]
        public string Phone { get; set; }

        [Display(Name = "Улица")]
        public string Street { get; set; }

        [Display(Name = "Дом")]
        public string House { get; set; }

        [Display(Name = "Квартира")]
        public string Apartament { get; set; }

        [Display(Name = "Способ оплаты")]
        public string Payment { get; set; }
        [Display(Name = "Стоимость заказа")]
        public decimal TotalPrice { get; set; }
        [Display(Name = "Дата")]

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime Date { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        public string UserId { get; set; }
    }
}