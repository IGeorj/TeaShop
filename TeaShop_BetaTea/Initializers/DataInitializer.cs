using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using TeaShop_BetaTea.Models;

namespace TeaShop_BetaTea.Initializers
{
    //public class DataInitializer : DropCreateDatabaseAlways<DataContext>
    //public class DataInitializer : DropCreateDatabaseIfModelChanges<DataContext>
    public class DataInitializer : CreateDatabaseIfNotExists<DataContext>
    {
        protected override void Seed(DataContext db)
        {
            NewsModel n2 = new NewsModel
            {
                Title = "Вторая новость",
                ShortDescription = "Второе описание",
                FullDescription = "Второе полное описание",
                Image = "~/Images/MissingImg.jpg",
                Date = DateTime.Now,
                Topic = "Блог"
            };
            NewsModel n1 = new NewsModel
            {
                Title = "Первая новость",
                ShortDescription = "Первое краткое описание",
                FullDescription = "Первое полное описание",
                Image = "~/Images/MissingImg.jpg",
                Date = DateTime.Now,
                Topic = "Акция"
            };
            db.News.Add(n1);
            db.News.Add(n2);
            db.SaveChanges();

            CategoryModel c1 = new CategoryModel
            {
                Name = "Чай",
                Description = "Описание категории чёрного чая"
            };
            CategoryModel c2 = new CategoryModel
            {
                Name = "Кофе",
                Description = "Описание категории кофе"
            };
            BrandModel br1 = new BrandModel
            {
                Name = "Beta Tea"
            };
            db.Categories.Add(c1);
            db.Categories.Add(c2);
            db.Brands.Add(br1);
            db.SaveChanges();

            ProductModel p1 = new ProductModel
            {
                Name = "Черный чай 1",
                Type = "Листовой",
                Weight = 250,
                Country = "Шри-Ланка",
                Color = "Черный чай",
                Size = "Крупный лист",
                Price = 2.0m,
                Description = "Очень интересное описание зеленого чая",
                Image = "~/Images/MissingImg.jpg",
                CategoryId = 1,
                BrandId = 1
            };
            ProductModel p2 = new ProductModel
            {
                Name = "Зеленый чай 1",
                Price = 2.5m,
                Type = "Листовой",
                Country = "Китай",
                Weight = 250,
                Color = "Зеленый чай",
                Size = "Средний лист",
                Description = "Очень интересное описание черного чая",
                Image = "~/Images/MissingImg.jpg",
                CategoryId = 1,
                BrandId = 1
            };
            ProductModel p3 = new ProductModel
            {
                Name = "Кофе 1",
                Type = "Растворимый",
                Weight = 800,
                Country = "Россия",
                Price = 0.5m,
                Description = "Очень интересное описание кофе",
                Image = "~/Images/MissingImg.jpg",
                CategoryId = 2,
                BrandId = 1
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
            RoleManager<IdentityRole> _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var role1 = new IdentityRole { Name = "Admin" };
            var role2 = new IdentityRole { Name = "User" };
            _roleManager.Create(role1);
            _roleManager.Create(role2);
            db.SaveChanges();
            var admin = new ApplicationUser { Email = "_Admin1@mail.ru", UserName = "Admin" };
            string password = "_Admin1";    
            ApplicationUserManager _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            var result = _userManager.Create(admin, password);
            if (result.Succeeded)
            {
                // добавляем для пользователя роль
                _userManager.AddToRole(admin.Id, role1.Name);
                _userManager.AddToRole(admin.Id, role2.Name);
            }
            db.SaveChanges();
        }
    }
}