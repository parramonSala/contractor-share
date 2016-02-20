using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ContractorShareService.Domain
{
    public class ProposalInfo
    {
        [DataMember]
        public int ProposalId { get; set; }

        [DataMember]
        public int JobId { get; set; }

        [DataMember]
        public int FromUserId { get; set; }

        [DataMember]
        public int ToUserId { get; set; }

        [DataMember]
        public int StatusId { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public DateTime? ProposedTime { get; set; }

        [DataMember]
        public bool Active { get; set; }

        [DataMember]
        public decimal? AproxDuration { get; set; }

        [DataMember]
        public decimal ProposedPrice { get; set; }

        [DataMember]
        public DateTime? Created { get; set; }

        [DataMember]
        public int? UpdatedByUserId { get; set; }
    }
}