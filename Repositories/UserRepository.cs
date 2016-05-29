using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using ContractorShareService.Enumerations;
using ContractorShareService.Domain;
using System.Web.Security;

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
                var users = from user in db.Users
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

        public bool UserExists(string mail)
        {
            try
            {
                var users = from user in db.Users
                            where user.Email == mail
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
                var users = from user in db.Users
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
            var usertype = from user in db.Users
                           where user.Email == mail
                           select user.UserType;

            int type = usertype.First();

            return (type == (int)(ModelEnum.Client));
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
                    ExpDate = DateTime.Now.AddDays(360),
                    Active = true
                };

                db.Users.Add(newuser);
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

        public AuthenticationResult Authenticate(string mail, string password)
        {
            try
            {
                var users = from user in db.Users
                            where user.Email == mail && user.EncPassword == password
                            select user;

                AuthenticationResult loginresult = new AuthenticationResult();

                if (!users.Any()) return new AuthenticationResult(EnumHelper.GetDescription(ErrorListEnum.Login_Incorrect_Password));

                User result = users.First();

                if (result.ExpDate <= DateTime.Now) return new AuthenticationResult (EnumHelper.GetDescription(ErrorListEnum.Login_Other_Error));

                loginresult.UserId = result.ID;
                loginresult.UserType = result.UserType;
                loginresult.error = EnumHelper.GetDescription(ErrorListEnum.OK);

                return loginresult;

            }
            catch (Exception ex)
            {
                Logger.Error("Error UserRepository.Login", ex);
                return new AuthenticationResult(EnumHelper.GetDescription(ErrorListEnum.Login_Other_Error));
            }

        }

        public Enum EditUserProfile(UserInfo userprofile)
        {
            try
            {
                var users = from user in db.Users
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

                /*Add categories to the user.
                First remove the current list of categories and then
                add the new list of categories to the user*/

                if (RemoveUserCategories(matcheduser.ID) == "OK")
                {
                    if (userprofile.Categories.Count() > 0)
                    {
                        foreach (int category in userprofile.Categories)
                        {
                            AddCategoryToTheClient(category, matcheduser.ID);
                        }
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

                db.UserCategories.Add(newUserCategory);
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
                var users = from user in db.Users
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
                userinfo.AverageRate = userprofile.CAverageRate;
                userinfo.NumOfRates = userprofile.CNumOfRates;

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
                List<int> categories = (from userCategory in db.UserCategories
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

        public List<ContractorInfo> GetListContractors(SearchContractor SearchParams)
        {
            try
            {
                var contractors = from contractor in
                                  db.GetListContractors(SearchParams.CategoryId, SearchParams.PostCode, SearchParams.City, SearchParams.PricePerHour)
                                  select contractor;

                var contractorslist = contractors.ToList();

                List<ContractorInfo> contractorsinfolist = new List<ContractorInfo>();

                foreach (var selectedcontractor in contractorslist)
                {
                    ContractorInfo contractorinfo = new ContractorInfo();

                    contractorinfo.ID = selectedcontractor.ID;
                    contractorinfo.Email = selectedcontractor.Email;
                    contractorinfo.Address = selectedcontractor.Address;
                    contractorinfo.PostalCode = selectedcontractor.PostalCode;
                    contractorinfo.City = selectedcontractor.City;
                    contractorinfo.Country = selectedcontractor.Country;
                    contractorinfo.PhoneNumber = selectedcontractor.PhoneNumber;
                    contractorinfo.MobileNumber = selectedcontractor.MobileNumber;
                    contractorinfo.CompanyName = selectedcontractor.CompanyName;
                    contractorinfo.website = selectedcontractor.Contractor_website;
                    contractorinfo.Description = selectedcontractor.Description;
                    contractorinfo.Categories = GetUserCategoryList(selectedcontractor.ID);
                    contractorinfo.PricePerHour = selectedcontractor.PricePerHour;
                    contractorinfo.NumberOfRates = selectedcontractor.CNumOfRates;
                    contractorinfo.AverageRate = selectedcontractor.CAverageRate;

                    contractorsinfolist.Add(contractorinfo);
                }

                return contractorsinfolist;
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

                db.UserFavourites.Add(newfavourite);
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

        public string RemoveFavourite(int FromUser, int ToUser)
        {
            try
            {
                UserFavourite favouriterelation = (from favourite in db.UserFavourites
                                                   where favourite.FromUserID == FromUser && favourite.ToUserID == ToUser
                                                   select favourite).FirstOrDefault();

                db.UserFavourites.Remove(favouriterelation);
                db.SaveChanges();

                Logger.Info(String.Format("UserRepository.RemoveFavourite: removed favourite"));

                return EnumHelper.GetDescription(ErrorListEnum.OK);
            }
            catch (Exception ex)
            {
                Logger.Error("Error UserRepository.RemoveFavourite", ex);
                return EnumHelper.GetDescription(ErrorListEnum.Favourite_Error);
            }
        }

        public List<ContractorInfo> GetUserFavourites(int FromUser)
        {
            try
            {
                Logger.Info(String.Format("UserRepository.GetUserFavourites: get UserFavourites from UserId {0}", FromUser.ToString()));
                
                 var result = (from userfavourite in db.UserFavourites
                              join user in db.Users on userfavourite.ToUserID equals user.ID
                              where userfavourite.FromUserID == FromUser
                              
                              select new ContractorInfo()
                               {
                                   ID = user.ID,
                                   Email = user.Email,
                                   Address = user.Address,
                                   PostalCode = user.PostalCode,
                                   City = user.City,
                                   Country = user.Country,
                                   PhoneNumber = user.PhoneNumber,
                                   MobileNumber = user.MobileNumber,
                                   CompanyName = user.CompanyName,
                                   CompanyCoordX = user.CompanyCoordX,
                                   CompanyCoordY = user.CompanyCoordY,
                                   website = user.Contractor_website,
                                   Description = user.Description,
                                   Categories = fromUserCategoryToIntList(user.UserCategories.ToList()),
                                   PricePerHour = user.PricePerHour,
                                   NumberOfRates = user.CNumOfRates,
                                   AverageRate = user.CAverageRate
                               }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("Error on UserRepository.GetUserFavourites", ex);
                return null;
            }
        }

        private List<int> fromUserCategoryToIntList(List<UserCategory> usercategorylist)
        {
            List<int> categorieslist = new List<int>();

            foreach (UserCategory usercategory in usercategorylist)
            {
                categorieslist.Add(usercategory.CategoryID);
            }

            return categorieslist;
        }

        public bool UserIsFavourite(int FromUser, int ToUser)
        {
            try
            {
                var favourite = from userfavourite in db.UserFavourites
                                where userfavourite.FromUserID == FromUser
                                && userfavourite.ToUserID == ToUser
                                select userfavourite;
                return (favourite.Count() != 0);
            }
            catch (Exception ex)
            {
                Logger.Error("Error UserRepository.UserIsFavourite", ex);
                return false;
            }
        }


        public string AddDenunce(int FromUser, int ToUser, string Comment, int statusid, bool blockUser)
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

                db.UserDenunces.Add(newdenunce);
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

        public bool UserDenunceExists(int FromUser, int ToUser)
        {
            try
            {
                var denunce = from userdenunce in db.UserDenunces
                              where userdenunce.FromUserID == FromUser
                              && userdenunce.ToUserID == ToUser
                              select userdenunce;
                return (denunce.Count() != 0);
            }
            catch (Exception ex)
            {
                Logger.Error("Error UserRepository.UserDenunceExists", ex);
                return false;
            }
        }

        public bool UserIsBlocked(int FromUser, int ToUser)
        {
            try
            {
                var userblock = from block in db.UserBlocks
                              where block.FromUserID == FromUser
                              && block.ToUserID == ToUser
                              select block;
                
                return (userblock.Count() != 0);
            }
            catch (Exception ex)
            {
                Logger.Error("Error UserRepository.UserIsBlocked", ex);
                return false;
            }
        }

        public Result BlockUser(int FromUser, int ToUser)
        {
            try
            {
                UserBlock userblock = new UserBlock
                {
                    FromUserID = FromUser,
                    ToUserID = ToUser,
                };

                db.UserBlocks.Add(userblock);
                db.SaveChanges();
                int id = (int)userblock.ID;

                Logger.Info(String.Format("UserRepository.BlockUser: created denunce {0} From {1} To {2}", id.ToString(), FromUser.ToString(), ToUser.ToString()));

                return new Result();
            }
            catch (Exception ex)
            {
                Logger.Error("Error UserRepository.BlockUser", ex);
                return new Result(ex.ToString(), (int)(ErrorListEnum.BlockUserOtherError));
            }
        }

        public Result UnBlockUser(int FromUser, int ToUser)
        {
            try
            {
                UserBlock userblock = (from block in db.UserBlocks
                                       where block.FromUserID == FromUser && block.ToUserID == ToUser
                                       select block).FirstOrDefault();

                db.UserBlocks.Remove(userblock);
                db.SaveChanges();

                Logger.Info(String.Format("UserRepository.UnBlock"));

                return new Result();
            }
            catch (Exception ex)
            {
                Logger.Error("Error UserRepository.RemoveFavourite", ex);
                return new Result(ex.ToString(), (int)ErrorListEnum.BlockUserOtherError);
            }
        }


        public double GetUserAverage(int UserID)
        {
            try
            {
                double averagerate = (double)(from user in db.Users
                                              where user.ID == UserID
                                              select user.CAverageRate).FirstOrDefault();
                return averagerate;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error when getting user {0} rating average", ex);
                return -1;
            }
        }


        public string ResetPassword(string email)
        {
            int error = -1;
            try
            {
                var users = from user in db.Users
                            where user.Email == email
                            select user;

                User matcheduser = users.FirstOrDefault();

                string password = Membership.GeneratePassword(12, 1);

                matcheduser.EncPassword = password;
                matcheduser.ExpDate = DateTime.Now.AddDays(1);

                db.SaveChanges();

                return password;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error when resetting user password: {0}", ex);
                return error.ToString();
            }
        }

        public string ChangePassword(string email, string newpassword)
        {
            try
            {
                var users = from user in db.Users
                            where user.Email == email
                            select user;

                User matcheduser = users.FirstOrDefault();

                matcheduser.EncPassword = newpassword;
                matcheduser.ExpDate = DateTime.Now.AddDays(360);

                db.SaveChanges();

                return EnumHelper.GetDescription(ErrorListEnum.OK);
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error when changing user password: {0}", ex);
                return ex.ToString();
            }
        }

        public String CreateUserPreferences(int userId, ChangePreferencesInfo defaultpreferences)
        {
            try
            {
                Preference newuserpreferences = new Preference()
                {
                    UserID = userId,
                    EnableNotifications = defaultpreferences.enableNotifications,
                    ShowContactEmail = defaultpreferences.showContactEmail,
                    ShareLocation = defaultpreferences.shareLocation,
                    ShowContactNumber = defaultpreferences.showContactNumber
                };

                db.Preferences.Add(newuserpreferences);
                db.SaveChanges();

                Logger.Info(String.Format("UserRepository.CreateUserPreferences: created userpreferences for user {0}", userId.ToString()));

                return "OK";
            }
            catch (Exception ex)
            {
                Logger.Error("Error UserRepository.CreateUser", ex);
                return ex.ToString();
            }
        }

        public PreferencesResult GetPreferences(int userId)
        {
            try
            {
                var userpreferences = from preference in db.Preferences
                                      where preference.UserID == userId
                                      select preference;

                Preference userpreference = userpreferences.FirstOrDefault();

                PreferencesResult preferenceresult = new PreferencesResult();

                preferenceresult.enableNotifications = userpreference.EnableNotifications;
                preferenceresult.showContactEmail = userpreference.ShowContactEmail;
                preferenceresult.showContactNumber = userpreference.ShowContactNumber;
                preferenceresult.shareLocation = userpreference.ShareLocation;

                return preferenceresult;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error when getting user preferences: {0}", ex);
                return null;
            }
        }

        public string ChangePreferences(string userId, ChangePreferencesInfo preferences)
        {
            try
            {
                int userid = Convert.ToInt32(userId);

                var userpreferences = from userpref in db.Preferences
                                      where userpref.UserID == userid
                                      select userpref;

                Preference matcheduserpref = userpreferences.FirstOrDefault();

                matcheduserpref.ShareLocation = preferences.shareLocation;
                matcheduserpref.ShowContactEmail = preferences.showContactEmail;
                matcheduserpref.ShowContactNumber = preferences.showContactNumber;
                matcheduserpref.EnableNotifications = preferences.enableNotifications;

                db.SaveChanges();

                return EnumHelper.GetDescription(ErrorListEnum.OK); ;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error when changing user preferences: {0}", ex);
                return ex.ToString();
            }
        }

        public string RemoveUserCategories(int userId)
        {
            try
            {
                List<UserCategory> usercategories = new List<UserCategory>();

                usercategories = (from categories in db.UserCategories
                                  where categories.UserID == userId
                                  select categories).ToList();

                if (usercategories.Count() > 0)
                {
                    foreach (UserCategory usercategory in usercategories)
                    {
                        db.UserCategories.Remove(usercategory);
                        db.SaveChanges();
                    }
                }

                Logger.Info("UserRepository.RemoveUserCategories OK");

                return EnumHelper.GetDescription(ErrorListEnum.OK);
            }
            catch (Exception ex)
            {
                Logger.Error("Error UserRepository.RemoveUserCategories", ex);
                return ex.ToString();
            }
        }
    }
}
