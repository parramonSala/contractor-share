using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ContractorShareService.Domain
{
    [DataContract]
    public class LoginResult
    {
        [DataMember(IsRequired = true)]
        public int UserId { get; set; }

        [DataMember(IsRequired = true)]
        public int UserType { get; set; }

        [DataMember(IsRequired = true)]
        public string error { get; set; }

        public LoginResult(string errormessage)
        {
            UserId = -1;
            UserType = -1;
            error = errormessage;
        }

        public LoginResult()
        {
        }
            
    }
}