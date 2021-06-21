using Microsoft.AspNet.Identity;
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
        private DataContext ctx = new DataContext();

        // GET: Catalog
        [HttpGet]
        public async Task<ActionResult> Index(string category, string type, string searchString, string sortBy)
        {
            ViewBag.SortParm = sortBy;
            ViewBag.SearchParm = searchString;
            ViewBag.Category = category;
            ViewBag.Type = type;
            ProductViewModel model = new ProductViewModel();
            model.Filters = new FilterRepository();
            using (DataContext db = new DataContext())
            {
                model.Products = await db.Products.Include("Category").OrderByDescending(x => x.ProductId).ToListAsync();
                if (!String.IsNullOrEmpty(category))
                {
                    if (!String.IsNullOrEmpty(type))
                    {
                        if (category == "Чай")
                        {
                            model.Products = model.Products.Where(x => x.Category.Name == category && x.Color == type).ToList();
                        }
                        else
                        {
                            model.Products = model.Products.Where(x => x.Category.Name == category && x.Type == type).ToList();
                        }
                    }
                    else
                    {
                        model.Products = model.Products.Where(x => x.Category.Name == category).ToList();
                    }
                }
                if (!String.IsNullOrEmpty(searchString))
                {
                    model.Products = model.Products.Where(
                        x => x.Name.ToLower().Contains(searchString.ToLower()
                        ))
                        .ToList();
                }
                switch (sortBy)
                {
                    default:
                        break;

                    case "PriceAsc":
                        model.Products = model.Products.OrderByDescending(x => x.Price).ToList();
                        break;

                    case "PriceDsc":
                        model.Products = model.Products.OrderBy(x => x.Price).ToList();
                        break;

                    case "NameAsc":
                        model.Products = model.Products.OrderByDescending(x => x.Name).ToList();
                        break;

                    case "NameDsc":
                        model.Products = model.Products.OrderBy(x => x.Name).ToList();
                        break;

                    case "DateAsc":
                        model.Products = model.Products.OrderBy(x => x.ProductId).ToList();
                        break;

                    case "DateDsc":
                        model.Products = model.Products.OrderByDescending(x => x.ProductId).ToList();
                        break;
                }
                return View(model);
            }
        }

        public ActionResult ProductDetails(int id)
        {
            ProductViewModel model = new ProductViewModel();
            model.Products = GetProductListById(id);
            model.Reviews = GetReviewListByProductId(id);
            string userId = User.Identity.GetUserId();
            using (DataContext db = new DataContext())
            {
                LikeModel like = db.Likes.FirstOrDefault(x => x.ProductId == id && x.UserId == userId);
                if (like != null)
                {
                    model.Like = true;
                }
                else
                {
                    model.Like = false;
                }
            }
            return View(model);
        }

        private List<ReviewModel> GetReviewListByProductId(int id)
        {
            using (DataContext db = new DataContext())
            {
                return db.Reviews.Include(x => x.User).Where(x => x.ProductId == id).ToList();
            }
        }

        private List<ProductModel> GetProductListById(int id)
        {
            using (DataContext db = new DataContext())
            {
                return db.Products.Include("Category").Include("Brand").Where(x => x.ProductId == id).ToList();
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
                string userId = User.Identity.GetUserId();
                ReviewModel review = new ReviewModel
                {
                    UserId = userId,
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
            ProductViewModel model = new ProductViewModel();
            model.Filters = new FilterRepository();
            using (DataContext db = new DataContext())
            {
                model.Filters.BrandList = new SelectList(db.Brands.Select(x => x.Name).ToList());
            }
            SelectList сtg = new SelectList(ctx.Categories, "CategoryId", "Name");
            SelectList brd = new SelectList(ctx.Brands, "BrandId", "Name");
            ViewBag.Categories = сtg;
            ViewBag.Brands = brd;
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateProduct(HttpPostedFileBase Image, ProductViewModel model)
        {
            ProductViewModel temp = model;
            if (Image != null)
            {
                string path = Path.Combine(Server.MapPath("~/Images"), Path.GetFileName(Image.FileName));
                Image.SaveAs(path);
                temp.Product.Image = $"~/Images/{Image.FileName}";
            }
            else
            {
                temp.Product.Image = "~/Images/MissingImg.jpg";
            }
            using (DataContext db = new DataContext())
            {
                if (temp.Product.CategoryId == db.Categories.FirstOrDefault(x => x.Name == "Кофе").CategoryId)
                {
                    temp.Product.Color = null;
                    temp.Product.Size = null;
                }
                db.Products.Add(temp.Product);
                db.SaveChanges();
            }
            return RedirectToAction("AddProduct");
        }

        [ChildActionOnly]
        public ActionResult Stars(int ProductId)
        {
            List<ReviewModel> reviews = GetReviewListByProductId(ProductId);
            if (!reviews.Any())
            {
                return PartialView(-1);
            }
            int counter = 0;
            foreach (var item in reviews)
            {
                counter += item.Rate;
            }
            return PartialView((counter / reviews.Count));
        }

        [HttpPost]
        public ActionResult Like(int ProductId)
        {
            using (DataContext db = new DataContext())
            {
                LikeModel like = (LikeModel)db.Likes.FirstOrDefault(x => x.ProductId == ProductId);
                if (like != null)
                {
                    db.Likes.Remove(like);
                }
                else
                {
                    string userId = User.Identity.GetUserId();
                    db.Likes.Add(new LikeModel { UserId = userId, ProductId = ProductId });
                }
                db.SaveChanges();
            }
            return Redirect($"ProductDetails/{ProductId}");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DeleteProduct(int id)
        {
            using (DataContext db = new DataContext())
            {
                db.Products.Remove(db.Products.Find(id));
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DeleteReview(int rvid, int prid)
        {
            using (DataContext db = new DataContext())
            {
                db.Reviews.Remove(db.Reviews.Find(rvid));
                db.SaveChanges();
            }
            return Redirect($"ProductDetails/{prid}");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AddBrand()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddBrand(BrandModel brand)
        {
            using (DataContext db = new DataContext())
            {
                db.Brands.Add(new BrandModel { Name = brand.Name });
                db.SaveChanges();
            }
            return RedirectToAction("AddProduct");
        }
    }
}