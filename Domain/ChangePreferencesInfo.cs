using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ContractorShareService.Domain
{
    [DataContract]
    public class ChangePreferencesInfo
    {
        public bool shareLocation { get; set; }

        public bool showContactNumber { get; set; }

        public bool showContactEmail { get; set; }

        public bool enableNotifications { get; set; }
    }
}