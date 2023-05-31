using SALE_PRODUCT.Areas.Admin.Controllers;
using SALE_PRODUCT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SALE_PRODUCT.Controllers
{
    public class HomeController : DefaultController
    {
        public ActionResult Index()
        {
            var model = new HomeModel();

            var banner = DB.Banners.Where(a => a.Status == true).Where(a => a.Type == "left");
            var right = DB.Banners.Where(a => a.Status == true).Where(a => a.Type == "right").Take(2);
            var product = DB.Products.Where(a => a.Status == true).OrderBy(a => a.Sort).Take(24);

            model.ListBanner = banner.OrderBy(a => a.Sort).ToList();
            model.RightBanner = right.ToList();
            model.ListProduct = product.ToList();
            return View(model);
        }
    }
}