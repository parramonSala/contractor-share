﻿using System;
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
        //TODO: Try to wrapp this into UserInfo class so that I can build the json with a wrapping class with the same name.
        [WebInvoke(UriTemplate = "sessions", Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        LoginResult Login(LoginInfo loginInfo);

        [OperationContract]
        [WebInvoke(UriTemplate = "users", Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,RequestFormat = WebMessageFormat.Json)]
        string Register(RegisterInfo registerinfo);

        [OperationContract]
        [WebInvoke(UriTemplate = "resetPassword", Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string ResetPassword(ResetPasswordInfo resetpasswordinfo);

        [OperationContract]
        [WebGet(UriTemplate = "users/contractors?CategoryId={CategoryId}&LocationCoordX={LocationCoordX}&LocationCoordY={LocationCoordY}&City={City}&CompanyName={CompanyName}&PricePerHour={PricePerHour}&NumOfRates={NumOfRates}&AverageRate={AverageRate}", ResponseFormat = WebMessageFormat.Json,RequestFormat = WebMessageFormat.Json)]
        List<GetListContractors_Result> SearchContractors(int CategoryId, decimal LocationCoordX, decimal LocationCoordY,
            string City, string CompanyName, double PricePerHour, int NumOfRates, double AverageRate);
        

        [OperationContract]
        [WebGet(UriTemplate = "users/{userId}/averageRating", ResponseFormat = WebMessageFormat.Json,RequestFormat = WebMessageFormat.Json)]
        double GetUserAverageRating(string UserID);

        //2.Manage profile operations
        [OperationContract]
        [WebInvoke(UriTemplate = "users/{userId}/profile", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string EditUserProfile(string userId,UserInfo userprofile);

        [OperationContract]
        [WebGet(UriTemplate = "user/{userId}/profile")]
        UserInfo GetUserProfile(string userId);

        [OperationContract]
        [WebInvoke(UriTemplate = "users/{userId}/favourites", Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string AddFavourite(string userId, int ToUser);

        [OperationContract]
        [WebInvoke(UriTemplate = "users/{userId}/favourites/{favouriteUserId}", Method = "DELETE", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string RemoveFavourite(string userId, string favouriteUserId);

        [OperationContract]
        [WebGet(UriTemplate = "user/{userId}/favourites", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<UserFavourite> GetUserFavourites(string userID);

        [OperationContract]
        [WebInvoke(UriTemplate = "users/{userId}/denunces/", Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string AddDenunce(string userId, int ToUser, string Comment, bool BlockUser);

        [OperationContract]
        [WebInvoke(UriTemplate = "users/{userId}/blocks/", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string BlockUser(string userId, int ToUser);

        //3.Manage Service Requests operations
        [OperationContract]
        [WebInvoke(UriTemplate = "serviceRequests", Method = "POST")]
        string CreateServiceRequest(ServiceInfo servicerequest);

        [OperationContract]
        [WebInvoke(UriTemplate = "serviceRequests/{serviceRequestId}", Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string EditServiceRequest(string serviceRequestId, ServiceInfo servicerequest);

        [OperationContract]
        [WebGet(UriTemplate = "serviceRequests/{serviceRequestId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        ServiceInfo GetServiceRequest(string serviceRequestId);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<ServiceInfo> GetMyCurrentServices(int clientId);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<ServiceInfo> GetMyCompletedServices(int clientId);

        //4. Service Operations
        [OperationContract]
        [WebInvoke(UriTemplate = "services/{serviceRequestId}", Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string ChangeServiceStatus(string serviceRequestId, int StatusID);

        [OperationContract]
        [WebGet(UriTemplate = "services?CategoryId={CategoryId}&City={City}&PostCode={PostCode}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<GetListServices_Result> SearchServices(int CategoryId, string City, string PostCode);

        [OperationContract]
        [WebGet(UriTemplate = "services/{serviceId}/serviceRate", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        double GetServiceRate(string ServiceID);

        //5.Create Task Operations
        [OperationContract]
        [WebInvoke(UriTemplate = "services/{serviceId}/tasks", Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string CreateTask(string serviceId, string name, string description);
      

        //6.Rating Operations
        [OperationContract]
        [WebInvoke(UriTemplate = "user/{userId}/ratings", Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string AddRating(string userId, int ToUser, int service, string title, string comment, float rate);
        

        [OperationContract]
        [WebGet(UriTemplate = "user/{userId}/ratings", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<Rate> GetUserRates(string userId);
    }

}
