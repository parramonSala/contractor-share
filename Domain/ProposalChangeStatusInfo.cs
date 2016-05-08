using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ContractorShareService.Domain
{
    public class UpdateProposalStatusInfo
    {
        [DataMember]
        public int StatusId { get; set; }

        [DataMember]
        public int UpdatedByUserId { get; set; }
    }
}