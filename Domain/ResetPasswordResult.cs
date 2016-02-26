using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ContractorShareService.Domain
{
    [DataContract]
    public class ResetPasswordResult
    {
        [DataMember(IsRequired = true)]
        public string Email { get; set; }

        [DataMember(IsRequired = true)]
        public string Result { get; set; }
    }
}