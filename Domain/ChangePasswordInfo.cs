using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ContractorShareService.Domain
{
    [DataContract]
    public class ChangePasswordInfo
    {
        [DataMember(IsRequired = true)]
        public string email { get; set; }
            
        [DataMember(IsRequired = true)]
        public string OldPassword { get; set; }

        [DataMember(IsRequired = true)]
        public string NewPassword { get; set; }
    }
}