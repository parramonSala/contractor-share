using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ContractorShareService.Domain
{
    public class AppointmentInfo
    {
        [DataMember]
        public int ProposalId { get; set; }

        [DataMember]
        public int JobId { get; set; }

        [DataMember]
        public int ClientId { get; set; }

        [DataMember]
        public int ContractorId { get; set; }

        [DataMember]
        public int StatusId { get; set; }

        [DataMember]
        public DateTime? MeetingTime { get; set; }

        [DataMember]
        public bool Active { get; set; }

        [DataMember]
        public decimal? AproxDuration { get; set; }

        [DataMember]
        public decimal? LocationCoordX { get; set; }

        [DataMember]
        public decimal? LocationCoordY { get; set; }
    }
}