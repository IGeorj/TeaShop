﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TeaShop_BetaTea.Models
{
    public class CategoryModel
    {
        [Key]
        public int CategoryId { get; set; }

        [Display(Name = "Категория")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        public List<ProductModel> Products { get; set; }
    }
}