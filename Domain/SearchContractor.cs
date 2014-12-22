using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ContractorShareService.Domain
{
    public class SearchContractor
    {
        [DataMember]
        public int CategoryId { get; set; }
        [DataMember]
        public decimal LocationCoordX { get; set; }
        [DataMember]
        public decimal LocationCoordY { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string CompanyName { get; set; }
        [DataMember]
        public double PricePerHour { get; set; }
        [DataMember]
        public int NumOfRates { get; set; }
        [DataMember]
        public double AverageRate { get; set; }
    }
}