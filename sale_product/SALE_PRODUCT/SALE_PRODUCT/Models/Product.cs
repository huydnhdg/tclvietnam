//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SALE_PRODUCT.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public string Alt { get; set; }
        public string Link { get; set; }
        public string Shopee { get; set; }
        public string Lazada { get; set; }
        public string Model { get; set; }
        public string Size { get; set; }
        public int Price { get; set; }
        public int Discount { get; set; }
        public string Description { get; set; }
        public string Detail { get; set; }
        public Nullable<long> Cate { get; set; }
        public Nullable<System.DateTime> Createdate { get; set; }
        public bool Status { get; set; }
        public int Sort { get; set; }
        public string Thumnail { get; set; }
        public int Sale { get; set; }
        public string DMCL { get; set; }
        public string DMX { get; set; }
    }
}
