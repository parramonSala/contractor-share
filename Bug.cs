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
    
    public partial class Bug
    {
        public int ID { get; set; }
        public string BugText { get; set; }
        public int CreatedByUserID { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
    
        public virtual User User { get; set; }
    }
}
