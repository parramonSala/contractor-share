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

        public Result AddRate(Rate rate)
        {
            int id = -1;
            try
            {
                Rating newrate = new Rating
                {
                    FromUserID = rate.FromUserId,
                    ToUserID = rate.ToUserId,
                    ServiceID = rate.ServiceId,
                    Title = rate.Title,
                    Comment = rate.Comment,
                    rating1 = rate.Rating,
                    Created = DateTime.Now
                };

                db.Ratings.Add(newrate);
                db.SaveChanges();
                id = (int)newrate.ID;

                Logger.Info(String.Format("RateRepository.AddRate: created rate {0} ", id.ToString()));

                //Update User Average Rate
                Logger.Info(String.Format("RateRepository.AddRate: Updating User {0} Average Rate ", rate.ToUserId.ToString()));

                User user = (from matched_user in db.Users
                             where matched_user.ID == rate.ToUserId
                             select matched_user).FirstOrDefault();

                double addedrate = Convert.ToDouble(user.CTotalRate) + rate.Rating;
                int numOfRates = Convert.ToInt32(user.CNumOfRates) + 1;

                user.CTotalRate = addedrate;
                user.CNumOfRates = numOfRates;
                user.CAverageRate = addedrate / numOfRates;

                db.SaveChanges();

                Logger.Info(String.Format("RateRepository.AddRate: User {0} CAverageRate updated to {1}",
                    rate.ToUserId.ToString(), (addedrate / numOfRates).ToString()));

                return new Result();
            }
            catch (Exception ex)
            {
                Logger.Error("Error RateRepository.AddRate", ex);
                return new Result(ex.ToString(), (int)ErrorListEnum.Rate_OtherError);
            }

        }

        public List<Rate> GetUserRates(int UserID)
        {
            try
            {
                List<Rating> rates = (from rating in db.Ratings
                                      where rating.ToUserID == UserID
                                      select rating).ToList();

                List<Rate> userRates = new List<Rate>();

                foreach (Rating rating in rates)
                {
                    Rate rate = new Rate();
                    rate.RateId = rating.ID;
                    rate.FromUserId = rating.FromUserID;
                    rate.ToUserId = rating.ToUserID;
                    rate.ServiceId = (int)rating.ServiceID;
                    rate.Title = rating.Title;
                    rate.Comment = rating.Comment;
                    rate.Rating = (int)rating.rating1;
                    rate.Created = rating.Created;

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

        public double GetServiceRate(int ServiceID)
        {
            try
            {
                Rating rate = (from rating in db.Ratings
                               where rating.ServiceID == ServiceID
                               select rating).FirstOrDefault();

                return (double)rate.rating1;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error when getting rate for service {0}: {1}", ServiceID.ToString(), ex);
                return -1;
            }
        }

        public Result DeleteRating(int RateId)
        {
            try
            {
                Logger.Info(String.Format("RateRepository.DeleteRating"));

                Rating rating = (from r in db.Ratings
                                 where r.ID == RateId
                                 select r).FirstOrDefault();

                db.Ratings.Remove(rating);
                db.SaveChanges();
                return new Result();
            }
            catch (Exception ex)
            {
                Logger.Error("Error RateRepository.DeleteRating", ex);
                return new Result(ex.ToString(), (int)(ErrorListEnum.Rate_DeleteError));
            }
        }
    }
}