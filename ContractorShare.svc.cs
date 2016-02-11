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

        #endregion

        #region Login operations

        //1.Login operations
        public LoginResult Login(LoginInfo loginInfo)
        {
            return _userController.Login(loginInfo.Email, loginInfo.Password);
        }

        public string Register(RegisterInfo registerinfo)
        {
            return _userController.Register(registerinfo.Email, registerinfo.Password, registerinfo.UserType);
        }

        public string ResetPassword(ResetPasswordInfo resetpasswordinfo)
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


        public string AddDenunce(string userId, int ToUser, string Comment, bool BlockUser)
        {
            return _userController.AddDenunce(Convert.ToInt32(userId), ToUser, Comment, BlockUser);
        }

        public string BlockUser(string userId, int ToUser)
        {
            return _userController.BlockUser(Convert.ToInt32(userId), ToUser);
        }

        #endregion

        #region Service request operations

        //3.Service request operations
        public string CreateServiceRequest(ServiceInfo servicerequest)
        {
            return _serviceController.Create(servicerequest);
        }

        public string EditServiceRequest(string serviceID, ServiceInfo servicerequest)
        {
            return _serviceController.EditServiceInfo(Convert.ToInt32(serviceID), servicerequest);
        }

        public ServiceInfo GetServiceRequest(string serviceRequestId)
        {
            return _serviceController.GetServiceInfo(Convert.ToInt32(serviceRequestId));
        }

        public List<ServiceInfo> GetMyCurrentServices(string clientId)
        {
            return _serviceController.GetMyCurrentServices(Convert.ToInt32(clientId));
        }

        public List<ServiceInfo> GetMyCompletedServices(string clientId)
        {
            return _serviceController.GetMyCompletedServices(Convert.ToInt32(clientId));
        }

        public List<ServiceInfo> GetOpenServicesAssignedToMe(int contractorId)
        {
            return _serviceController.GetOpenServicesAssignedToMe(contractorId);
        }

        public List<ServiceInfo> GetClosedServicesAssignedToMe(int contractorId)
        {
            return _serviceController.GetClosedServicesAssignedToMe(contractorId);
        }

        public string ChangeServiceStatus(string serviceID, int StatusID)
        {
            return _serviceController.ChangeServiceStatus(Convert.ToInt32(serviceID), StatusID);
        }

        public string AddServiceComment(int serviceID, int CreatedByUserID, string CommentTitle, string CommentText)
        {
            return _serviceController.AddServiceComment(serviceID, CreatedByUserID, CommentTitle, CommentText);
        }

        public List<Comment> GetServiceComments(int serviceID)
        {
            return _serviceController.GetServiceComment(serviceID);
        }

        //Search for Available Service Requeste
        public List<ServiceInfo> SearchServices(int CategoryId, string City, string PostCode)
        {
            return _serviceController.GetListServices(CategoryId, City, PostCode);
        }

        #endregion

        #region Proposal operations

        public string CreateProposal(ProposalInfo proposal)
        {
            return _proposalController.Create(proposal);
        }

        #endregion


        #region Task operations
        //4. Task operations

        public string CreateTask(string serviceId,string name, string description)
        {
            return _taskController.CreateTask(name, description, Convert.ToInt32(serviceId));
        }

        #endregion

        #region Search contractors operations

        //5.Search Contractors (WIP)
        public List<GetListContractors_Result> SearchContractors(int CategoryId, decimal LocationCoordX, decimal LocationCoordY,
            string City, string CompanyName, double PricePerHour, int NumOfRates, double AverageRate)
        {
            SearchContractor searchContractor = new SearchContractor();
            //TODO: Fill searchContractor with parameters
            return _userController.GetListContractors(searchContractor);
        }

        #endregion

        #region Rating operations

        //6.Rating operations

        public string AddRating(string userId, int toUserId, int serviceId, string title, string comment, float rate)
        {
            Domain.Rate rating = new Domain.Rate();
            rating.FromUserId = Convert.ToInt32(userId);
            rating.ToUserId = toUserId;
            rating.ServiceId = serviceId;
            rating.Title = title;
            rating.Rating = rate;

            return _rateController.AddRate(rating);
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

        #endregion

    }
}
