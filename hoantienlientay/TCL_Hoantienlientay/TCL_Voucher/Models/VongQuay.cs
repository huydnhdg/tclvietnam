//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TCL_Voucher.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class VongQuay
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string Address { get; set; }
        public System.DateTime Createdate { get; set; }
        public int Status { get; set; }
        public Nullable<System.DateTime> Buydate { get; set; }
        public string BuyAdr { get; set; }
        public string PRODUCT { get; set; }
        public string SERIAL { get; set; }
        public string MODEL { get; set; }
        public string IMEI { get; set; }
        public string INVOICE { get; set; }
        public string CMND_MT { get; set; }
        public string CMNT_MS { get; set; }
        public string SIZE { get; set; }
        public int PAYMENT { get; set; }
    }
}
