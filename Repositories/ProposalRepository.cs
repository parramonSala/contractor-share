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
                    ProposedTime = (Nullable<System.DateTime>)proposal.ProposedTime,
                    AproxDuration = proposal.AproxDuration,
                    Created = DateTime.Now
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

        public ProposalInfo GetProposal(int ProposalId)
        {
            try
            {
                var proposals = from proposal in db.Proposals
                                where proposal.ID == ProposalId
                                select proposal;

                Proposal selectedproposal = proposals.FirstOrDefault();

                ProposalInfo proposalinfo = new ProposalInfo();

                proposalinfo.ProposalId = selectedproposal.ID;
                proposalinfo.JobId = selectedproposal.ServiceID;
                proposalinfo.FromUserId = selectedproposal.FromUserID;
                proposalinfo.ToUserId = selectedproposal.ToUserID;
                proposalinfo.Message = selectedproposal.Message;
                proposalinfo.StatusId = selectedproposal.StatusID;
                proposalinfo.ProposedPrice = (decimal)selectedproposal.ProposedPrice;
                proposalinfo.ProposedTime = selectedproposal.ProposedTime;
                proposalinfo.Active = selectedproposal.Active;
                proposalinfo.AproxDuration = selectedproposal.AproxDuration;
                proposalinfo.Created = selectedproposal.Created;

                return proposalinfo;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("ProposalRepository.GetProposal {0}: {1}", ProposalId.ToString(), ex);
                return null;
            }
        }

        public string EditProposal(int ProposalId, ProposalInfo proposalinfo)
        {
            try
            {
                var proposals = from proposal in db.Proposals
                                where proposal.ID == ProposalId
                                select proposal;

                Proposal selectedproposal = proposals.FirstOrDefault();

                selectedproposal.ServiceID = proposalinfo.JobId;
                selectedproposal.FromUserID = proposalinfo.FromUserId;
                selectedproposal.ToUserID = proposalinfo.ToUserId;

                selectedproposal.Message = proposalinfo.Message;
                selectedproposal.StatusID = proposalinfo.StatusId;
                selectedproposal.ProposedPrice = proposalinfo.ProposedPrice;
                selectedproposal.ProposedTime = proposalinfo.ProposedTime;
                selectedproposal.Active = proposalinfo.Active;
                selectedproposal.AproxDuration = proposalinfo.AproxDuration;

                db.SaveChanges();
                return EnumHelper.GetDescription(ErrorListEnum.OK);
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error ProposalRepository.EditProposal{0}: {1}", ProposalId.ToString(), ex);
                return ex.ToString();
            }
        }

        public string ChangeProposalStatus(int ProposalId, int StatusId)
        {
            try
            {
                var proposals = from proposal in db.Proposals
                                where proposal.ID == ProposalId
                                select proposal;

                Proposal selectedproposal = proposals.FirstOrDefault();

                selectedproposal.StatusID = StatusId;

                db.SaveChanges();
                return EnumHelper.GetDescription(ErrorListEnum.OK);
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error ProposalRepository.ChangeProposalStatus {0}: {1}", ProposalId.ToString(), ex);
                return ex.ToString();
            }
        }

        public List<ProposalInfo> GetMyCreatedProposals(int FromUserId)
        {
            try
            {
                var proposals = from proposal in db.Proposals
                                where proposal.FromUserID == FromUserId
                                && proposal.Active == true
                                select proposal;

                List<ProposalInfo> proposalinfolist = new List<ProposalInfo>();

                foreach (var selectedproposal in proposals)
                {
                    ProposalInfo proposalinfo = new ProposalInfo();

                    proposalinfo.ProposalId = selectedproposal.ID;
                    proposalinfo.JobId = selectedproposal.ServiceID;
                    proposalinfo.FromUserId = selectedproposal.FromUserID;
                    proposalinfo.ToUserId = selectedproposal.ToUserID;
                    proposalinfo.Message = selectedproposal.Message;
                    proposalinfo.StatusId = selectedproposal.StatusID;
                    proposalinfo.ProposedPrice = (decimal)selectedproposal.ProposedPrice;
                    proposalinfo.ProposedTime = selectedproposal.ProposedTime;
                    proposalinfo.Active = selectedproposal.Active;
                    proposalinfo.AproxDuration = selectedproposal.AproxDuration;
                    proposalinfo.Created = selectedproposal.Created;

                    proposalinfolist.Add(proposalinfo);
                }

                return proposalinfolist;
            }
            catch (Exception ex)
            {
                Logger.Error("Error ProposalRepository.GetMyCreatedProposals", ex);
                return null;
            }
        }

        public List<ProposalInfo> GetMyReceivedProposals(int ToUserId)
        {
            try
            {
                var proposals = from proposal in db.Proposals
                                where proposal.ToUserID == ToUserId
                                && proposal.Active == true
                                select proposal;

                List<ProposalInfo> proposalinfolist = new List<ProposalInfo>();

                foreach (var selectedproposal in proposals)
                {
                    ProposalInfo proposalinfo = new ProposalInfo();

                    proposalinfo.ProposalId = selectedproposal.ID;
                    proposalinfo.JobId = selectedproposal.ServiceID;
                    proposalinfo.FromUserId = selectedproposal.FromUserID;
                    proposalinfo.ToUserId = selectedproposal.ToUserID;
                    proposalinfo.Message = selectedproposal.Message;
                    proposalinfo.StatusId = selectedproposal.StatusID;
                    proposalinfo.ProposedPrice = (decimal)selectedproposal.ProposedPrice;
                    proposalinfo.ProposedTime = selectedproposal.ProposedTime;
                    proposalinfo.Active = selectedproposal.Active;
                    proposalinfo.AproxDuration = selectedproposal.AproxDuration;
                    proposalinfo.Created = selectedproposal.Created;

                    proposalinfolist.Add(proposalinfo);
                }

                return proposalinfolist;
            }
            catch (Exception ex)
            {
                Logger.Error("Error ProposalRepository.GetMyReceivedProposals", ex);
                return null;
            }
        }

        public List<ProposalInfo> GetMyClosedProposals(int userId)
        {
            try
            {
                var proposals = from proposal in db.Proposals
                                where (proposal.FromUserID == userId || proposal.ToUserID == userId)
                                && proposal.Active == false
                                select proposal;

                List<ProposalInfo> proposalinfolist = new List<ProposalInfo>();

                foreach (var selectedproposal in proposals)
                {
                    ProposalInfo proposalinfo = new ProposalInfo();

                    proposalinfo.ProposalId = selectedproposal.ID;
                    proposalinfo.JobId = selectedproposal.ServiceID;
                    proposalinfo.FromUserId = selectedproposal.FromUserID;
                    proposalinfo.ToUserId = selectedproposal.ToUserID;
                    proposalinfo.Message = selectedproposal.Message;
                    proposalinfo.StatusId = selectedproposal.StatusID;
                    proposalinfo.ProposedPrice = (decimal)selectedproposal.ProposedPrice;
                    proposalinfo.ProposedTime = selectedproposal.ProposedTime;
                    proposalinfo.Active = selectedproposal.Active;
                    proposalinfo.AproxDuration = selectedproposal.AproxDuration;
                    proposalinfo.Created = selectedproposal.Created;

                    proposalinfolist.Add(proposalinfo);
                }

                return proposalinfolist;
            }
            catch (Exception ex)
            {
                Logger.Error("Error ProposalRepository.GetMyClosedProposals", ex);
                return null;
            }
        }


    }
}