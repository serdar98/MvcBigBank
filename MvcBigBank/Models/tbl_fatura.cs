//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MvcBigBank.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_fatura
    {
        public int faturaID { get; set; }
        public int aboneID { get; set; }
        public decimal guncelBorc { get; set; }
        public System.DateTime sonOdemeTarihi { get; set; }
    
        public virtual tbl_abone tbl_abone { get; set; }
    }
}