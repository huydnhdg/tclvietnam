using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCL_Voucher.Models;

namespace TCL_Voucher.Areas.Admin.Data
{
    public class ContactView : Contact
    {
        public string image1 { get; set; }
        public string image2 { get; set; }
        public string image3 { get; set; }
    }
}