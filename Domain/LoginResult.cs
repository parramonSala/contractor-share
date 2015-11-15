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
        public int UserId { get; set; }

        public int UserType { get; set; }

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