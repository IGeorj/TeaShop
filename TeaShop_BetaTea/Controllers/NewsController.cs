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
    [Authorize(Roles = "Admin")]
    public class NewsController : Controller
    {
        // GET: News
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> Index(string searchString)
        {
            List<NewsModel> model;
            using (DataContext db = new DataContext())
            {
                model = await db.News.OrderByDescending(x => x.Date).ToListAsync();
                if (!String.IsNullOrEmpty(searchString))
                {
                    searchString = searchString.ToLower();
                    model = model.Where(
                        x => x.Title.ToLower().Contains(searchString) ||
                        x.Topic.ToLower().Contains(searchString))
                        .ToList();
                }
            }
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            using (DataContext db = new DataContext())
            {
                db.News.Remove(db.News.Find(id));
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Create(HttpPostedFileBase Image, NewsModel news)
        {
            NewsModel temp = news;
            temp.Date = DateTime.Now;
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
                db.News.Add(temp);
                db.SaveChanges();
            }
            return RedirectToAction("AddNews");
        }

        public ActionResult AddNews()
        {
            return View();
        }
    }
}