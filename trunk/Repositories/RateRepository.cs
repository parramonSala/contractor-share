using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using ContractorShareService.Enumerations;
using ContractorShareService.Domain;

namespace ContractorShareService.Repositories
{
    public class RateRepository
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(UserRepository));
        private ContractorShareEntities db = new ContractorShareEntities();

        public string AddRate(Rate rate)
        {
            try
            {
                Rating newrate = new Rating
                {
                    FromUserID = rate.FromUserId,
                    ToUserID = rate.ToUserId,
                    ServiceID = rate.ServiceId,
                    Title = rate.Title,
                    Comment = rate.Comment,
                    rating1 = rate.Rating
                };

                db.Ratings.Add(newrate);
                db.SaveChanges();
                int id = (int)newrate.ID;

                Logger.Info(String.Format("RateRepository.AddRate: created rate {0} ", id.ToString()));

                return EnumHelper.GetDescription(ErrorListEnum.OK);
            }
            catch (Exception ex)
            {
                Logger.Error("Error RateRepository.AddRate", ex);
                return EnumHelper.GetDescription(ErrorListEnum.Rate_OtherError);
            }
        
        }

        public List<Rate> GetUserRates(int UserID)
        {
            try
            {
                List<Rating> rates = (from rating in db.Ratings
                                        where rating.FromUserID == UserID
                                        select rating).ToList();

                List<Rate> userRates = new List<Rate>();

                foreach (Rating rating in rates)
                {
                    Rate rate = new Rate();
                    rate.FromUserId = rating.FromUserID;
                    rate.ToUserId = rating.ToUserID;
                    rate.ServiceId = (int)rating.ServiceID;
                    rate.Title = rating.Title;
                    rate.Comment = rating.Comment;
                    rate.Rating = (int)rating.rating1;

                    userRates.Add(rate);
                }

                return userRates;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error when getting user rates list for user {0}: {1}", UserID.ToString(), ex);
                return null;
            }
        
        }
    }
}