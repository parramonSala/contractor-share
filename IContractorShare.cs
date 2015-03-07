using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ContractorShareService.Domain;

namespace ContractorShareService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IContractorShare
    {
        //1.Login operations
        [OperationContract]
        string Login(string email, string password, int TypeOfUser);

        [OperationContract]
        string Register(string email, string password, int TypeOfUser);

        //2.Manage profile operations
        [OperationContract]
        string EditUserProfile(UserInfo userprofile);

        [OperationContract]
        UserInfo GetUserProfile(int UserId);

        [OperationContract]
        string AddFavourite(int FromUser, int ToUser);

        [OperationContract]
        string RemoveFavourite(int FromUser, int ToUser);

        [OperationContract]
        List<UserFavourite> GetUserFavourites(int FromUser);

        [OperationContract]
        string AddDenunce(int FromUser, int ToUser, string Comment, bool BlockUser);

        [OperationContract]
        string BlockUser(int FromUser, int ToUser);

        //3.Manage Service Requests operations
        [OperationContract]
        string CreateServiceRequest(ServiceInfo servicerequest);

        [OperationContract]
        string EditServiceRequest(int serviceID, ServiceInfo servicerequest);

        [OperationContract]
        ServiceInfo GetServiceRequest(int serviceID);

        [OperationContract]
        string ChangeServiceStatus(int serviceID, int StatusID);

        [OperationContract]
        List<GetListServices_Result> SearchServices(SearchService Searchservice);

        //4.Create Task Operations
        [OperationContract]
        string CreateTask(string name, string description, int serviceId);
       
        //5.Search Contractors
        [OperationContract]
        List<GetListContractors_Result> SearchContractors(SearchContractor searchcontractor);

        //6.Rating Operations
        [OperationContract]
        string AddRating(int FromUser, int ToUser, int service, string title, string comment, float rate);

        [OperationContract]
        double GetUserAverageRating(int UserID);
    }

}
