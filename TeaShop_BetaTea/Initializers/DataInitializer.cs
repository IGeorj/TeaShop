using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TeaShop_BetaTea.Models;

namespace TeaShop_BetaTea.Initializers
{
    public class DataInitializer : DropCreateDatabaseIfModelChanges<DataContext>
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

            CategoryModel c1 = new CategoryModel
            {
                Name = "Чёрный чай",
                Description = "Описание категории чёрного чая"

            };
            CategoryModel c2 = new CategoryModel
            {
                Name = "Зелёный чай",
                Description = "Описание категории зелёного чая"

            };
            CategoryModel c3 = new CategoryModel
            {
                Name = "Кофе",
                Description = "Описание категории кофе"

            };
            db.Categories.Add(c1);
            db.Categories.Add(c2);
            db.Categories.Add(c3);
            db.SaveChanges();

            ProductModel p1 = new ProductModel
            {
                Name = "Черный чай 1",
                Price = 2.0m,
                Description = "Очень интересное описание зелёного чая",
                Image = "~/Images/MissingImg.jpg",
                CategoryId = 1
            };
            ProductModel p2 = new ProductModel
            {
                Name = "Зелёный чай 1",
                Price = 2.5m,
                Description = "Очень интересное описание чёрного чая",
                Image = "~/Images/MissingImg.jpg",
                CategoryId = 2
            };
            ProductModel p3 = new ProductModel
            {
                Name = "Кофе 1",
                Price = 0.5m,
                Description = "Очень интересное описание кофе",
                Image = "~/Images/MissingImg.jpg",
                CategoryId = 3
            };
            db.Products.Add(p1);
            db.Products.Add(p2);
            db.Products.Add(p3);
            db.SaveChanges();

            ReviewModel r1 = new ReviewModel
            {
                Username = "Тестовый пользователь",
                Description = "Чай хороший",
                Date = DateTime.Now,
                Rate = 5,
                ProductId = 1
            };
            ReviewModel r2 = new ReviewModel
            {
                Username = "Тестовый пользователь",
                Description = "Чай плохой",
                Date = DateTime.Now,
                Rate = 1,
                ProductId = 2
            };
            ReviewModel r3 = new ReviewModel
            {
                Username = "Тестовый пользователь",
                Description = "Чай нормальный",
                Date = DateTime.Now,
                Rate = 3,
                ProductId = 1
            };
            db.Reviews.Add(r1);
            db.Reviews.Add(r2);
            db.Reviews.Add(r3);
            db.SaveChanges();
        }
    }
}