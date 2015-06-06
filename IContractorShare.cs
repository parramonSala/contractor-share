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
        //1.User operations
        [OperationContract]
        [WebInvoke(UriTemplate = "sessions", Method = "POST", BodyStyle= WebMessageBodyStyle.Wrapped)]
        string Login(string email, string password, int TypeOfUser);

        [OperationContract]
        [WebInvoke(UriTemplate = "users", Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped)]
        string Register(string email, string password, int TypeOfUser);

        [OperationContract]
        [WebGet(UriTemplate = "users/contractors?CategoryId={CategoryId}&LocationCoordX={LocationCoordX}&LocationCoordY={LocationCoordY}&City={City}&CompanyName={CompanyName}&PricePerHour={PricePerHour}&NumOfRates={NumOfRates}&AverageRate={AverageRate}")]
        List<GetListContractors_Result> SearchContractors(int CategoryId, decimal LocationCoordX, decimal LocationCoordY,
            string City, string CompanyName, double PricePerHour, int NumOfRates, double AverageRate);
        

        [OperationContract]
        [WebGet(UriTemplate = "users/{userId}/averageRating")]
        double GetUserAverageRating(string UserID);

        //2.Manage profile operations
        [OperationContract]
        [WebInvoke(UriTemplate = "users/{userId}/profile", Method = "POST")]
        string EditUserProfile(string userId,UserInfo userprofile);

        [OperationContract]
        [WebGet(UriTemplate = "user/{userId}/profile")]
        UserInfo GetUserProfile(string userId);

        [OperationContract]
        [WebInvoke(UriTemplate = "users/{userId}/favourites", Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped)]
        string AddFavourite(string userId, int ToUser);

        [OperationContract]
        [WebInvoke(UriTemplate = "users/{userId}/favourites/{favouriteUserId}", Method = "DELETE", BodyStyle = WebMessageBodyStyle.Wrapped)]
        string RemoveFavourite(string userId, string favouriteUserId);

        [OperationContract]
        [WebGet(UriTemplate = "user/{userId}/favourites")]
        List<UserFavourite> GetUserFavourites(string userID);

        [OperationContract]
        [WebInvoke(UriTemplate = "users/{userId}/denunces/", Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped)]
        string AddDenunce(string userId, int ToUser, string Comment, bool BlockUser);

        [OperationContract]
        [WebInvoke(UriTemplate = "users/{userId}/blocks/", Method = "POST")]
        string BlockUser(string userId, int ToUser);

        //3.Manage Service Requests operations
        [OperationContract]
        [WebInvoke(UriTemplate = "serviceRequests", Method = "POST")]
        string CreateServiceRequest(ServiceInfo servicerequest);

        [OperationContract]
        [WebInvoke(UriTemplate = "serviceRequests/{serviceRequestId}", Method = "PUT")]
        string EditServiceRequest(string serviceRequestId, ServiceInfo servicerequest);

        [OperationContract]
        [WebGet(UriTemplate="serviceRequests/{serviceRequestId}")]
        ServiceInfo GetServiceRequest(string serviceRequestId);

        //4. Service Operations
        [OperationContract]
        [WebInvoke(UriTemplate = "services/{serviceRequestId}", Method = "PUT")]
        string ChangeServiceStatus(string serviceRequestId, int StatusID);

        [OperationContract]
        [WebGet(UriTemplate = "services?CategoryId={CategoryId}&City={City}&PostCode={PostCode}")]
        List<GetListServices_Result> SearchServices(int CategoryId, string City, string PostCode);

        [OperationContract]
        [WebGet(UriTemplate = "services/{serviceId}/serviceRate")]
        double GetServiceRate(string ServiceID);

        //5.Create Task Operations
        [OperationContract]
        [WebInvoke(UriTemplate = "services/{serviceId}/tasks", Method = "POST",BodyStyle= WebMessageBodyStyle.Wrapped)]
        string CreateTask(string serviceId, string name, string description);
      

        //6.Rating Operations
        [OperationContract]
        [WebInvoke(UriTemplate = "user/{userId}/ratings", Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped)]
        string AddRating(string userId, int ToUser, int service, string title, string comment, float rate);
        

        [OperationContract]
        [WebGet(UriTemplate="user/{userId}/ratings")]
        List<Rate> GetUserRates(string userId);
    }

}
