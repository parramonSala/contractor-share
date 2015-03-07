using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ContractorShareService.Domain
{
    public class SearchService
    {
        [DataMember]
        public int CategoryId { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string PostCode { get; set; }
    }
}