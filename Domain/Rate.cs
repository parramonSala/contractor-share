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

        public int FromUserId { get; set; }
        [DataMember]
        public int ToUserId { get; set; }
        [DataMember]
        public int ServiceId { get; set; }
        [DataMember]
        public string RateComment { get; set; }
        [DataMember]
        public string RateTitle { get; set; }
        [DataMember]
        public float Rating { get; set; }
 
    }
}