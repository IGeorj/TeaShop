using System.Data.Entity;
using TeaShop_BetaTea.Initializers;

namespace TeaShop_BetaTea.Models
{
    public class DataContext : DbContext
    {
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<NewsModel> News { get; set; }
        public DbSet<ReviewModel> Reviews { get; set; }
    }
}