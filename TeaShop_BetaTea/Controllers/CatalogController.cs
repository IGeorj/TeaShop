using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TeaShop_BetaTea.Models;

namespace TeaShop_BetaTea.Controllers
{
    public class CatalogController : Controller
    {
        // GET: Catalog
        public ActionResult Index()
        {
            List<ProductModel> model = new List<ProductModel>();
            using (DataContext db = new DataContext())
            {
                model = db.Products.Include("Category").Include("Review").ToList();
            }
            return View(model);
        }

        public ActionResult BlackTea()
        {
            return View(GetProductListByCategoryId(1));
        }

        public ActionResult GreenTea()
        {
            return View(GetProductListByCategoryId(2));
        }

        public ActionResult Coffee()
        {
            return View(GetProductListByCategoryId(3));
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
                int? reviewId = db.Products.Find(id).ReviewId;
                if(reviewId == null)
                {
                    return new List<ReviewModel>();
                }
                else
                {
                    return db.Reviews.Where(x => x.ReviewId == (int)reviewId).ToList();
                }
            }
        }
        private List<ProductModel> GetProductListById(int id)
        {
            using (DataContext db = new DataContext())
            {
                return db.Products.Include("Category").Include("Review").Where(x => x.ProductId == id).ToList();
            }
        }
        private List<ProductModel> GetProductListByCategoryId(int id)
        {
            using (DataContext db = new DataContext())
            {
                return db.Products.Include("Category").Include("Review").Where(x => x.CategoryId == id).ToList();
            }
        }
    }
}