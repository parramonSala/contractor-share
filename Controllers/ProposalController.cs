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

        public ProposalInfo GetProposal(int ProposalId)
        {
            try
            {
                string message = string.Format("Executing Get Proposal");
                Logger.Info(message);

                return _proposalRepository.GetProposal(ProposalId);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error Get Proposal");
                Logger.Error(error_message, ex);
                return null;
            }
        }

        public string EditProposal(int ProposalId, ProposalInfo proposalinfo)
        {
            try
            {
                string message = string.Format("Executing Edit Proposal");
                Logger.Info(message);

                return _proposalRepository.EditProposal(ProposalId, proposalinfo);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error Edit Proposal");
                Logger.Error(error_message, ex);
                return ex.ToString();
            }
        }

        public string ChangeProposalStatus(int ProposalId, int StatusId)
        {
            try
            {
                string message = string.Format("Executing ChangeProposalStatus");
                Logger.Info(message);

                return _proposalRepository.ChangeProposalStatus(ProposalId, StatusId);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error ChangeProposalStatus");
                Logger.Error(error_message, ex);
                return ex.ToString();
            }
        }

        public List<ProposalInfo> GetMyCreatedProposals(int FromUserId)
        {
            try
            {
                string message = string.Format("Executing GetMyCreatedProposals");
                Logger.Info(message);

                return _proposalRepository.GetMyCreatedProposals(FromUserId);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error GetMyCreatedProposals");
                Logger.Error(error_message, ex);
                return null;
            }
        }

        public List<ProposalInfo> GetMyReceivedProposals(int ToUserId)
        {
            try
            {
                string message = string.Format("Executing GetMyReceivedProposals");
                Logger.Info(message);

                return _proposalRepository.GetMyReceivedProposals(ToUserId);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error GetMyReceivedProposals");
                Logger.Error(error_message, ex);
                return null;
            }
        }




    }
}