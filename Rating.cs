//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ContractorShareService
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rating
    {
        public int ID { get; set; }
        public double rating1 { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
        public int FromUserID { get; set; }
        public int ToUserID { get; set; }
        public Nullable<int> ServiceID { get; set; }
    
        public virtual User User { get; set; }
        public virtual Service Service { get; set; }
        public virtual User User1 { get; set; }
    }
}