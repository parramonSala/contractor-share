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
        protected static ILog Logger = LogManager.GetLogger(typeof(ContractorShare));
        private UserController _userController = new UserController();
        private ServiceController _serviceController = new ServiceController();
        private RateController _rateController = new RateController();

        //1.Login operations
        public string Login(string email, string password, int TypeOfUser)
        {
            return _userController.Login(email, password, TypeOfUser);
        }

        public string Register(string email, string password, int TypeOfUser)
        {
            return _userController.Register(email, password, TypeOfUser);
        }

        //2.Create profile
        public string EditUserProfile(UserInfo userprofile)
        {
            return _userController.EditUserProfile(userprofile);
        }

        public UserInfo GetUserProfile(int userId)
        {
            return _userController.GetUserProfile(userId);
        }

        //3.Create service request
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

        //5.Search Contractors (WIP)
        public List<GetListContractors_Result> SearchContractors(SearchContractor Searchcontractor)
        {
            return _userController.GetListContractors(Searchcontractor);
        }

        //6.View Professional´s profile
        //Add as Favourite
        public string AddFavourite(int FromUser, int ToUser)
        {
            return _userController.AddFavourite(FromUser,ToUser);
        }

        //Add a denunce
        public string AddDenunce(int FromUser, int ToUser, string Comment, bool BlockUser)
        {
            return _userController.AddDenunce(FromUser, ToUser, Comment, BlockUser);
        }

        //Add a Rate
        public string AddRating(Rate rate)
        {
            return _rateController.AddRate(rate);
        }

        //Get User´s Rate
        public List<Rate> GetUserRates(int UserID)
        {
            return _rateController.GetUserRates(UserID);
        }

        //Get User´s average rating IN PROGRESS!
        public float GetUserAverageRating(int UserID)
        {
            throw new NotImplementedException();
        }
       
    }
}
