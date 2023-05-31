using SALE_PRODUCT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SALE_PRODUCT.Areas.Admin.Controllers
{
    public abstract partial class DefaultController : Controller
    {
        public SALE_PRODUCTEntities DB = null;
        public DefaultController()
        {
            DB = new SALE_PRODUCTEntities();
        }
    }
}