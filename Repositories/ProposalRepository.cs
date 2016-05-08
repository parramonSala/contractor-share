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
                    Created = DateTime.Now,
                    UpdatedByUserID = proposal.FromUserId
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
                proposalinfo.UpdatedByUserId = selectedproposal.UpdatedByUserID;

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

        public string UpdateProposalStatus(int proposalId, int statusId, int? updatedByUserId)
        {
            try
            {
                var selectedproposal = (from proposal in db.Proposals
                                        where proposal.ID == proposalId
                                        select proposal).FirstOrDefault();

                if (selectedproposal.StatusID != statusId)
                {
                    selectedproposal.StatusID = statusId;

                    if (updatedByUserId.HasValue) selectedproposal.UpdatedByUserID = updatedByUserId;

                    if ((statusId == (int)ProposalStatusEnum.Accepted))
                    {
                          //Update job to In progress
                          selectedproposal.Service.StatusID = (int)ServiceStatusEnum.InProgress;

                          // Client is the only one who can accept, so updateByuserId will always be the clientId. We need the contractorId.
                          selectedproposal.Service.ContractorID = updatedByUserId == selectedproposal.FromUserID ? selectedproposal.ToUserID : selectedproposal.FromUserID;                        

                          //Reject all other proposals
                          var otherProposals = (from proposal in db.Proposals
                                               where proposal.ID != selectedproposal.ID && proposal.ServiceID ==selectedproposal.ServiceID
                                              select proposal).ToList();

                          foreach (var proposal in otherProposals)
                          {
                              proposal.StatusID = (int)ProposalStatusEnum.Rejected;
                          }
                    }

                    if ((statusId == (int)ProposalStatusEnum.Rejected || statusId == (int)ProposalStatusEnum.Cancelled) && selectedproposal.Active)
                    {
                        selectedproposal.Active = false;
                    }

                    db.SaveChanges();
                }

                return EnumHelper.GetDescription(ErrorListEnum.OK);
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error ProposalRepository.ChangeProposalStatus {0}: {1}", proposalId.ToString(), ex);
                return ex.ToString();
            }
        }

        public List<ProposalInfo> GetActiveProposals(int UserId, bool includeFromMe)
        {
            try
            {
                var proposals = from proposal in db.Proposals
                                join u in db.Users on proposal.FromUserID equals u.ID
                                join j in db.Services on proposal.ServiceID equals j.ID
                                where (proposal.FromUserID == UserId || proposal.ToUserID == UserId)
                                && proposal.Active == true
                                select new { proposal, u.Email, j.Name};
                                //select proposal;

                List<ProposalInfo> proposalinfolist = new List<ProposalInfo>();

                foreach (var selectedproposal in proposals)
                {
                    if (includeFromMe || (includeFromMe == false && selectedproposal.proposal.ToUserID == UserId))
                    {
                        ProposalInfo proposalinfo = new ProposalInfo();
                        proposalinfo.ProposalId = selectedproposal.proposal.ID;
                        proposalinfo.JobId = selectedproposal.proposal.ServiceID;
                        proposalinfo.JobName = selectedproposal.Name;
                        proposalinfo.FromUserId = selectedproposal.proposal.FromUserID;
                        proposalinfo.FromUsername = selectedproposal.Email;
                        proposalinfo.ToUserId = selectedproposal.proposal.ToUserID;
                        proposalinfo.Message = selectedproposal.proposal.Message;
                        proposalinfo.StatusId = selectedproposal.proposal.StatusID;
                        proposalinfo.ProposedPrice = (decimal)selectedproposal.proposal.ProposedPrice;
                        proposalinfo.ProposedTime = selectedproposal.proposal.ProposedTime;
                        proposalinfo.Active = selectedproposal.proposal.Active;
                        proposalinfo.AproxDuration = selectedproposal.proposal.AproxDuration;
                        proposalinfo.Created = selectedproposal.proposal.Created;
                        proposalinfo.UpdatedByUserId = selectedproposal.proposal.UpdatedByUserID;

                        proposalinfolist.Add(proposalinfo);
                    }
                }

                return proposalinfolist;
            }
            catch (Exception ex)
            {
                Logger.Error("Error ProposalRepository.GetActiveProposals", ex);
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
                    proposalinfo.UpdatedByUserId = selectedproposal.UpdatedByUserID;

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

        public string ChangeProposalUpdatedByUser(int proposalId, int userId)
        {
            try
            {
                var proposals = from proposal in db.Proposals
                                where proposal.ID == proposalId
                                select proposal;

                Proposal selectedproposal = proposals.FirstOrDefault();

                selectedproposal.UpdatedByUserID = userId;

                db.SaveChanges();
                return EnumHelper.GetDescription(ErrorListEnum.OK);
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error ProposalRepository.ChangeProposalUpdatedByUser {0}: {1}", proposalId.ToString(), ex);
                return ex.ToString();
            }
        }

        public string CreateMessage(MessageInfo message)
        {
            try
            {
                Message newmessage = new Message()
                {
                    ProposalID = message.ProposalId,
                    FromUserID = message.FromUserId,
                    ToUserID = message.ToUserId,
                    Message1 = message.Message,
                    Created = DateTime.Now
                };

                db.Messages.Add(newmessage);
                db.SaveChanges();

                Logger.Info(String.Format("ProposalRepository.CreateMessage: created message"));

                return EnumHelper.GetDescription(ErrorListEnum.OK);
            }
            catch (Exception ex)
            {
                Logger.Error("Error ProposalRepository.CreateMessage", ex);
                return ex.ToString();
            }
        }

        public List<MessageInfo> GetProposalMessages(int proposalId)
        {
            try
            {
                var messages = from message in db.Messages
                               where message.ProposalID == proposalId
                               orderby message.ID descending
                               select message;

                List<MessageInfo> messagelist = new List<MessageInfo>();

                foreach (var selectedmessage in messages)
                {
                    MessageInfo messageinfo = new MessageInfo();

                    messageinfo.MessageId = selectedmessage.ID;
                    messageinfo.ProposalId = selectedmessage.ProposalID;
                    messageinfo.FromUserId = selectedmessage.FromUserID;
                    messageinfo.ToUserId = selectedmessage.ToUserID;
                    messageinfo.Created = selectedmessage.Created;
                    messageinfo.Message = selectedmessage.Message1;

                    messagelist.Add(messageinfo);
                }

                return messagelist;
            }
            catch (Exception ex)
            {
                Logger.Error("Error ProposalRepository.GetProposalMessages", ex);
                return null;
            }
        }

        public Result DeleteMessage(int MessageId)
        {
            try
            {
                Message proposalmessage = (from message in db.Messages
                                      where message.ID == MessageId
                                           select message).First();

                db.Messages.Remove(proposalmessage);
                db.SaveChanges();

                Logger.Info(String.Format("DeleteMessage"));

                return new Result();
            }
            catch (Exception ex)
            {
                Logger.Error("Error DeleteMessage", ex);
                return new Result(ex.ToString(), (int)(ErrorListEnum.DeleteMessage_Other_Error));
            }

        }

    }
}