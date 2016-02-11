using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using ContractorShareService.Enumerations;
using ContractorShareService.Domain;

namespace ContractorShareService.Repositories
{
    public class ProposalRepository
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(ServiceRepository));
        private ContractorShareEntities db = new ContractorShareEntities();

        public string CreateProposal(ProposalInfo proposal)
        {
            try
            {
                int statusid = (int)ProposalStatusEnum.Open;

                Proposal newproposal = new Proposal()
                {
                    ServiceID = proposal.JobId,
                    FromUserID = proposal.FromUserId,
                    ToUserID = proposal.ToUserId,
                    Active = true,
                    StatusID = statusid,
                    Message = proposal.Message,
                    ProposedPrice = proposal.ProposedPrice,
                    ProposedTime = proposal.ProposedTime,
                    AproxDuration = proposal.AproxDuration
                };

                db.Proposals.Add(newproposal);
                db.SaveChanges();

                Logger.Info(String.Format("ProposalRepository.CreateProposal: created proposal"));

                return EnumHelper.GetDescription(ErrorListEnum.OK);
            }
            catch (Exception ex)
            {
                Logger.Error("Error ProposalRepository.CreateProposal", ex);
                return ex.ToString();
            }
        }

    }
}