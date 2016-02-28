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
                return ex.ToString();
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

        public List<ServiceInfo> GetOpenServicesAssignedToMe(int ContractorId)
        {
            try
            {
                string message = string.Format("Executing GetOpenServicesAssignedToMe {0}", ContractorId.ToString());
                Logger.Info(message);

                return _serviceRepository.GetOpenServicesAssignedToMe(ContractorId);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error Getting Open Services assigned to contractor {0}", ContractorId.ToString());
                Logger.Error(error_message, ex);
                return null;
            }
        }

        public List<ServiceInfo> GetClosedServicesAssignedToMe(int ContractorId)
        {
      
            string message = string.Format("Executing GetClosedServicesAssignedToMe {0}", ContractorId.ToString());     
            return _serviceRepository.GetClosedServicesAssignedToMe(ContractorId);   
        }
        public Result ChangeServiceStatus(int ServiceId, int StatusId)
        {
            return _serviceRepository.ChangeServiceStatus(ServiceId, StatusId);
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



        public List<ServiceInfo> GetListServices(int categoryid, string city, string postcode)
        {
            try
             {
                string message = string.Format("Executing GetListServices");
                Logger.Info(message);

                return _serviceRepository.GetListServices(categoryid, city, postcode);
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