using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeaShop_BetaTea.Models;

namespace TeaShop_BetaTea.Controllers
{
    public class NewsController : Controller
    {
        // GET: News
        public ActionResult Index()
        {
            List<NewsModel> model = new List<NewsModel>();
            using (DataContext db = new DataContext())
            {
                model = db.News.ToList();
            }
            return View(model);
        }
    }
}