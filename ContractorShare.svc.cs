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

namespace ContractorShareService
{
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

        #endregion

        #region Login operations

        //1.Login operations
        public string Login(string email, string password, int TypeOfUser)
        {
            return _userController.Login(email, password, TypeOfUser);
        }

        public string Register(string email, string password, int TypeOfUser)
        {
            return _userController.Register(email, password, TypeOfUser);
        }

        #endregion

        #region Profile operations

        //2.Profile operations
        public string EditUserProfile(UserInfo userprofile)
        {
            return _userController.EditUserProfile(userprofile);
        }

        public UserInfo GetUserProfile(int userId)
        {
            return _userController.GetUserProfile(userId);
        }

        public string AddFavourite(int FromUser, int ToUser)
        {
            return _userController.AddFavourite(FromUser, ToUser);
        }

        public string RemoveFavourite(int FromUser, int ToUser)
        {
            return _userController.RemoveFavourite(FromUser, ToUser);
        }

        public List<UserFavourite> GetUserFavourites(int FromUser)
        {
            return _userController.GetUserFavourites(FromUser);
        }


        public string AddDenunce(int FromUser, int ToUser, string Comment, bool BlockUser)
        {
            return _userController.AddDenunce(FromUser, ToUser, Comment, BlockUser);
        }

        #endregion

        #region Service request operations

        //3.Service request operations
        public string CreateServiceRequest(ServiceInfo servicerequest)
        {
            return _serviceController.Create(servicerequest);
        }

        public string EditServiceRequest(int serviceID, ServiceInfo servicerequest)
        {
            return _serviceController.EditServiceInfo(serviceID, servicerequest);
        }

        public ServiceInfo GetServiceRequest(int serviceID)
        {
            return _serviceController.GetServiceInfo(serviceID);
        }

        public string ChangeServiceStatus(int serviceID, int StatusID)
        {
            return _serviceController.ChangeServiceStatus(serviceID, StatusID);
        }

        public string AddServiceComment(int serviceID, int CreatedByUserID, string CommentTitle, string CommentText)
        {
            return _serviceController.AddServiceComment(serviceID, CreatedByUserID, CommentTitle, CommentText);
        }

        #endregion

        #region Task operations
        //4. Task operations

        public string CreateTask(string name, string description, int serviceId)
        {
            return _taskController.CreateTask(name, description, serviceId);
        }

        #endregion

        #region Search contractors operations

        //5.Search Contractors (WIP)
        public List<GetListContractors_Result> SearchContractors(SearchContractor Searchcontractor)
        {
            return _userController.GetListContractors(Searchcontractor);
        }

        #endregion

        #region Rating operations

        //6.Rating operations

        public string AddRating(int fromUserId, int toUserId, int serviceId, string title, string comment, float rate)
        {
            Domain.Rate rating = new Domain.Rate();
            rating.FromUserId = fromUserId;
            rating.ToUserId = toUserId;
            rating.ServiceId = serviceId;
            rating.Title = title;

            return _rateController.AddRate(rating);
        }

        public List<Rate> GetUserRates(int UserID)
        {
            return _rateController.GetUserRates(UserID);
        }

        public float GetUserAverageRating(int UserID)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
