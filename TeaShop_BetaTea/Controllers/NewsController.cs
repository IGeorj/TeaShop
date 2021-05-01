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
        public async Task<ActionResult> Index(string SearchString)
        {
            List<NewsModel> model;
            using (DataContext db = new DataContext())
            {
                model = await db.News.ToListAsync();
                if (!String.IsNullOrEmpty(SearchString))
                {
                    model = await db.News
                        .Where(
                        x => x.Title.Contains(SearchString) ||
                        x.Topic.Contains(SearchString) ||
                        x.ShortDescription.Contains(SearchString))
                        .ToListAsync();
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