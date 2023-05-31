using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SALE_PRODUCT.Models
{
    public class HomeModel
    {
        public List<Banner> ListBanner { get; set; }
        public List<Banner> RightBanner { get; set; }

        public List<Product> ListProduct { get; set; }
    }
}