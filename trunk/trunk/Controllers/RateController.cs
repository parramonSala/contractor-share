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
    public class RateController
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(UserController));
        private RateRepository _rateRepository = new RateRepository();
        private UserRepository _userRepository = new UserRepository();
        private ServiceRepository _serviceRepository = new ServiceRepository();

        public string AddRate(Rate rate)
        {
            try
            {
                string message = string.Format("Executing AddRate(FromUser {0}, ToUser {1}, ToService {2})", rate.FromUserId.ToString(), rate.ToUserId.ToString(),rate.ServiceId.ToString());
                Logger.Info(message);

                if (!_userRepository.UserIdExists(rate.FromUserId) || !_userRepository.UserIdExists(rate.ToUserId) || (rate.ServiceId != null && !_serviceRepository.ServiceIdExists(rate.ServiceId)))
                {
                    return EnumHelper.GetDescription(ErrorListEnum.Rate_NonExistError);
                }

                return _rateRepository.AddRate(rate);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error executing AddRate(From {0}, To {1},ToService {2})", rate.FromUserId.ToString(), rate.ToUserId.ToString(), rate.ServiceId.ToString());
                Logger.Error(error_message, ex);
                return EnumHelper.GetDescription(ErrorListEnum.Rate_OtherError);
            }

        }


        public List<Rate> GetUserRates(int UserID)
        {
            try
            {
                string message = string.Format("Executing GetUserRates for user {0}", UserID.ToString());
                Logger.Info(message);

                return _rateRepository.GetUserRates(UserID);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error executing GetUserRates for user {0}", UserID.ToString());
                Logger.Error(error_message, ex);
                return null;
            }
        }

    }
}