using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ContractorShareService.Domain
{
    public class IssueInfo
    {
        [DataMember]
        public int IssueId { get; set; }
        
        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public DateTime? Created { get; set; }

        [DataMember]
        public int CreatedByUserId { get; set; }

    }
}