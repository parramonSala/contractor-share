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
    [ServiceContract]
    public interface IContractorShare
    {
        //1.User operations
        [OperationContract]
        //TODO: Try to wrapp this into UserInfo class so that I can build the json with a wrapping class with the same name.
        [WebInvoke(UriTemplate = "sessions", Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        AuthenticationResult Login(LoginInfo loginInfo);

        [OperationContract]
        [WebInvoke(UriTemplate = "users", Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,RequestFormat = WebMessageFormat.Json)]
        AuthenticationResult Register(RegisterInfo registerinfo);

        [OperationContract]
        [WebInvoke(UriTemplate = "resetPassword", Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        ResetPasswordResult ResetPassword(ResetPasswordInfo resetpasswordinfo);

        [OperationContract]
        [WebInvoke(UriTemplate = "users/{userId}/password", Method = "PUT", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string ChangePassword(string userId,ChangePasswordInfo changepasswordinfo);

        [OperationContract]
        [WebInvoke(UriTemplate = "users/{userId}/preferences", Method = "PUT", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string ChangePreferences(string userId, ChangePreferencesInfo changepreferencesinfo);

        [OperationContract]
        [WebInvoke(UriTemplate = "users/{userId}/preferences", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        PreferencesResult GetPreferences(string userId);

        [OperationContract]
        [WebGet(UriTemplate = "contractors?CategoryId={CategoryId}&City={City}&PostCode={PostCode}&PricePerHour={PricePerHour}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<ContractorInfo> SearchContractors(int CategoryId, string City, string PostCode, double PricePerHour);
        
        [OperationContract]
        [WebGet(UriTemplate = "users/{userId}/averageRating", ResponseFormat = WebMessageFormat.Json,RequestFormat = WebMessageFormat.Json)]
        double GetUserAverageRating(string UserID);

        //2.Manage profile operations
        [OperationContract]
        [WebInvoke(UriTemplate = "users/{userId}/profile", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string EditUserProfile(string userId,UserInfo userprofile);

        [OperationContract]
        [WebGet(UriTemplate = "users/{userId}/profile")]
        UserInfo GetUserProfile(string userId);

        [OperationContract]
        [WebInvoke(UriTemplate = "users/{userId}/favourites", Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string AddFavourite(string userId, int ToUser);

        [OperationContract]
        [WebInvoke(UriTemplate = "users/{userId}/favourites/{favouriteUserId}", Method = "DELETE", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string RemoveFavourite(string userId, string favouriteUserId);

        [OperationContract]
        [WebGet(UriTemplate = "users/{userId}/favourites", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<ContractorInfo> GetUserFavourites(string userID);

        [OperationContract]
        [WebGet(UriTemplate = "users/{userId}/isfavourite/{fromuserId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        bool UserIsFavourite(string fromuserId, string userId);

        [OperationContract]
        [WebInvoke(UriTemplate = "users/{userId}/denunces/", Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string AddDenunce(string userId, int ToUser, string Comment, bool BlockUser);

        [OperationContract]
        [WebInvoke(UriTemplate = "users/{userId}/blocks/", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Result BlockUser(string userId, int ToUser);

        [OperationContract]
        [WebGet(UriTemplate = "users/{userId}/isblocked/{fromuserId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        bool UserIsBlocked(string fromuserId, string userId);

        //3.Manage Jobs operations
        [OperationContract]
        [WebInvoke(UriTemplate = "jobs", Method = "POST")]
        string CreateServiceRequest(JobInfo jobRequest);

        [OperationContract]
        [WebInvoke(UriTemplate = "jobs/{jobId}", Method = "PUT", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Result EditJob(string jobId, JobInfo jobRequest);

        [OperationContract]
        [WebGet(UriTemplate = "jobs/{jobId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        JobInfo GetServiceRequest(string jobId);

        [OperationContract]
        [WebGet(UriTemplate = "users/{userId}/jobs", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<JobInfo> GetMyCurrentServices(string userId);

        [OperationContract]
        [WebGet(UriTemplate = "users/{userId}/completedjobs",ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<JobInfo> GetMyCompletedServices(string userId);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<JobInfo> GetOpenServicesAssignedToMe(int contractorId);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<JobInfo> GetClosedServicesAssignedToMe(int contractorId);

        //Job Comments
        [OperationContract]
        [WebInvoke(UriTemplate = "jobs/{jobId}/addcomment", Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Result AddJobComment(string jobId, CommentInfo jobcommentinfo);

        [OperationContract]
        [WebGet(UriTemplate = "jobs/{jobId}/comments", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<CommentInfo> GetJobComments(string jobId);

        [OperationContract]
        [WebInvoke(UriTemplate = "jobs/{jobId}/comments/{jobCommentId}/delete", Method = "DELETE", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Result DeleteJobComment(string jobId, string jobCommentId);

        //4. Service Operations
        [OperationContract]
        [WebInvoke(UriTemplate = "jobs/{jobId}/status/{statusId}", Method = "PUT", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Result ChangeServiceStatus(string jobId, string statusId, string totalprice);

        [OperationContract]
        [WebGet(UriTemplate = "jobs?CategoryId={CategoryId}&City={City}&PostCode={PostCode}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<JobInfo> SearchServices(int CategoryId, string City, string PostCode);

        [OperationContract]
        [WebGet(UriTemplate = "jobs/{jobId}/jobRate", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        double GetServiceRate(string jobId);

        //Proposal Operations
        [OperationContract]
        [WebInvoke(UriTemplate = "proposals", Method = "POST")]
        Result CreateProposal(ProposalInfo proposal);

        [OperationContract]
        [WebGet(UriTemplate = "proposals/{proposalId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        ProposalInfo GetProposal(string proposalId);

        [OperationContract]
        [WebInvoke(UriTemplate = "proposals/{proposalId}", Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Result EditProposal(string proposalId, ProposalInfo proposalinfo);

        [OperationContract]
        [WebInvoke(UriTemplate = "proposals/{proposalId}/status", Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Result UpdateProposalStatus(string proposalId, UpdateProposalStatusInfo info);

        [OperationContract]
        [WebGet(UriTemplate = "users/{userId}/activeproposals?includeFromMe={includeFromMe}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<ProposalInfo> GetActiveProposals(string userId, bool includeFromMe = true);

        [OperationContract]
        [WebGet(UriTemplate = "users/{userId}/closedproposals", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<ProposalInfo> GetMyClosedProposals(string userId);

        //[OperationContract]
        //[WebInvoke(UriTemplate = "proposals/{proposalId}/accept", Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        //string AcceptProposal(string ProposalId, int userId);

        [OperationContract]
        [WebInvoke(UriTemplate = "proposals/{proposalId}/reply", Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Result SendProposalMessage(string proposalId, MessageInfo proposalmessageInfo);

        [OperationContract]
        [WebGet(UriTemplate = "proposals/{proposalId}/messages", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<MessageInfo> GetProposalMessages(string proposalId);

        [OperationContract]
        [WebInvoke(UriTemplate = "proposals/{proposalId}/messages/{MessageId}/delete", Method = "DELETE", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Result DeleteMessage(string proposalId, string MessageId);

        [OperationContract]
        [WebGet(UriTemplate = "users/{userId}/activeappointments", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<AppointmentInfo> GetActiveAppointments(string userId);

        [OperationContract]
        [WebGet(UriTemplate = "users/{userId}/closedappointments", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<AppointmentInfo> GetClosedAppointments(string userId);

        [OperationContract]
        [WebGet(UriTemplate = "appointments/{appointmentId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        AppointmentInfo GetAppointment(string appointmentId);

        [OperationContract]
        [WebInvoke(UriTemplate = "jobs/{jobId}/closeappointment", Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Result CloseAppointment(string jobId);

        [OperationContract]
        [WebInvoke(UriTemplate = "appointments/{appointmentId}/cancel", Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Result CancelAppointment(string appointmentId);

        //5.Create Task Operations
        [OperationContract]
        [WebInvoke(UriTemplate = "jobs/{jobId}/addtask", Method = "POST")]
        Result CreateTask(string jobId, TaskInfo taskinfo);

        [OperationContract]
        [WebInvoke(UriTemplate = "jobs/{jobId}/tasks/{taskId}", Method = "PUT", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Result EditTask(string jobId, string taskId, TaskInfo taskinfo);

        [OperationContract]
        [WebGet(UriTemplate = "jobs/{jobId}/tasks/{taskId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        TaskInfo GetTask(string jobId, string taskId);

        [OperationContract]
        [WebInvoke(UriTemplate = "jobs/{jobId}/tasks/{taskId}/status/{statusId}", Method = "PUT", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Result ChangeTaskStatus(string jobId, string taskId, string statusId);

        [OperationContract]
        [WebInvoke(UriTemplate = "jobs/{jobId}/tasks/{taskId}/delete", Method = "DELETE", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Result DeleteTask(string jobId, string taskId);

        [OperationContract]
        [WebGet(UriTemplate = "jobs/{jobId}/tasks", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<TaskInfo> GetJobTasks(string jobId);
      
        //6.Rating Operations
        [OperationContract]
        [WebInvoke(UriTemplate = "users/{userId}/rate", Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Result AddRating(string userId, Rate rateinfo);
        
        [OperationContract]
        [WebGet(UriTemplate = "users/{userId}/ratings", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<Rate> GetUserRates(string userId);

        [OperationContract]
        [WebInvoke(UriTemplate = "users/{userId}/ratings/{RateId}", Method = "DELETE", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Result DeleteRating(string userId, string RateId);

        [OperationContract]
        [WebGet(UriTemplate = "users/{userId}/jobstorate", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<JobRateInfo> GetJobRateInfoList(string userId);

        //Calendar Operations
        [OperationContract]
        [WebInvoke(UriTemplate = "users/{userId}/addevent", Method = "POST")]
        Result CreateEvent(string userId, EventInfo eventinfo);

        [OperationContract]
        [WebInvoke(UriTemplate = "users/{userId}/events/{eventId}", Method = "PUT", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Result EditEvent(string userId, string eventId, EventInfo eventinfo);

        [OperationContract]
        [WebInvoke(UriTemplate = "users/{userId}/events/{eventId}/delete", Method = "DELETE", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Result DeleteEvent(string userId, string eventId);

        [OperationContract]
        [WebGet(UriTemplate = "users/{userId}/events", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<EventInfo> GetUserEvents(string userId);


        //FAQS Operations
        [OperationContract]
        [WebInvoke(UriTemplate = "users/{userId}/suggestion", Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Result AddSuggestion(string userId, string comment);

        [OperationContract]
        [WebInvoke(UriTemplate = "users/{userId}/bug", Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Result AddBug(string userId, string comment);

        [OperationContract]
        [WebGet(UriTemplate = "users/{userId}/suggestionslist", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<IssueInfo> ListSuggestions(string userId);

        [OperationContract]
        [WebGet(UriTemplate = "users/{userId}/bugslist", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<IssueInfo> ListBugs(string userId);

        //Payment Operations
        [OperationContract]
        [WebGet(UriTemplate = "users/{userId}/jobsnotpaid", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<JobInfo> ListJobsToBePaid(string userId);

        [OperationContract]
        [WebGet(UriTemplate = "users/{userId}/jobspaid", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<JobInfo> ListPaidJobs(string userId);

        [OperationContract]
        [WebInvoke(UriTemplate = "users/{userId}/jobs/{jobId}/pay", Method = "PUT", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Result Pay(string userId, string jobId);
    }

}
