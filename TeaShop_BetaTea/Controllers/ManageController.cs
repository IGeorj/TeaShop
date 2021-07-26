using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
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
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            var userId = User.Identity.GetUserId();
            using (DataContext db = new DataContext())
            {
                ViewBag.OrdersCounter = db.Orders.Where(x => x.UserId == userId).Count();
                ViewBag.ReviewsCounter = db.Reviews.Where(x => x.UserId == userId).Count();
                ViewBag.FavoritesCounter = db.Likes.Where(x => x.UserId == userId).Count();
                ViewBag.Avatar = db.Users.Find(userId).Avatar;
                ViewBag.Name = User.Identity.Name;
                ApplicationUser user = db.Users.Find(userId);
                if (string.IsNullOrEmpty(user.Street) || string.IsNullOrEmpty(user.House) || string.IsNullOrEmpty(user.Apartament))
                {
                    ViewBag.Address = " ";
                }
                else
                {
                    ViewBag.Address = $"{user.Street}, " + $"{user.House}" + $"-{user.Apartament}";
                }
            }
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Ваш пароль изменен"
                : message == ManageMessageId.SetPasswordSuccess ? "Пароль задан"
                : message == ManageMessageId.SetTwoFactorSuccess ? "Настроен поставщик двухфакторной проверки подлинности"
                : message == ManageMessageId.Error ? "Произошла ошибка"
                : message == ManageMessageId.AddPhoneSuccess ? "Ваш номер телефона изменен"
                : message == ManageMessageId.RemovePhoneSuccess ? "Ваш номер телефона удален"
                : "";

            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Создание и отправка маркера
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Ваш код безопасности: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.Number, code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            ModelState.AddModelError("", "Не удалось привязать");
            return View(model);
            //return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Отправка SMS через поставщик SMS для проверки номера телефона
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // Это сообщение означает наличие ошибки; повторное отображение формы
            ModelState.AddModelError("", "Не удалось проверить телефон");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // Это сообщение означает наличие ошибки; повторное отображение формы
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "Внешнее имя входа удалено."
                : message == ManageMessageId.Error ? "Произошла ошибка."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Запрос перенаправления к внешнему поставщику входа для связывания имени входа текущего пользователя
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AllOrders()
        {
            List<string> statusList = new List<string>() { "Доставлен", "Отправлен", "В обработке", "Отменен" };
            using (DataContext db = new DataContext())
            {
                ViewBag.StatusList = new SelectList(statusList);
                var model = await db.Orders.OrderByDescending(x => x.Date).ToListAsync();
                return View(model);
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ChangeStatus(int orderId, string status)
        {
            using (DataContext db = new DataContext())
            {
                OrderModel order = db.Orders.Find(orderId);
                order.Status = status;
                db.SaveChanges();
            }
            return RedirectToAction("AllOrders");
        }

        public async Task<ActionResult> OrdersHistory()
        {
            using (DataContext db = new DataContext())
            {
                string userId = User.Identity.GetUserId();
                var model = await db.Orders.Where(x => x.UserId == userId).OrderByDescending(x => x.Date).ToListAsync();
                return View(model);
            }
        }

        public async Task<ActionResult> ReviewsHistory()
        {
            using (DataContext db = new DataContext())
            {
                string userId = User.Identity.GetUserId();
                var model = await db.Reviews.Where(x => x.UserId == userId).Include(x => x.Product).OrderByDescending(x => x.Date).ToListAsync();
                return View(model);
            }
        }

        public ActionResult Favorites()
        {
            using (DataContext db = new DataContext())
            {
                string userId = User.Identity.GetUserId();
                var likes = db.Likes.Where(x => x.UserId == userId).ToList();
                ProductViewModel model = new ProductViewModel();
                List<ProductModel> products = new List<ProductModel>();
                foreach (var item in likes)
                {
                    products.Add(db.Products.Find(item.ProductId));
                }
                model.Products = products;
                return View(model);
            }
        }

        public ActionResult ChangeAvatar(HttpPostedFileBase Image)
        {
            using (DataContext db = new DataContext())
            {
                string userId = User.Identity.GetUserId();
                ApplicationUser user = db.Users.Find(userId);
                if (Image != null)
                {
                    string path = Path.Combine(Server.MapPath("~/Images"), Path.GetFileName(Image.FileName));
                    Image.SaveAs(path);
                    user.Avatar = $"~/Images/{Image.FileName}";
                }
                else
                {
                    user.Avatar = "~/Images/MissingImg.jpg";
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public ActionResult ChangePersonalData()
        {
            string userId = User.Identity.GetUserId();
            ViewBag.Name = User.Identity.Name;
            using (DataContext db = new DataContext())
            {
                if (string.IsNullOrEmpty(db.Users.Find(userId).House))
                {
                    ViewBag.Street = " ";
                    ViewBag.House = " ";
                    ViewBag.Apartament = " ";
                }
                else
                {
                    ViewBag.Street = db.Users.Find(userId).Street;
                    ViewBag.House = db.Users.Find(userId).House;
                    ViewBag.Apartament = db.Users.Find(userId).Apartament;
                }
            }
            return View();
        }

        //
        // POST: /Manage/ChangePersonalData
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePersonalData(string Street, string House, string Apartament, string Name)
        {
            string userId = User.Identity.GetUserId();
            using (DataContext db = new DataContext())
            {
                ApplicationUser user = db.Users.Find(userId);
                user.UserName = Name;
                user.Street = Street;
                user.Apartament = Apartament;
                user.House = House;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult ProductsFromOrder(int orderId)
        {
            using (DataContext db = new DataContext())
            {
                var orders = db.CartItems.Where(x => x.OrderId == orderId).ToList();
                ProductViewModel model = new ProductViewModel();
                model.CartItems = new Dictionary<ProductModel, int>();
                foreach (var item in orders)
                {
                    model.CartItems.Add(db.Products.Find(item.ProductId), item.Quantity);
                }
                return PartialView(model);
            }
        }

        #region Вспомогательные приложения

        // Используется для защиты от XSRF-атак при добавлении внешних имен входа
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion Вспомогательные приложения
    }
}