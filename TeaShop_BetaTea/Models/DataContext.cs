using System.Data.Entity;
using TeaShop_BetaTea.Initializers;

namespace TeaShop_BetaTea.Models
{
    public class DataContext : ApplicationDbContext
    {
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<NewsModel> News { get; set; }
        public DbSet<ReviewModel> Reviews { get; set; }
        public DbSet<CartItemModel> CartItems { get; set; }
        public DbSet<OrderModel> Orders { get; set; }

    }
}