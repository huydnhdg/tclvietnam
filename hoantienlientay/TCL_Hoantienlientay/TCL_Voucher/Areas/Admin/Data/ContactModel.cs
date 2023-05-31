using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCL_Voucher.Models;

namespace TCL_Voucher.Areas.Admin.Data
{
    public class ContactModel:Contact
    {
        public string Code { get; set; }
        public int? Giaithuong { get; set; }
    }
}