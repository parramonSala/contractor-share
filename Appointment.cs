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
    
    public partial class Appointment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Appointment()
        {
            this.Events = new HashSet<Event>();
        }
    
        public int ID { get; set; }
        public int ServiceID { get; set; }
        public int ClientID { get; set; }
        public int ContractorID { get; set; }
        public int StatusID { get; set; }
        public Nullable<System.DateTime> MeetingTime { get; set; }
        public bool Active { get; set; }
        public Nullable<decimal> Duration { get; set; }
        public Nullable<decimal> CoordX { get; set; }
        public Nullable<decimal> CoordY { get; set; }
        public Nullable<int> ProposalId { get; set; }
    
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
        public virtual Service Service { get; set; }
        public virtual Status Status { get; set; }
        public virtual Proposal Proposal { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Event> Events { get; set; }
    }
}
