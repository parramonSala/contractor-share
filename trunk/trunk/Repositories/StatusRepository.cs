using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using ContractorShareService.Enumerations;
using ContractorShareService.Domain;

namespace ContractorShareService.Repositories
{
    public class StatusRepository
    {

        protected static ILog Logger = LogManager.GetLogger(typeof(StatusRepository));
        private ContractorShareEntities db = new ContractorShareEntities();

        public int GetStatusId(string StatusDesc)
        {
            try
            {
                var id = from status in db.Status
                         where status.Description == StatusDesc
                         select status.ID;

                int statusid = id.First();
                return statusid;
                
            }
            catch (Exception ex)
            {
                Logger.Error("Error StatusRepository.GetStatusId", ex);
                return (int)ErrorListEnum.Status_Error;
            }
        }

    }
}