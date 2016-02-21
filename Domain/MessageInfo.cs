using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ContractorShareService.Domain
{
    public class MessageInfo
    {
        [DataMember]
        public int ProposalId { get; set; }

        [DataMember]
        public int FromUserId { get; set; }

        [DataMember]
        public int ToUserId { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public DateTime? Created { get; set; }
    }
}