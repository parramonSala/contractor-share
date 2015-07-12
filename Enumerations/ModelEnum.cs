﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContractorShareService.Enumerations
{
    public enum ModelEnum
    {
        Client = 1,
        Contractor = 2
    }

    public enum TaskStatusEnum
    {
        Open = 1,
        InProgress = 2,
        Closed = 3,
    }

    public enum ServiceStatusEnum
    {
        Open = 1,
        InProgress = 2,
        Closed = 3,
    }

}