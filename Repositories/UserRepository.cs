using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using ContractorShareService.Enumerations;
using ContractorShareService.Domain;

namespace ContractorShareService.Repositories
{
    public class UserRepository
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(UserRepository));
        private ContractorShareEntities db = new ContractorShareEntities();

        public bool UserExists(string mail, int usertype)
        {
            try
            {
                var users = from user in db.User
                            where user.Email == mail && user.UserType == usertype
                            && user.Active == true
                            select user;
                List<User> result = users.ToList();
                return (result.Count() != 0);
            }
            catch (Exception ex)
            {
                Logger.Error("Error UserRepository.UserExists", ex);
                return false;
            }
        }

        public bool UserIdExists(int UserId)
        {
            try
            {
                var users = from user in db.User
                            where user.ID == UserId
                            && user.Active == true
                            select user;
                List<User> result = users.ToList();
                return (result.Count() != 0);
            }
            catch (Exception ex)
            {
                Logger.Error("Error UserRepository.UserIdExists", ex);
                return false;
            }
        }

        public bool UserIsClient(string mail)
        {
            var usertype = from user in db.User
                           where user.Email == mail
                           select user.UserType;

            int type = usertype.First();

            return (type == (int)(UserTypeEnum.client));
        }

        public int CreateUser(string email, string password, int TypeOfUser)
        {
            try
            {
                User newuser = new User()
                {
                    UserType = TypeOfUser,
                    Email = email,
                    EncPassword = password,
                    ExpDate = DateTime.Now.AddDays(30),
                    Active = true
                };

                db.User.Add(newuser);
                db.SaveChanges();
                int id = (int)newuser.ID;

                Logger.Info(String.Format("UserRepository.CreateUser: created user {0} with ID {1}", email, id));

                return id;
            }
            catch (Exception ex)
            {
                Logger.Error("Error UserRepository.CreateUser", ex);
                return (int)(ErrorListEnum.Register_User_Error);
            }
        }

        public string Authenticate(string mail, string password)
        {
            try
            {
                var users = from user in db.User
                            where user.Email == mail && user.EncPassword == password
                            select user;

                if (!users.Any()) return EnumHelper.GetDescription(ErrorListEnum.Login_Incorrect_Password);

                User result = users.First();

                if (result.ExpDate <= DateTime.Now) return EnumHelper.GetDescription(ErrorListEnum.Login_Other_Error);

                return result.ID.ToString();

            }
            catch (Exception ex)
            {
                Logger.Error("Error UserRepository.Login", ex);
                return EnumHelper.GetDescription(ErrorListEnum.Login_Other_Error);
            }

        }

        public Enum EditUserProfile(UserInfo userprofile)
        {
            try
            {
                var users = from user in db.User
                            where user.Email == userprofile.Email
                            select user;

                User matcheduser = users.FirstOrDefault();

                matcheduser.Address = userprofile.Address;
                matcheduser.PostalCode = userprofile.PostalCode;
                matcheduser.City = userprofile.City;
                matcheduser.Country = userprofile.Country;
                matcheduser.PhoneNumber = userprofile.PhoneNumber;
                matcheduser.MobileNumber = userprofile.MobileNumber;
                matcheduser.Description = userprofile.Description;
                matcheduser.Firstname = userprofile.Firstname;
                matcheduser.Surname = userprofile.Surname;
                matcheduser.CompanyName = userprofile.CompanyName;
                matcheduser.Contractor_website = userprofile.website;
                matcheduser.CompanyCoordX = userprofile.CompanyCoordX;
                matcheduser.CompanyCoordY = userprofile.CompanyCoordY;
                matcheduser.PricePerHour = userprofile.PricePerHour;

                db.SaveChanges();

                //Add categories to the user if is a client
                if (userprofile.Categories.Count() > 0)
                {
                    foreach (int category in userprofile.Categories)
                    {
                        AddCategoryToTheClient(category, matcheduser.ID);
                    }
                }
                   
                return ErrorListEnum.OK;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error when editing user profile: {0}", ex);
                return ErrorListEnum.Profile_Other_Error;
            }
        }

        private void AddCategoryToTheClient(int category, int clientid)
        {

            try
            {
                UserCategory newUserCategory = new UserCategory
                {
                      UserID = clientid,
                      CategoryID = category
                };

                db.UserCategory.Add(newUserCategory);
                db.SaveChanges();

                int id = (int)newUserCategory.ID;

                Logger.Info(String.Format("UserRepository.AddFavourite: created userCategory relationship {0} User {1} Category {2}", id.ToString(), clientid.ToString(), category.ToString()));

            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error when adding userCategory: {0}", ex);
            }
        }

        public UserInfo GetUserProfile(int userId)
        {
            try
            {
                var users = from user in db.User
                            where user.ID == userId
                            select user;

                User userprofile = users.FirstOrDefault();

                UserInfo userinfo = new UserInfo();

                userinfo.Email = userprofile.Email;
                userinfo.Address = userprofile.Address;
                userinfo.PostalCode = userprofile.PostalCode;
                userinfo.City = userprofile.City;
                userinfo.Country = userprofile.Country;
                userinfo.PhoneNumber = userprofile.PhoneNumber;
                userinfo.MobileNumber = userprofile.MobileNumber;
                userinfo.Firstname = userprofile.Firstname;
                userinfo.Surname = userprofile.Surname;
                userinfo.CompanyName = userprofile.CompanyName;
                userinfo.website = userprofile.Contractor_website;
                userinfo.Description = userprofile.Description;
                userinfo.CompanyCoordX = userprofile.CompanyCoordX;
                userinfo.CompanyCoordY = userprofile.CompanyCoordY;
                userinfo.PricePerHour = userprofile.PricePerHour;
                userinfo.Categories = GetUserCategoryList(userId);

                return userinfo;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error when getting user profile: {0}", ex);
                return null;
            }
        }

        private List<int> GetUserCategoryList(int userId)
        {
            try
            {
                List<int> categories = (from userCategory in db.UserCategory
                                 where userCategory.UserID == userId
                                 select userCategory.CategoryID).ToList();

                return categories;

                }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error when getting user category list for user {0}: {1}", userId.ToString(), ex);
                return null;
            }
        
        }

        public List<GetListContractors_Result> GetListContractors(SearchContractor SearchParams)
        {
            try
            {
                var contractors = from contractor in
                                      db.GetListContractors(SearchParams.CategoryId, SearchParams.LocationCoordX, SearchParams.LocationCoordY,
                                      SearchParams.City, SearchParams.CompanyName, SearchParams.PricePerHour, SearchParams.NumOfRates,
                                      SearchParams.AverageRate)
                                  select contractor;

                return contractors.ToList();
            }
            catch (Exception ex)
            {
                Logger.Error("Error when getting List of Contractors", ex);
                return null;
            }
        }

        public string AddFavourite(int FromUser, int ToUser)
        {
            try
            {
                UserFavourite newfavourite = new UserFavourite
                {
                    FromUserID = FromUser,
                    ToUserID = ToUser
                };

                db.UserFavourite.Add(newfavourite);
                db.SaveChanges();
                int id = (int)newfavourite.ID;

                Logger.Info(String.Format("UserRepository.AddFavourite: created favourite relationship {0} From {1} To {2}", id.ToString(), FromUser.ToString(), ToUser.ToString()));

                return EnumHelper.GetDescription(ErrorListEnum.OK); 
            }
            catch (Exception ex)
            {
                Logger.Error("Error UserRepository.AddFavourite", ex);
                return EnumHelper.GetDescription(ErrorListEnum.Favourite_Error);
            }
        }

        public string AddDenunce(int FromUser, int ToUser, string Comment, int statusid,bool blockUser)
        {
            try
            {
                UserDenunce newdenunce = new UserDenunce
                {
                    FromUserID = FromUser,
                    ToUserID = ToUser,
                    DenunceComment = Comment,
                    StatusID = statusid,
                    BlockUser = blockUser
                };

                db.UserDenunce.Add(newdenunce);
                db.SaveChanges();
                int id = (int)newdenunce.ID;

                Logger.Info(String.Format("UserRepository.AddDenunce: created denunce {0} From {1} To {2}", id.ToString(), FromUser.ToString(), ToUser.ToString()));

                return EnumHelper.GetDescription(ErrorListEnum.OK);
            }
            catch (Exception ex)
            {
                Logger.Error("Error UserRepository.AddDenunce", ex);
                return EnumHelper.GetDescription(ErrorListEnum.Denunce_Error);
            }
        }
    }
}
