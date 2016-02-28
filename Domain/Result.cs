using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ContractorShareService.Domain
{
    public class Result
    {
        [DataMember(IsRequired = true)]
        public string message { get; set; }

        [DataMember(IsRequired = true)]
        public int resultCode { get; set; }
    }
}