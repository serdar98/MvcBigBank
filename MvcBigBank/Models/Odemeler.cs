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
    
    public partial class Odemeler
    {
        public int odemeID { get; set; }
        public int musteriID { get; set; }
        public int firmaID { get; set; }
        public Nullable<decimal> tutar { get; set; }
        public int hesapID { get; set; }
        public Nullable<System.DateTime> odemeTarihi { get; set; }
    
        public virtual Firmalar Firmalar { get; set; }
        public virtual Hesap Hesap { get; set; }
        public virtual Musteri Musteri { get; set; }
    }
}
