using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TeaShop_BetaTea.Models;

namespace TeaShop_BetaTea.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public async Task<ActionResult> Index(string orderCreated)
        {
            if (string.IsNullOrEmpty(orderCreated))
            {
                ViewBag.StatusMessage = "";
            }
            else
            {
                ViewBag.StatusMessage = "Вы успешно оформили заказ!";
            }
            var userId = User.Identity.GetUserId();

            ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ViewBag.Phone = await userManager.GetPhoneNumberAsync(userId);
            return View();
        }

        public ActionResult AddToCart(int id)
        {
            string temp2 = Session["asd"] as string;
            List<CartItemModel> items = (List<CartItemModel>)Session["cart"];
            if (items == null)
            {
                items = new List<CartItemModel>();
                using (DataContext db = new DataContext())
                {
                    items.Add(new CartItemModel { Product = db.Products.Find(id), Quantity = 1 });
                }
                Session["asd"] = "yes2";
                Session["cart"] = items;
                Session["cartTotal"] = items[0].Product.Price;
            }
            else
            {
                if (ItemInCart(id))
                {
                    IncreaseQuantity(id);
                }
                else
                {
                    using (DataContext db = new DataContext())
                    {
                        ProductModel product = db.Products.Find(id);
                        items.Add(new CartItemModel { Product = product, Quantity = 1 });
                        Session["cartTotal"] = (decimal)Session["cartTotal"] + product.Price;
                    }
                    Session["cart"] = items;
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Increase(int id)
        {
            IncreaseQuantity(id);
            return RedirectToAction("Index");
        }

        public ActionResult Decrease(int id)
        {
            List<CartItemModel> items = (List<CartItemModel>)Session["cart"];
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Product.ProductId == id && items[i].Quantity > 1)
                {
                    items[i].Quantity--;
                    Session["cartTotal"] = (decimal)Session["cartTotal"] - items[i].Product.Price;
                    break;
                }
            }
            Session["cart"] = items;
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            List<CartItemModel> items = (List<CartItemModel>)Session["cart"];
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Product.ProductId == id)
                {
                    Session["cartTotal"] = (decimal)Session["cartTotal"] - (items[i].Product.Price * items[i].Quantity);
                    items.RemoveAt(i);
                    break;
                }
            }
            if (items.Count == 0)
            {
                Session["cart"] = null;
            }
            else
            {
                Session["cart"] = items;
            }
            return RedirectToAction("Index");
        }

        public void IncreaseQuantity(int id)
        {
            List<CartItemModel> items = (List<CartItemModel>)Session["cart"];
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Product.ProductId == id)
                {
                    items[i].Quantity++;
                    Session["cartTotal"] = (decimal)Session["cartTotal"] + items[i].Product.Price;
                    break;
                }
            }
            Session["Сart"] = items;
        }

        private bool ItemInCart(int id)
        {
            List<CartItemModel> items = (List<CartItemModel>)Session["cart"];
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Product.ProductId == id)
                    return true;
            }
            return false;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateOrder()
        {
            Session["cart"] = null;
            return RedirectToAction("Index", new { orderCreated = "Yes"});
        }
    }
}