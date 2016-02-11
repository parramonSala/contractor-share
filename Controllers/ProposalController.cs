using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using ContractorShareService.Repositories;
using ContractorShareService.Enumerations;
using ContractorShareService.Domain;

namespace ContractorShareService.Controllers
{
    public class ProposalController
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(ServiceController));
        private ProposalRepository _proposalRepository = new ProposalRepository();

        public string Create(ProposalInfo proposal)
        {
            try
            {
                string message = string.Format("Executing Create Proposal");
                Logger.Info(message);

                return _proposalRepository.CreateProposal(proposal);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error Creating Proposal");
                Logger.Error(error_message, ex);
                return ex.ToString();
            }

        }
    }
}