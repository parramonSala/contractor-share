using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using ContractorShareService.Repositories;
using ContractorShareService.Enumerations;
using ContractorShareService.Domain;
using System.Net.Mail;
using System.Web.Http;
using System.Net;

namespace ContractorShareService.Controllers
{
    public class UserController
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(UserController));
        private UserRepository _userRepository = new UserRepository();
        private StatusRepository _statusRepository = new StatusRepository();

        public AuthenticationResult Login(string email, string password)
        {
            try
            {
                string message = string.Format("Executing Login for user {0}", email);
                Logger.Info(message);

                bool exists = _userRepository.UserExists(email);

                if (exists)
                {
                    return _userRepository.Authenticate(email, password);
                }
                else
                {
                    string error_message = string.Format("Error Login: user with mail {0} doesn't exist in the DB", email);
                    Logger.Error(error_message);
                    return new AuthenticationResult(EnumHelper.GetDescription(ErrorListEnum.Login_Client_NoExists));
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error Login user {0}", email);
                Logger.Error(error_message, ex);
                return new AuthenticationResult(EnumHelper.GetDescription(ErrorListEnum.Login_Other_Error));
            }
        }

        public AuthenticationResult Register(string email, string password, int userTypeId)
        {
            AuthenticationResult result = new AuthenticationResult() { UserId = -1, UserType = userTypeId, error = EnumHelper.GetDescription(ErrorListEnum.Register_Other_Error) };
            try
            {
                string message = string.Format("Executing Register for user {0}", email);
                Logger.Info(message);

                if (!_userRepository.UserExists(email, userTypeId))
                {
                    int userid = _userRepository.CreateUser(email, password, userTypeId);

                    if (userid < 0) return new AuthenticationResult() { UserId = userid, UserType = userTypeId, error = EnumHelper.GetDescription(ErrorListEnum.Register_Other_Error) };
                    else
                    {
                        Logger.Info("User with ID " + userid + "created");
                        string preferences = CreateUserDefaultPreferences(userid);
                        if (preferences == "OK")
                        {
                            result.UserId = userid;
                            result.UserType = userTypeId;
                            result.error = "OK";
                        }
                        return result;
                    }
                }
                else
                {
                    return new AuthenticationResult() { UserId = -1, UserType = userTypeId, error = EnumHelper.GetDescription(ErrorListEnum.Register_User_Exists) }; ;
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error Register user {0}", email);
                Logger.Error(error_message, ex);
                return result;
            }
        }

        public string EditUserProfile(UserInfo userprofile)
        {
            try
            {
                string message = string.Format("Executing EditProfile for user {0}", userprofile.Email);
                Logger.Info(message);
                if (_userRepository.UserIsClient(userprofile.Email) && (string.IsNullOrEmpty(userprofile.Firstname) || string.IsNullOrEmpty(userprofile.Surname)))
                {
                    return EnumHelper.GetDescription(ErrorListEnum.Profile_ClientNameMissing);
                }
                else if (!_userRepository.UserIsClient(userprofile.Email) && string.IsNullOrEmpty(userprofile.CompanyName))
                {
                    return EnumHelper.GetDescription(ErrorListEnum.Profile_ClientNameMissing);
                }
                else
                {
                    return EnumHelper.GetDescription(_userRepository.EditUserProfile(userprofile));
                }
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error editing user {0}", userprofile.Email);
                Logger.Error(error_message, ex);
                return EnumHelper.GetDescription(ErrorListEnum.Profile_Other_Error);
            }
        }

        public UserInfo GetUserProfile(int userId)
        {
            try
            {
                string message = string.Format("Executing GetUserProfile for user {0}", userId.ToString());
                Logger.Info(message);

                return _userRepository.GetUserProfile(userId);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error executing GetUserProfile for user {0}", userId.ToString());
                Logger.Error(error_message, ex);
                return null;
            }
        }

        public List<ContractorInfo> GetListContractors(SearchContractor SearchParams)
        {
            try
            {
                string message = string.Format("Executing GetListContractors");
                Logger.Info(message);

                return _userRepository.GetListContractors(SearchParams);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error executing GetListContractors");
                Logger.Error(error_message, ex);
                return null;
            }
        }

        public string AddFavourite(int FromUser, int ToUser)
        {
            try
            {
                string message = string.Format("Executing AddFavourite(From {0}, To {1})", FromUser.ToString(), ToUser.ToString());
                Logger.Info(message);

                if (!_userRepository.UserIdExists(FromUser) || !_userRepository.UserIdExists(ToUser))
                {
                    return EnumHelper.GetDescription(ErrorListEnum.Favourite_UserNotExistError);
                }

                return _userRepository.AddFavourite(FromUser, ToUser);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error executing AddFavourite(From {0}, To {1})", FromUser.ToString(), ToUser.ToString());
                Logger.Error(error_message, ex);
                return EnumHelper.GetDescription(ErrorListEnum.Favourite_Error);
            }

        }

        public string RemoveFavourite(int FromUser, int ToUser)
        {
            try
            {
                string message = string.Format("Executing RemoveFavourite(From {0}, To {1})", FromUser.ToString(), ToUser.ToString());
                Logger.Info(message);

                if (!_userRepository.UserIdExists(FromUser) || !_userRepository.UserIdExists(ToUser))
                {
                    return EnumHelper.GetDescription(ErrorListEnum.Favourite_UserNotExistError);
                }

                return _userRepository.RemoveFavourite(FromUser, ToUser);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error executing RemoveFavourite(From {0}, To {1})", FromUser.ToString(), ToUser.ToString());
                Logger.Error(error_message, ex);
                return EnumHelper.GetDescription(ErrorListEnum.Favourite_Error);
            }

        }

        public List<ContractorInfo> GetUserFavourites(int FromUser)
        {
            try
            {
                string message = string.Format("Executing GetUserFavourites(From {0})", FromUser.ToString());
                Logger.Info(message);

                return _userRepository.GetUserFavourites(FromUser);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error executing GetUserFavourites(From {0}, To {1})", FromUser.ToString());
                Logger.Error(error_message, ex);
                return null;
            }
        }

        public bool UserIsFavourite(int fromuserID, int touserID)
        {
            string message = string.Format("Executing UserIsFavourite(From {0} to {1})", fromuserID.ToString(), touserID.ToString());
            Logger.Info(message);

            return _userRepository.UserIsFavourite(fromuserID,touserID);
        }

        public string AddDenunce(int FromUser, int ToUser, string Comment, bool BlockUser)
        {
            try
            {
                string message = string.Format("Executing AddDenunce(From {0}, To {1})", FromUser.ToString(), ToUser.ToString());
                Logger.Info(message);

                if (!_userRepository.UserIdExists(FromUser) || !_userRepository.UserIdExists(ToUser))
                {
                    return EnumHelper.GetDescription(ErrorListEnum.Denunce_UserNotExistError);
                }

                return _userRepository.AddDenunce(FromUser, ToUser, Comment, (int)DenunceStatusEnum.Open, BlockUser);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error executing AddFavourite(From {0}, To {1})", FromUser.ToString(), ToUser.ToString());
                Logger.Error(error_message, ex);
                return EnumHelper.GetDescription(ErrorListEnum.Favourite_Error);
            }

        }

        public Result BlockUser(int FromUser, int ToUser)
        { 
            string message = string.Format("Executing BlockUser(From {0}, To {1})", FromUser.ToString(), ToUser.ToString());
            Logger.Info(message);

            if (!_userRepository.UserIdExists(FromUser) || !_userRepository.UserIdExists(ToUser))
            {
                return new Result(EnumHelper.GetDescription(ErrorListEnum.Denunce_UserNotExistError), (int)ErrorListEnum.Denunce_UserNotExistError);
            }

            if (_userRepository.UserIsBlocked(FromUser, ToUser))
            {
                return _userRepository.UnBlockUser(FromUser, ToUser);
            }

            return _userRepository.BlockUser(FromUser, ToUser);
        }

        public bool UserIsBlocked(int FromUser, int ToUser)
        {
            return _userRepository.UserIsBlocked(FromUser, ToUser);
        }
        public double GetUserAverage(int UserID)
        {
            try
            {
                string message = string.Format("Executing GetUserAverage for user {0}", UserID.ToString());
                Logger.Info(message);

                return _userRepository.GetUserAverage(UserID);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error executing GetUserAverage for user {0}", UserID.ToString());
                Logger.Error(error_message, ex);
                return -1;
            }
        }

        public ResetPasswordResult ResetPassword(string email)
        {
            ResetPasswordResult result = new ResetPasswordResult();
            result.Email = email;
            try
            {
                string message = string.Format("Executing ResetPassword for user {0}", email);
                Logger.Info(message);

                if (_userRepository.UserExists(email))
                {
                    string temporalpassword = _userRepository.ResetPassword(email);

                    if (temporalpassword != "-1")
                    {
                        if (SendEmail(temporalpassword, email)) result.Result= EnumHelper.GetDescription(ErrorListEnum.OK);
                        else result.Result = EnumHelper.GetDescription(ErrorListEnum.Send_Email_Other_Error);
                    }
                    else
                    {
                        result.Result = EnumHelper.GetDescription(ErrorListEnum.Reset_Password_Other_Error);
                    }
                }
                result.Result = EnumHelper.GetDescription(ErrorListEnum.Reset_Password_UserNotExist);
                return result;
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error Reset Password for user {0}", email);
                Logger.Error(error_message, ex);
                result.Result = EnumHelper.GetDescription(ErrorListEnum.Reset_Password_Other_Error);
                return result;
            }
        }

        private bool SendEmail(string temporalpassword, string email)
        {
            try
            {
                string message = string.Format("Executing SendEmail to {0}", email);
                Logger.Info(message);

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("findmyhandyman@gmail.com");
                mail.To.Add(email);
                mail.Subject = "FindYourHandyMan Reset Password";
                mail.Body = "FindYourHandyMan recently received a request to reset your password. Your new temporal password is "
                    + temporalpassword + ". This password will expire in 24 hours, so please log into your FindYourHandyMan account and change your password";
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("findmyhandyman@gmail.com", "contractorshare");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error Send Email");
                Logger.Error(error_message, ex);
                return false;
            }
        }

        public PreferencesResult GetPreferences(string userId)
        {
            try
            {
                string message = string.Format("Executing GetPreferences for user {0}", userId);
                Logger.Info(message);

                return _userRepository.GetPreferences(Convert.ToInt32(userId));
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error executing GetPreferences for user {0}", userId);
                Logger.Error(error_message, ex);
                return null;
            }
        }

        public string ChangePreferences(string userId, ChangePreferencesInfo changepreferencesinfo)
        {
            try
            {
                string message = string.Format("Executing ChangePreferences for user {0}", userId);
                Logger.Info(message);

                return _userRepository.ChangePreferences(userId, changepreferencesinfo);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error when changing Preferences for user {0}", userId);
                Logger.Error(error_message, ex);
                return ex.ToString();
            }
        }

        public string ChangePassword(ChangePasswordInfo changepasswordinfo)
        {
            try
            {
                string message = string.Format("Executing ChangePassword for user {0}", changepasswordinfo.email);
                Logger.Info(message);

                int userid = _userRepository.Authenticate(changepasswordinfo.email, changepasswordinfo.OldPassword).UserId;

                if (userid > 0)
                {
                    return _userRepository.ChangePassword(changepasswordinfo.email, changepasswordinfo.NewPassword);
                }
                else
                {
                    return EnumHelper.GetDescription(ErrorListEnum.Change_Password_IncorrectOldPassword);
                }
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error Change Password for user {0}", changepasswordinfo.email);
                Logger.Error(error_message, ex);
                return ex.ToString();
            }
        }

        public String CreateUserDefaultPreferences(int userId)
        {
             try
            {
                string message = string.Format("Executing CreateUserDefaultSettings for user {0}", userId.ToString());
                Logger.Info(message);

                //set default Preferences to all true
                ChangePreferencesInfo defaultpreferences = new ChangePreferencesInfo();
                defaultpreferences.enableNotifications = true;
                defaultpreferences.shareLocation = true;
                defaultpreferences.showContactNumber = true;
                defaultpreferences.showContactEmail = true;

                return _userRepository.CreateUserPreferences(userId, defaultpreferences);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error when creating Preferences for user {0}", userId.ToString());
                Logger.Error(error_message, ex);
                return ex.ToString();
            }
        }

        public Result AddSuggestion(int FromUser, string comment)
        {
            string message = string.Format("Executing AddSuggestion(From {0})", FromUser.ToString());
            Logger.Info(message);

            return _userRepository.AddSuggestion(FromUser, comment);
        }

        public Result AddBug(int FromUser, string comment)
        {
            string message = string.Format("Executing AddBug(From {0})", FromUser.ToString());
            Logger.Info(message);

            return _userRepository.Addbug(FromUser, comment);
        }

        public List<IssueInfo> ListSuggestions(int userId)
        {
            string message = string.Format("Executing ListSuggestions(For {0})", userId.ToString());
            Logger.Info(message);

            return _userRepository.ListSuggestions(userId);
        }

        public List<IssueInfo> ListBugs(int userId)
        {
            string message = string.Format("Executing ListBugs(For {0})", userId.ToString());
            Logger.Info(message);

            return _userRepository.ListBugs(userId);
        }


    }
}