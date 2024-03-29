﻿using System;
using System.ComponentModel.DataAnnotations;

namespace TeaShop_BetaTea.Models
{
    public class NewsModel
    {
        [Key]
        public int NewsId { get; set; }

        [Display(Name = "Заголовок")]
        public string Title { get; set; }

        [Display(Name = "Топик")]
        public string Topic { get; set; }

        [Display(Name = "Изображение")]
        public string Image { get; set; }

        [Display(Name = "Краткое описание")]
        public string ShortDescription { get; set; }

        [Display(Name = "Полное описание")]
        public string FullDescription { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime Date { get; set; }
    }
}