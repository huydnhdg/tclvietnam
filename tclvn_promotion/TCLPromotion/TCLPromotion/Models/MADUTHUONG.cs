//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TCLPromotion.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class MADUTHUONG
    {
        public long ID { get; set; }
        public string Code { get; set; }
        public int Type { get; set; }
        public bool Status { get; set; }
        public Nullable<System.DateTime> Activedate { get; set; }
        public string PhoneActive { get; set; }
        public string Payment { get; set; }
        public Nullable<long> IDCusNew { get; set; }
    }
}
