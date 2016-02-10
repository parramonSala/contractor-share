using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ContractorShareService.Domain
{
    [DataContract]
    public class PreferencesResult
    {
        [DataMember]
        public bool? shareLocation { get; set; }

        [DataMember]
        public bool? showContactNumber { get; set; }

        [DataMember]
        public bool? showContactEmail { get; set; }

        [DataMember]
        public bool? enableNotifications { get; set; }
    }
}