using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContractorShareService.Enumerations
{
    public enum UserTypeEnum
    {
        client = 1,
        contractor = 2
    }

    public enum StatusEnum
    {
        open = 1,
        inProgress = 2,
        closed = 3,
    }

}