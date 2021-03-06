﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ContractorShareService.Domain
{
    public class JobInfo
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int StatusID { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string PostalCode { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public decimal CoordX { get; set; }
        [DataMember]
        public decimal CoordY { get; set; }
        [DataMember]
        public int ClientID { get; set; }
        [DataMember]
        public int CategoryID { get; set; }
        [DataMember]
        public string PostedDate { get; set; }
        [DataMember]
        public int? ContractorID { get; set; }
        [DataMember]
        public decimal? TotalPrice { get; set; }
        [DataMember]
        public bool? Paid { get; set; }
        [DataMember]
        public int? PaidBy { get; set; }

    }
}