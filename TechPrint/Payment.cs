//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TechPrint
{
    using System;
    using System.Collections.Generic;
    
    public partial class Payment
    {
        public long PaymentID { get; set; }
        public string PaymentNumber { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public Nullable<int> PaymentMode { get; set; }
        public Nullable<decimal> PaidAmount { get; set; }
        public string PaymentDetail { get; set; }
        public string FinYear { get; set; }
        public string RecMode { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> LMB { get; set; }
        public Nullable<System.DateTime> LMD { get; set; }
        public Nullable<int> DeleteBy { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
    }
}
