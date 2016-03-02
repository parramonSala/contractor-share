using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using ContractorShareService.Enumerations;

namespace ContractorShareService.Domain
{
    public class Result
    {
        [DataMember(IsRequired = true)]
        public string message { get; set; }

        [DataMember(IsRequired = true)]
        public int resultCode { get; set; }

        public Result()
        {
            message = "OK";
            resultCode = (int)ErrorListEnum.OK;
        }

        public Result(string Message, int code)
        {
            message = Message;
            resultCode = code;
        }

    }
}