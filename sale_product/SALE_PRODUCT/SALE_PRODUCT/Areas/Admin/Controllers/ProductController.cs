using PagedList;
using SALE_PRODUCT.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SALE_PRODUCT.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        static string urlss = "";
        public ActionResult Index(int? page, string textsearch, string status, string chanel, string start_date, string end_date)
        {
            var model = from a in DB.Products
                        select a;
            if (!string.IsNullOrEmpty(textsearch))
            {
                model = model.Where(a => a.Title.Contains(textsearch));
                ViewBag.textsearch = textsearch;
            }
            if (!string.IsNullOrEmpty(start_date))
            {
                DateTime s = DateTime.ParseExact(start_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                model = model.Where(a => a.Createdate >= s);
                ViewBag.start_date = start_date;
            }
            if (!string.IsNullOrEmpty(end_date))
            {
                DateTime s = DateTime.ParseExact(end_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                s = s.AddDays(1);
                model = model.Where(a => a.Createdate <= s);
                ViewBag.end_date = end_date;
            }
            IEnumerable<Product> data = model as IEnumerable<Product>;
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(data.OrderByDescending(a => a.Createdate).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Create()
        {
            ViewBag.Category = DB.Product_Cate.ToList();
            return View();
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "")] Product product, HttpPostedFileBase[] ProductImage)
        {
            try
            {
                foreach (HttpPostedFileBase file in ProductImage)
                {
                    if (file != null)
                    {
                        string image = UPLOAD(file);
                        var pdImage = new ProductImage()
                        {
                            IDProduct = product.ID,
                            Image = image,
                            Alt = product.Alt,
                        };
                        DB.ProductImages.Add(pdImage);
                    }
                }

                product.Createdate = DateTime.Now;
                DB.Products.Add(product);
                DB.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception ex) { }
            ViewBag.Category = DB.Product_Cate.ToList();
            return View(product);

        }
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product news = DB.Products.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            ViewBag.Category = DB.Product_Cate.ToList();
            ViewBag.ProductImage = DB.ProductImages.Where(a => a.IDProduct == id).ToList();
            urlss = Request.Url.AbsoluteUri;
            return View(news);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "")] Product product, HttpPostedFileBase[] ProductImage)
        {
            try
            {
                foreach (HttpPostedFileBase file in ProductImage)
                {
                    if (file != null)
                    {
                        string image = UPLOAD(file);
                        var pdImage = new ProductImage()
                        {
                            IDProduct = product.ID,
                            Image = image,
                            Alt = product.Alt
                        };
                        DB.ProductImages.Add(pdImage);
                    }
                }

                DB.Entry(product).State = EntityState.Modified;
                DB.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex) { }
            ViewBag.Category = DB.Product_Cate.ToList();
            ViewBag.ProductImage = DB.ProductImages.Where(a => a.IDProduct == product.ID).ToList();
            return View(product);
        }

        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = DB.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Product product = DB.Products.Find(id);
            DB.Products.Remove(product);
            DB.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteImg(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var img = DB.ProductImages.Find(id);
            if (img == null)
            {
                return HttpNotFound();
            }
            DB.ProductImages.Remove(img);
            DB.SaveChanges();
            return Redirect(urlss);
        }

        private string UPLOAD(HttpPostedFileBase file)
        {
            var rand = new Random();
            string strrand = rand.Next(0, 999).ToString();
            var fileName = Path.GetFileName(file.FileName);
            var path = Path.Combine(Server.MapPath("~/Data/"), strrand + "-" + fileName);
            file.SaveAs(path);
            string link = "/Data/" + strrand + "-" + fileName;
            return link;
        }
    }
}