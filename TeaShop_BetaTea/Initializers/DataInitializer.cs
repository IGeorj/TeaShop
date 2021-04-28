using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TeaShop_BetaTea.Models;

namespace TeaShop_BetaTea.Initializers
{
    public class DataInitializer : DropCreateDatabaseAlways<DataContext>
    {
        protected override void Seed(DataContext db)
        {
            NewsModel n1 = new NewsModel
            {
                Title = "Первая новость",
                ShortDescription = "Первое краткое описание",
                FullDescription = "Первое полное описание",
                Image = "~/Images/MissingImg.jpg",
                Date = DateTime.Now,
                Topic = "Акция"
            };
            NewsModel n2 = new NewsModel
            {
                Title = "Вторая новость",
                ShortDescription = "Второе описание",
                FullDescription = "Второе полное описание",
                Image = "~/Images/MissingImg.jpg",
                Date = DateTime.Now,
                Topic = "Блог"
            };

            db.News.Add(n1);
            db.News.Add(n2);
            db.SaveChanges();
        }
    }
}