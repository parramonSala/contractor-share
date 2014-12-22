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

                int id = _serviceRepository.CreateService(servicerequest);

                if (id < 0) return EnumHelper.GetDescription(ErrorListEnum.Service_Create_Error);
                else return id.ToString();
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

        public string ChangeServiceStatus(int ServiceId, int StatusId)
        {
            try
            {
                string message = string.Format("Executing CloseServiceRequest {0}", ServiceId.ToString());
                Logger.Info(message);

                return _serviceRepository.ChangeServiceStatus(ServiceId, StatusId);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error CloseServiceRequest {0}", ServiceId.ToString());
                Logger.Error(error_message, ex);
                return EnumHelper.GetDescription(ErrorListEnum.Service_Close_Error);
            }
        }

    }
}