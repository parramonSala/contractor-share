using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ContractorShareService.Domain
{
    public class JobRateInfo
    {
        [DataMember]
        public int JobId { get; set; }
        [DataMember]
        public string JobName { get; set; }
        [DataMember]
        public int JobCategoryId { get; set; }
        [DataMember]
        public int? ContractorID { get; set; }
        [DataMember]
        public string ContractorName { get; set; }
        [DataMember]
        public DateTime? AppointmentDate { get; set; }
        [DataMember]
        public decimal? Price { get; set; }
        [DataMember]
        public double? AverageRate { get; set; }
    }
}