using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ComponentModel;
using System.Text;
using log4net;
using ContractorShareService.Controllers;
using ContractorShareService.Domain;
using System.ServiceModel.Activation;

namespace ContractorShareService
{
    [AspNetCompatibilityRequirements
    (RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class ContractorShare : IContractorShare
    {
        #region ContractorShare properties definition

        protected static ILog Logger = LogManager.GetLogger(typeof(ContractorShare));
        private UserController _userController = new UserController();
        private ServiceController _serviceController = new ServiceController();
        private RateController _rateController = new RateController();
        private TaskController _taskController = new TaskController();
        private ProposalController _proposalController = new ProposalController();
        private AppointmentController _appointmentController = new AppointmentController();

        #endregion

        #region Login operations

        //1.Login operations
        public AuthenticationResult Login(LoginInfo loginInfo)
        {
            return _userController.Login(loginInfo.Email, loginInfo.Password);
        }

        public AuthenticationResult Register(RegisterInfo registerinfo)
        {
            return _userController.Register(registerinfo.Email, registerinfo.Password, registerinfo.UserType);
        }

        public ResetPasswordResult ResetPassword(ResetPasswordInfo resetpasswordinfo)
        {
            return _userController.ResetPassword(resetpasswordinfo.Email);
        }

        public string ChangePassword(string userId, ChangePasswordInfo changepasswordinfo)
        {
            return _userController.ChangePassword(changepasswordinfo);
        }

        public string ChangePreferences(string userId, ChangePreferencesInfo changepreferencesinfo)
        {
            return _userController.ChangePreferences(userId,changepreferencesinfo);
        }

        public PreferencesResult GetPreferences(string userId)
        {
            return _userController.GetPreferences(userId);
        }

        #endregion

        #region Profile operations

        //2.Profile operations
        public string EditUserProfile(string UserId, UserInfo userprofile)
        {
            return _userController.EditUserProfile(userprofile);
        }

        public UserInfo GetUserProfile(string userId)
        {
            return _userController.GetUserProfile(Convert.ToInt32(userId));
        }

        public string AddFavourite(string userId, int ToUser)
        {
            return _userController.AddFavourite(Convert.ToInt32(userId), ToUser);
        }

        public string RemoveFavourite(string userId, string favouriteUserId)
        {
            return _userController.RemoveFavourite(Convert.ToInt32(userId), Convert.ToInt32(favouriteUserId));
        }

        public List<UserFavourite> GetUserFavourites(string userId)
        {
            return _userController.GetUserFavourites(Convert.ToInt32(userId));
        }

        public bool UserIsFavourite(string fromuserId, string userId)
        {
            return _userController.UserIsFavourite(Convert.ToInt32(fromuserId), Convert.ToInt32(userId));
        }

        public string AddDenunce(string userId, int ToUser, string Comment, bool BlockUser)
        {
            return _userController.AddDenunce(Convert.ToInt32(userId), ToUser, Comment, BlockUser);
        }

        public Result BlockUser(string userId, int ToUser)
        {
            return _userController.BlockUser(Convert.ToInt32(userId), ToUser);
        }

        public bool UserIsBlocked(string FromUser, string ToUser)
        {
            return _userController.UserIsBlocked(Convert.ToInt32(FromUser), Convert.ToInt32(ToUser));
        }

        #endregion

        #region Service request operations

        //3.Service request operations
        public string CreateServiceRequest(JobInfo servicerequest)
        {
            return _serviceController.Create(servicerequest);
        }

        public Result EditJob(string serviceID, JobInfo servicerequest)
        {
            return _serviceController.EditServiceInfo(Convert.ToInt32(serviceID), servicerequest);
        }

        public JobInfo GetServiceRequest(string serviceRequestId)
        {
            return _serviceController.GetServiceInfo(Convert.ToInt32(serviceRequestId));
        }

        public List<JobInfo> GetMyCurrentServices(string clientId)
        {
            return _serviceController.GetMyCurrentServices(Convert.ToInt32(clientId));
        }

        public List<JobInfo> GetMyCompletedServices(string clientId)
        {
            return _serviceController.GetMyCompletedServices(Convert.ToInt32(clientId));
        }

        public List<JobInfo> GetOpenServicesAssignedToMe(int contractorId)
        {
            return _serviceController.GetOpenServicesAssignedToMe(contractorId);
        }

        public List<JobInfo> GetClosedServicesAssignedToMe(int contractorId)
        {
            return _serviceController.GetClosedServicesAssignedToMe(contractorId);
        }

        public Result ChangeServiceStatus(string jobId, string StatusID)
        {
            return _serviceController.ChangeServiceStatus(Convert.ToInt32(jobId), Convert.ToInt32(StatusID));
        }


        //Search for Available Service Requeste
        public List<JobInfo> SearchServices(int CategoryId, string City, string PostCode)
        {
            return _serviceController.GetListServices(CategoryId, City, PostCode);
        }
        
        //Job Comments
        public Result AddJobComment(string jobId, CommentInfo jobcommentinfo)
        {
            return _serviceController.AddJobComment(Convert.ToInt32(jobId), jobcommentinfo);
        }

        public List<CommentInfo> GetJobComments(string jobId)
        {
            return _serviceController.GetServiceComments(Convert.ToInt32(jobId));
        }

        public Result DeleteJobComment(string jobId, string jobCommentId)
        {
            return _serviceController.DeleteJobComment(Convert.ToInt32(jobCommentId));
        }
        #endregion

        #region Proposal operations

        public Result CreateProposal(ProposalInfo proposal)
        {
            return _proposalController.Create(proposal);
        }

        public ProposalInfo GetProposal(string proposalId)
        {
            return _proposalController.GetProposal(Convert.ToInt32(proposalId));
        }

        public Result EditProposal(string proposalId, ProposalInfo proposalinfo)
        {
            return _proposalController.EditProposal(Convert.ToInt32(proposalId), proposalinfo);
        }

        public Result UpdateProposalStatus(string proposalId, UpdateProposalStatusInfo info)
        {
            return _proposalController.UpdateProposalStatus(Convert.ToInt32(proposalId), info.StatusId, info.UpdatedByUserId);
        }

        public List<ProposalInfo> GetActiveProposals(string userId, bool includeFromMe)
        {
            return _proposalController.GetActiveProposals(Convert.ToInt32(userId), includeFromMe);
        }

        public List<ProposalInfo> GetMyClosedProposals(string userId)
        {
            return _proposalController.GetMyClosedProposals(Convert.ToInt32(userId));
        }

        //public string AcceptProposal(string ProposalId, int userId)
        //{
        //    return _proposalController.AcceptProposal(Convert.ToInt32(ProposalId), userId);
        //}

        public Result SendProposalMessage(string proposalId, MessageInfo proposalmessageInfo)
        {
            return _proposalController.SendProposalMessage(Convert.ToInt32(proposalId), proposalmessageInfo);
        }

        public List<MessageInfo> GetProposalMessages(string proposalId)
        {
            return _proposalController.GetProposalMessages(Convert.ToInt32(proposalId));
        }

        public Result DeleteMessage(string proposalId, string MessageId)
        {
            return _proposalController.DeleteMessage(Convert.ToInt32(MessageId));
        }
        #endregion

        #region Appointments operations
        public List<AppointmentInfo> GetActiveAppointments(string userId)
        {
            return _appointmentController.GetActiveAppointments(Convert.ToInt32(userId));
        }

        public List<AppointmentInfo> GetClosedAppointments(string userId)
        {
            return _appointmentController.GetClosedAppointments(Convert.ToInt32(userId));
        }

        public AppointmentInfo GetAppointment(string appointmentId)
        {
            return _appointmentController.GetAppointment(Convert.ToInt32(appointmentId));
        }

        public Result CancelAppointment(string appointmentId)
        {
            return _appointmentController.CancelAppointment(Convert.ToInt32(appointmentId));
        }

        public Result CloseAppointment(string appointmentId)
        {
            return _appointmentController.CloseAppointment(Convert.ToInt32(appointmentId));
        }
        #endregion


        #region Task operations
        //4. Task operations

        public Result CreateTask(string jobId, TaskInfo taskinfo)
        {
            return _taskController.CreateTask(taskinfo);
        }

        public Result EditTask(string jobId, string taskId, TaskInfo taskinfo)
        {
            return _taskController.EditTask(Convert.ToInt32(taskId), taskinfo);
        }

        public TaskInfo GetTask(string jobId, string taskId)
        {
            return _taskController.GetTask(Convert.ToInt32(taskId));
        }

        public  Result ChangeTaskStatus(string jobId, string taskId, string statusId)
        {
            return _taskController.ChangeTaskStatus(Convert.ToInt32(taskId), Convert.ToInt32(statusId));
        }

        public List<TaskInfo> GetJobTasks(string jobId)
        {
            return _taskController.GetJobTasks(Convert.ToInt32(jobId));
        }

        public Result DeleteTask(string jobId, string taskId)
        {
            return _taskController.DeleteTask(Convert.ToInt32(taskId));
        }

        #endregion

        #region Search contractors operations

        //5.Search Contractors (WIP)
        public List<ContractorInfo> SearchContractors(int CategoryId, string City, string PostCode, double PricePerHour)
        {
            SearchContractor searchContractor = new SearchContractor();

            searchContractor.CategoryId = CategoryId;
            searchContractor.City = City;
            searchContractor.PostCode = PostCode;
            searchContractor.PricePerHour = PricePerHour;

            return _userController.GetListContractors(searchContractor);
        }

        #endregion

        #region Rating operations

        //6.Rating operations

        public Result AddRating(string userId, Rate rateinfo)
        {
            return _rateController.AddRate(rateinfo);
        }

        public List<Rate> GetUserRates(string UserID)
        {
            return _rateController.GetUserRates(Convert.ToInt32(UserID));
        }

        public double GetUserAverageRating(string UserID)
        {
            return _userController.GetUserAverage(Convert.ToInt32(UserID));
        }

        public double GetServiceRate(string ServiceID)
        {
            return _rateController.GetServiceRate(Convert.ToInt32(ServiceID));
        }

        public List<JobRateInfo> GetJobRateInfoList(string UserID)
        {
            return _serviceController.GetJobRateInfoList(Convert.ToInt32(UserID));
        }

        public Result DeleteRating(string userId, string RateId)
        {
            return _rateController.DeleteRating(Convert.ToInt32(RateId));
        }

        #endregion

    }
}
