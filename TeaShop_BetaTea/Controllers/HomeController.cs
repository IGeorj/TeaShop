using System.Linq;
using System.Web.Mvc;
using TeaShop_BetaTea.Models;

namespace TeaShop_BetaTea.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (DataContext db = new DataContext())
            {
                return View(db.Products.OrderByDescending(x => x.ProductId).Take(3).ToList());
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}