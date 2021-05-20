using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TeaShop_BetaTea.Models;

namespace TeaShop_BetaTea.Controllers
{
    public class CatalogController : Controller
    {
        DataContext ctx = new DataContext();
        // GET: Catalog
        [HttpGet]
        public async Task<ActionResult> Index(string category, string searchString, string sortBy)
        {
            ViewBag.SortParm = sortBy;
            List<ProductModel> model;
            using (DataContext db = new DataContext())
            {
                model = await db.Products.Include("Category").OrderByDescending(x => x.ProductId).ToListAsync();
                if (!String.IsNullOrEmpty(category))
                {
                    model = model.Where(x => x.Category.Name == category).ToList();
                }
                if (!String.IsNullOrEmpty(searchString))
                {
                    model = model.Where(
                        x => x.Name.Contains(searchString) ||
                        x.Description.Contains(searchString))
                        .ToList();
                }
                switch (sortBy)
                {
                    default:
                        break;
                    case "PriceAsc":
                        model = model.OrderByDescending(x => x.Price).ToList();
                        break;
                    case "PriceDsc":
                        model = model.OrderBy(x => x.Price).ToList();
                        break;
                    case "NameAsc":
                        model = model.OrderByDescending(x => x.Name).ToList();
                        break;
                    case "NameDsc":
                        model = model.OrderBy(x => x.Name).ToList();
                        break;
                }
                TempData["currentModel"] = model;
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
        [Authorize(Roles = "Admin")]
        public ActionResult AddProduct()
        {
                SelectList сtg = new SelectList(ctx.Categories, "CategoryId", "Name");
                ViewBag.Categories = сtg;
            return View();
        }
        [HttpPost]
        public ActionResult CreateProduct(HttpPostedFileBase Image, ProductModel news)
        {
            ProductModel temp = news;
            if (Image != null)
            {
                string path = Path.Combine(Server.MapPath("~/Images"), Path.GetFileName(Image.FileName));
                Image.SaveAs(path);
                temp.Image = $"~/Images/{Image.FileName}";
            }
            else
            {
                temp.Image = "~/Images/MissingImg.jpg";
            }
            using (DataContext db = new DataContext())
            {
                db.Products.Add(temp);
                db.SaveChanges();
            }
            return RedirectToAction("AddProduct");
        }
    }
}