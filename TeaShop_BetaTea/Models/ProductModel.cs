using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeaShop_BetaTea.Models
{
    public class ProductModel
    {
        [Key]
        public int ProductId { get; set; }

        [Display(Name = "Изображение")]
        public string Image { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Display(Name = "Размер")]
        public string Size { get; set; }

        [Display(Name = "Тип")]
        public string Type { get; set; }
        [Display(Name = "Цвет")]
        public string Color { get; set; }
        [Display(Name = "Вес")]
        public int Weight { get; set; }
        [Display(Name = "Страна")]
        public string Country { get; set; }

        [Display(Name = "Цена")]
        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual CategoryModel Category { get; set; }
        public int BrandId { get; set; }

        [ForeignKey("BrandId")]
        public virtual BrandModel Brand { get; set; }

    }
}