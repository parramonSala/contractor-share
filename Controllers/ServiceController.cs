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
    public class ServiceController
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(ServiceController));
        private ServiceRepository _serviceRepository = new ServiceRepository();

        public string Create(ServiceInfo servicerequest)
        {
            try
            {
                string message = string.Format("Executing Create ServiceRequest");
                Logger.Info(message);

                return _serviceRepository.CreateService(servicerequest);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error Creating ServiceRequest");
                Logger.Error(error_message, ex);
                return EnumHelper.GetDescription(ErrorListEnum.Service_Create_Error);
            }

        }

        public string EditServiceInfo(int ServiceId, ServiceInfo servicerequest)
        {
            try
            {
                string message = string.Format("Executing Edit ServiceRequest {0}", ServiceId.ToString());
                Logger.Info(message);

                return _serviceRepository.EditService(ServiceId, servicerequest);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error Editing ServiceRequest {0}", ServiceId.ToString());
                Logger.Error(error_message, ex);
                return EnumHelper.GetDescription(ErrorListEnum.Service_Edit_Error);
            }
        }

        public ServiceInfo GetServiceInfo(int ServiceId)
        {
            try
            {
                string message = string.Format("Executing Get ServiceInfo {0}", ServiceId.ToString());
                Logger.Info(message);

                return _serviceRepository.GetServiceInfo(ServiceId);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error Getting ServiceRequest {0}", ServiceId.ToString());
                Logger.Error(error_message, ex);
                return null;
            }
        }

        public List<ServiceInfo> GetMyCurrentServices(int ClientId)
        {
            try
            {
                string message = string.Format("Executing GetMyServices {0}", ClientId.ToString());
                Logger.Info(message);

                return _serviceRepository.GetMyCurrentServices(ClientId);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error Getting Services for client {0}", ClientId.ToString());
                Logger.Error(error_message, ex);
                return null;
            }
        }

        public List<ServiceInfo> GetMyCompletedServices(int ClientId)
        {
            try
            {
                string message = string.Format("Executing GetMyServices {0}", ClientId.ToString());
                Logger.Info(message);

                return _serviceRepository.GetMyCompletedServices(ClientId);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error Getting Services for client {0}", ClientId.ToString());
                Logger.Error(error_message, ex);
                return null;
            }
        }

        public string ChangeServiceStatus(int ServiceId, int StatusId)
        {
            try
            {
                string message = string.Format("Executing ChangeServiceStatus {0}, {1}", ServiceId.ToString(), StatusId.ToString());
                Logger.Info(message);

                return _serviceRepository.ChangeServiceStatus(ServiceId, StatusId);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error ChangeServiceStatus {0}, {1}", ServiceId.ToString(), StatusId.ToString());
                Logger.Error(error_message, ex);
                return EnumHelper.GetDescription(ErrorListEnum.Service_Edit_Error);
            }
        }


        public string AddServiceComment(int serviceID, int userID, string CommentTitle, string CommentText)
        {
            try
            {
                string message = string.Format("Executing AddServiceComment {0}", serviceID.ToString());
                Logger.Info(message);

                return _serviceRepository.AddComment(serviceID, userID, CommentTitle, CommentText).ToString();
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error AddServiceComment {0}", serviceID.ToString()); 
                Logger.Error(error_message, ex);
                return EnumHelper.GetDescription(ErrorListEnum.Comment_AddError);
            }
        }

        public List<Comment> GetServiceComment(int serviceID)
        {
            try
            {
                string message = string.Format("Executing GetServiceComment {0}", serviceID.ToString());
                Logger.Info(message);

                return _serviceRepository.GetServiceComments(serviceID);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error Getting ServiceComment {0}", serviceID.ToString());
                Logger.Error(error_message, ex);
                return null;
            }
        }



        public List<GetListServices_Result> GetListServices(SearchService Searchservice)
        {
            try
             {
                string message = string.Format("Executing GetListServices");
                Logger.Info(message);

                return _serviceRepository.GetListServices(Searchservice);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error executing GetListServices");
                Logger.Error(error_message, ex);
                return null;
            }
        }
    }
}