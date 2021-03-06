﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ContractorShareService.Domain
{
    public class TaskInfo
    {
        [DataMember]
        public int TaskId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int ServiceId { get; set; }
        [DataMember]
        public int StatusId { get; set; }
        [DataMember]
        public DateTime? Created { get; set; }
        [DataMember]
        public string Image { get; set; }
    }
}