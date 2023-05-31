using SALE_PRODUCT.Areas.Admin.Controllers;
using SALE_PRODUCT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SALE_PRODUCT.Controllers
{
    [RoutePrefix("san-pham")]
    public class ProductController : DefaultController
    {
        [Route]
        public ActionResult Index(long? textcate)
        {
            var product = DB.Products.Where(a => a.Status == true);
            var cate = DB.Product_Cate.Where(a => a.Status == true).OrderBy(a => a.Sort);

            if (textcate > 0)
            {
                product = product.Where(a => a.Cate == textcate);
            }

            var model = new ProductModel()
            {
                Cate = cate.ToList(),
                Products = product.OrderBy(a => a.Sort).ToList()
            };
            return View(model);
        }
        [Route("{rewrite}")]
        public ActionResult Detail(string rewrite)
        {
            var model = DB.Products.FirstOrDefault(a => a.Alt == rewrite);
            var list = DB.ProductImages.Where(a => a.IDProduct == model.ID);
            ViewBag.ListImage = list.ToList();
            return View(model);
        }
    }

}