using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
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
                ViewBag.StatusMessage = "Вы успешно оформили заказ, ждите звонка на мобильный телефон!";
            }
            var userId = User.Identity.GetUserId();

            ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            if (!string.IsNullOrEmpty(userId))
            {
                string phone = await userManager.GetPhoneNumberAsync(userId);
                ViewBag.Name = User.Identity.Name;
                ViewBag.Phone = " ";
                ViewBag.Street = " ";
                ViewBag.House = " ";
                ViewBag.Apartament = " ";
                if (!string.IsNullOrEmpty(phone))
                {
                    ViewBag.Phone = phone;
                }
                using(DataContext db = new DataContext())
                {
                    ApplicationUser user = db.Users.Find(userId);
                    if (!string.IsNullOrEmpty(user.Street))
                    {
                        ViewBag.Street = user.Street;
                        ViewBag.House = user.House;
                        ViewBag.Apartament = user.Apartament;
                    }
                }
            }
            else
            {
                ViewBag.Phone = " ";
            }
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
                    items.Add(new CartItemModel { Product = db.Products.Find(id), ProductId = id, Quantity = 1 });
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
                        items.Add(new CartItemModel { Product = product, ProductId = id, Quantity = 1 });
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
        public ActionResult CreateOrder(string inputPhone, string inputName, string inputStreet, string inputHouse, string inputApartment, string payment)
        {
            using (DataContext db = new DataContext())
            {
            OrderModel order = new OrderModel();
            if (User.Identity.IsAuthenticated)
            {
                order.UserId = User.Identity.GetUserId();
            }
            order.TotalPrice = (decimal)Session["cartTotal"];
            order.FirstName = inputName;
            order.Street = inputStreet;
            order.House = inputHouse;
            order.Apartament = inputApartment;
            order.Payment = payment;
            order.Phone = inputPhone;
            order.Date = DateTime.Now;
            order.Status = "В обработке";
                db.Orders.Add(order);
                db.SaveChanges();
                List<CartItemModel> items = (List<CartItemModel>)Session["cart"];
                for (int i = 0; i < items.Count; i++)
                {
                    items[i].OrderId = order.OrderId;
                    db.CartItems.Add(new CartItemModel { OrderId = order.OrderId, ProductId = items[i].ProductId, Quantity = items[i].Quantity});
                }
                db.SaveChanges();
            }
            Session["cart"] = null;
            return RedirectToAction("Index", new { orderCreated = "Yes" });
        }
    }
}