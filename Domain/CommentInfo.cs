using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ContractorShareService.Domain
{
    public class CommentInfo
    {
        [DataMember]
        public int? JobId { get; set; }

        [DataMember]
        public int? TaskId { get; set; }

        [DataMember]
        public int CreatedByUserId { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public string Image { get; set; }

        [DataMember]
        public DateTime? Created { get; set; }
    }
}