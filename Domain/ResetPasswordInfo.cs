using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ContractorShareService.Domain
{
    [DataContract]
    public class ResetPasswordInfo
    {
        [DataMember(IsRequired = true)]
        public string Email { get; set; }
    }
}