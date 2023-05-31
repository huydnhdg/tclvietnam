using SALE_PRODUCT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SALE_PRODUCT.Areas.Admin.Controllers
{
    [Authorize]
    public abstract partial class BaseController : Controller
    {
        public SALE_PRODUCTEntities DB = null;
        public BaseController()
        {
            DB = new SALE_PRODUCTEntities();
        }
    }
}