using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Store.Models;
using System.Data.Entity;
using System.Threading.Tasks;
using System.IO;

namespace AsyncContollers.Controllers
{
    [Authorize]
    public class catalogController : Controller
    {
        StoreContext db = new StoreContext();

        public ActionResult add()
        {
            ViewBag.Title = "Добавление товара или категории";
            ViewBag.Categories = db.Category.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult addItem(Item item, HttpPostedFileBase file)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Content/img/items/";
            string ext = Path.GetExtension(file.FileName);
            db.Item.Add(item);
            db.SaveChanges();
            file.SaveAs(Path.Combine(path, item.Id.ToString() + ext));
            item.Image = "/Content/img/items/" + item.Id.ToString() + ext;
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("add");
        }
        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.Categories = db.Category.ToList();
            ViewBag.Items = db.Item.Include(a => a.Category);
            ViewBag.Title = "Каталог товаров";
            return View();
        }
        [AllowAnonymous]
        public ActionResult category(int ? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Category category = db.Category.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            ViewBag.Categories = db.Category.ToList();
            ViewBag.Items = db.Item.Include(a => a.Category).Where(a => a.CategoryId == id);
            ViewBag.Title = category.Name;
            return View("Index");
        }
        public ActionResult changeCategories()
        {
            ViewBag.Title = "Изменение категорий";
            ViewBag.Categories = db.Category.ToList();
            return View();
        }
        public ActionResult changeItem(int ? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Item item = db.Item.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.Title = "Изменение товара " + item.Name;
            ViewBag.Categories = db.Category.ToList();
            return View(item);
        }
        [HttpPost]
        public ActionResult changeItem(Item item, HttpPostedFileBase file)
        {
            if(file != null)
            {
                if (System.IO.File.Exists("~" + item.Image))
                {
                    System.IO.File.Delete("~" + item.Image);
                }
                string path = AppDomain.CurrentDomain.BaseDirectory + "Content/img/items/";
                string ext = Path.GetExtension(file.FileName);
                file.SaveAs(Path.Combine(path, item.Id.ToString() + ext));
                item.Image = "/Content/img/items/" + item.Id.ToString() + ext;
            }
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult removeItem(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Item item = db.Item.Find(id);
            if(item == null)
            {
                return HttpNotFound();
            }
            db.Item.Remove(item);
            if (System.IO.File.Exists("~" + item.Image))
            {
                System.IO.File.Delete("~" + item.Image);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult addCat(Category category)
        {
            db.Category.Add(category);
            db.SaveChanges();
            return RedirectToAction("changeCategories");
        }
        [HttpPost]
        public ActionResult changeCat(Category category)
        {
            
            return RedirectToAction("changeCategories");
        }
        [HttpPost]
        public ActionResult removeCat(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Category cat = db.Category.Find(id);
            if (cat == null)
            {
                return HttpNotFound();
            }
            db.Category.Remove(cat);
            db.Category.RemoveRange(db.Category.Where(a => a.ParentId == id));
            db.SaveChanges();
            return RedirectToAction("changeCategories");
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}