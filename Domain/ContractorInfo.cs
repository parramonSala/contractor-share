using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ContractorShareService.Domain
{
    [DataContract]
    public class ContractorInfo
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string PostalCode { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public Nullable<long> PhoneNumber { get; set; }
        [DataMember]
        public Nullable<long> MobileNumber { get; set; }
        [DataMember]
        public string CompanyName { get; set; }
        [DataMember]
        public decimal CompanyCoordX { get; set; }
        [DataMember]
        public decimal CompanyCoordY { get; set; }
        [DataMember]
        public string website { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public List<int> Categories { get; set; }
        [DataMember]
        public double? PricePerHour { get; set; }
        [DataMember]
        public int? NumberOfRates { get; set; }
        [DataMember]
        public double? AverageRate { get; set; }
    }
}