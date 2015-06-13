using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ContractorShareService.Domain
{
    [DataContract]
    public class LoginInfo
    {
        [DataMember(IsRequired = true)]
        public string Email { get; set; }

        [DataMember(IsRequired = true)]
        public string Password { get; set; }

        [DataMember(IsRequired = true)]
        public int UserType { get; set; }
    }
}