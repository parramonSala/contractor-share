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
        protected static ILog Logger = LogManager.GetLogger(typeof(ProposalController));
        private ProposalRepository _proposalRepository = new ProposalRepository();
        private AppointmentController _appointmentController = new AppointmentController();
        private ServiceController _serviceController = new ServiceController();

        public Result Create(ProposalInfo proposal)
        {

            return _proposalRepository.CreateProposal(proposal);
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

        public Result EditProposal(int ProposalId, ProposalInfo proposalinfo)
        {     
            string message = string.Format("Executing Edit Proposal");
            Logger.Info(message);

            return _proposalRepository.EditProposal(ProposalId, proposalinfo);
        }

        public Result UpdateProposalStatus(int proposalId, int statusId, int? userId)
        {
            string message = string.Format("Executing ChangeProposalStatus");
                Logger.Info(message);

                Result result = _proposalRepository.UpdateProposalStatus(proposalId, statusId, userId ?? null);

                if (statusId == (int)ProposalStatusEnum.Accepted)
                {
                    //Add appointment
                    result= CreateNewAppointment(proposalId, userId ?? null);
                }

                return result;
        }

        public List<ProposalInfo> GetActiveProposals(int ToUserId, bool includeFromMe)
        {
            try
            {
                string message = string.Format("Executing GetActiveProposals");
                Logger.Info(message);

                return _proposalRepository.GetActiveProposals(ToUserId, includeFromMe);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error GetActiveProposals");
                Logger.Error(error_message, ex);
                return null;
            }
        }

        public List<ProposalInfo> GetMyClosedProposals(int userId)
        {
            try
            {
                string message = string.Format("Executing GetMyClosedProposals");
                Logger.Info(message);

                return _proposalRepository.GetMyClosedProposals(userId);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error GetMyClosedProposals");
                Logger.Error(error_message, ex);
                return null;
            }
        }

        public Result SendProposalMessage(int proposalId, MessageInfo proposalmessageInfo)
        {
                string message = string.Format("Executing SendProposalMessage");
                Logger.Info(message);

                //1. Create New Proposal Message
                Result result =_proposalRepository.CreateMessage(proposalmessageInfo);

                if (result.message != "OK")
                {
                    return result;
                }

                //2. Update the UpdatedByUser field in the proposal
                result = ChangeProposalUpdatedByUser(proposalId, proposalmessageInfo.FromUserId);
                if (result.message != "OK")
                {
                    return result;
                }

                //3. Update the status of the proposal to Pending
                int pendingstatus = (int)ProposalStatusEnum.Pending;
                
                return UpdateProposalStatus(proposalId, pendingstatus, null);
            }            


        //public string AcceptProposal(int ProposalId, int userId)
        //{

        //    try
        //    {//1.Change Status of the proposal to Accepted
        //        int statusId = (int)ProposalStatusEnum.Accepted;

        //        string result = UpdateProposalStatus(ProposalId, statusId, );
        //        if (result != "OK")
        //        {
        //            return result;
        //        }

        //        //2.Update the UpdatedByUserId in the proposal to be the userId 
        //        result = ChangeProposalUpdatedByUser(ProposalId, userId);
        //        if (result != "OK")
        //        {
        //            return result;
        //        }

        //        //3.Create a new Appointment
        //        return CreateNewAppointment(ProposalId, userId);
        //    }
        //    catch (Exception ex)
        //    {
        //        string error_message = string.Format("Error AcceptProposal");
        //        Logger.Error(error_message, ex);
        //        return null;
        //    }
        //}


        public List<MessageInfo> GetProposalMessages(int proposalId)
        {
            try
            {
                string message = string.Format("Executing GetProposalMessages");
                Logger.Info(message);

                return _proposalRepository.GetProposalMessages(proposalId);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error GetProposalMessages");
                Logger.Error(error_message, ex);
                return null;
            }
        }

        public Result DeleteMessage(int MessageId)
        {
            string message = string.Format("Executing DeleteMessage");
            Logger.Info(message);

            return _proposalRepository.DeleteMessage(MessageId);
        }

        //Private function
        private Result ChangeProposalUpdatedByUser(int proposalId, int userId)
        {            
               string message = string.Format("Executing ChangeProposalUpdatedByUser");
                Logger.Info(message);

                return _proposalRepository.ChangeProposalUpdatedByUser(proposalId, userId);
           
        }
        
        private Result CreateNewAppointment(int proposalId, int? userId)
        {
            AppointmentInfo newappointment = new AppointmentInfo();
            newappointment.ProposalId = proposalId;

            var proposal = GetProposal(proposalId);
            var job = _serviceController.GetServiceInfo(proposal.JobId);

            newappointment.JobId = proposal.JobId;
            newappointment.MeetingTime = proposal.ProposedTime;
            newappointment.AproxDuration = proposal.AproxDuration;
            newappointment.LocationCoordX = job.CoordX;
            newappointment.LocationCoordY = job.CoordY;

            //userId is the client
            if (userId == proposal.FromUserId)
            {
                newappointment.ClientId = userId.Value;
                newappointment.ContractorId = proposal.ToUserId;
            }
            else
            {
                newappointment.ClientId = userId.Value;
                newappointment.ContractorId = proposal.FromUserId;
            }

            return _appointmentController.Create(newappointment);
        }


    }
}