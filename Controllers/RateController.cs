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

        public Result AddRate(Rate rate)
        {
                string message = string.Format("Executing AddRate(FromUser {0}, ToUser {1}, ToService {2})", rate.FromUserId.ToString(), rate.ToUserId.ToString(),rate.ServiceId.ToString());
                Logger.Info(message);

                if (!_userRepository.UserIdExists(rate.FromUserId) || !_userRepository.UserIdExists(rate.ToUserId) || (rate.ServiceId != 0 && !_serviceRepository.ServiceIdExists(rate.ServiceId)))
                {
                    return new Result(EnumHelper.GetDescription(ErrorListEnum.Rate_NonExistError), (int)ErrorListEnum.Rate_NonExistError);
                }

                return _rateRepository.AddRate(rate);
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

        public double GetServiceRate(int ServiceID)
        {
            try
            {
                string message = string.Format("Executing GetServiceRate for service {0}", ServiceID.ToString());
                Logger.Info(message);

                return _rateRepository.GetServiceRate(ServiceID);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error executing GetServiceRate for service {0}", ServiceID.ToString());
                Logger.Error(error_message, ex);
                return -1;
            }
        }

        public Result DeleteRating(int RateId)
        {
            string message = string.Format("Executing DeleteRating for rate {0}", RateId.ToString());
            Logger.Info(message);

            return _rateRepository.DeleteRating(RateId);
        }

    }
}