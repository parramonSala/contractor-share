using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ContractorShareService.Domain
{
    public class EventInfo
    {
        [DataMember]
        public int EventId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public int? AppointmentId { get; set; }
        [DataMember]
        public DateTime? Start_Date { get; set; }
        [DataMember]
        public DateTime? End_Date { get; set; }
    }
}