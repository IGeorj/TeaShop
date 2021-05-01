using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TeaShop_BetaTea.Models;

namespace TeaShop_BetaTea.Controllers
{
    public class CatalogController : Controller
    {
        // GET: Catalog
        public async Task<ActionResult> Index(string category)
        {
            List<ProductModel> model;
            using (DataContext db = new DataContext())
            {
                model = await db.Products.Include("Category").ToListAsync();
                if (!String.IsNullOrEmpty(category))
                {
                    model = await db.Products.Include("Category").Where(x => x.Category.Name == category).ToListAsync();
                }
                return View(model);
            }
        }
        public ActionResult ProductDetails(int id)
        {
            ProductViewModel model = new ProductViewModel();
            model.Products = GetProductListById(id);
            model.Reviews = GetReviewListByProductId(id);
            return View(model);
        }

        private List<ReviewModel> GetReviewListByProductId(int id)
        {
            using (DataContext db = new DataContext())
            {
                return db.Reviews.Where(x => x.ProductId == id).ToList();
            }
        }

        private List<ProductModel> GetProductListById(int id)
        {
            using (DataContext db = new DataContext())
            {
                return db.Products.Include("Category").Where(x => x.ProductId == id).ToList();
            }
        }

        private List<ProductModel> GetProductListByCategoryId(int id)
        {
            using (DataContext db = new DataContext())
            {
                return db.Products.Include("Category").Where(x => x.CategoryId == id).ToList();
            }
        }

        [HttpPost]
        public ActionResult SendReview(int ProductId, int UserRate, string UserName, string ReviewDescription)
        {
            using (DataContext db = new DataContext())
            {
                ReviewModel review = new ReviewModel
                {
                    Username = UserName,
                    Date = DateTime.Now,
                    Description = ReviewDescription,
                    Rate = UserRate,
                    ProductId = ProductId
                };
                db.Reviews.Add(review);
                db.SaveChanges();
            }
            return Redirect($"ProductDetails/{ProductId}");
        }
    }
}