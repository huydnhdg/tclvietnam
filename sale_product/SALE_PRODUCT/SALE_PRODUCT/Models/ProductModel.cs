using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SALE_PRODUCT.Models
{
    public class ProductModel
    {
        public List<Product> Products { get; set; }
        public List<Product_Cate> Cate { get; set; }
    }
}