using SALE_PRODUCT.Areas.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SALE_PRODUCT.Controllers
{
    [RoutePrefix("bai-viet")]
    public class NewsController : DefaultController
    {
        [Route]
        public ActionResult Index()
        {
            var model = DB.News.Where(a => a.Status == true).OrderBy(a => a.Sort);
            return View(model.ToList());
        }

        [Route("{rewrite}")]
        public ActionResult Detail(string rewrite)
        {
            var model = DB.News.FirstOrDefault(a => a.Alt == rewrite);
            return View(model);
        }
    }
}