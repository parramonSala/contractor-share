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

        //2.Create profile operations
        [OperationContract]
        string EditUserProfile(UserInfo userprofile);

        [OperationContract]
        UserInfo GetUserProfile(int UserId);

        //3.Create Service operations
        [OperationContract]
        string CreateServiceRequest(ServiceInfo servicerequest);

        [OperationContract]
        string EditServiceRequest(int serviceID, ServiceInfo servicerequest);

        [OperationContract]
        ServiceInfo GetServiceRequest(int serviceID);

        [OperationContract]
        string ChangeServiceStatus(int serviceID, int StatusID);

        //5.Search Contractors
        [OperationContract]
        List<GetListContractors_Result> SearchContractors(SearchContractor searchcontractor);

        //6.View Professional´s profile
        [OperationContract]
        string AddFavourite(int FromUser, int ToUser);

        //Add Denunce
        [OperationContract]
        string AddDenunce(int FromUser, int ToUser, string Comment, bool BlockUser);

        //10.1 Add Rate
        [OperationContract]
        string AddRating(int FromUser, int ToUser, int service, string title, string comment, float rate);
       
    }

}
