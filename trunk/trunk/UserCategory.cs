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
    
    public partial class UserCategory
    {
        public int UserID { get; set; }
        public int CategoryID { get; set; }
        public int ID { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual User User { get; set; }
    }
}
