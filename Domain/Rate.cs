using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ContractorShareService.Domain
{
    [DataContract]
    public class Rate
    {
        [DataMember]
        public int? RateId { get; set; }
        [DataMember]
        public int FromUserId { get; set; }
        [DataMember]
        public int ToUserId { get; set; }
        [DataMember]
        public int ServiceId { get; set; }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public float Rating { get; set; }
        [DataMember]
        public DateTime? Created { get; set; } 
    
    }
}