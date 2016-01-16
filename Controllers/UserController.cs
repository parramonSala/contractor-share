using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using ContractorShareService.Repositories;
using ContractorShareService.Enumerations;
using ContractorShareService.Domain;
using System.Net.Mail;

namespace ContractorShareService.Controllers
{
    public class UserController
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(UserController));
        private UserRepository _userRepository = new UserRepository();
        private StatusRepository _statusRepository = new StatusRepository();

        public LoginResult Login(string email, string password)
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
                    return new LoginResult(EnumHelper.GetDescription(ErrorListEnum.Login_Client_NoExists));
                }
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error Login user {0}", email);
                Logger.Error(error_message, ex);
                return new LoginResult(EnumHelper.GetDescription(ErrorListEnum.Login_Other_Error));
            }
        }

        public string Register(string email, string password, int TypeOfUser)
        {
            try
            {
                string message = string.Format("Executing Register for user {0}", email);
                Logger.Info(message);

                if (!_userRepository.UserExists(email, TypeOfUser))
                {
                    int userid = _userRepository.CreateUser(email, password, TypeOfUser);

                    if (userid < 0) return EnumHelper.GetDescription(ErrorListEnum.Register_Other_Error);
                    else
                    {
                        Logger.Info("User with ID " + userid + "created");
                        return EnumHelper.GetDescription(ErrorListEnum.OK);
                    }
                }
                else return EnumHelper.GetDescription(ErrorListEnum.Register_User_Exists);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error Register user {0}", email);
                Logger.Error(error_message, ex);
                return EnumHelper.GetDescription(ErrorListEnum.Register_Other_Error);
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

        public List<GetListContractors_Result> GetListContractors(SearchContractor SearchParams)
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

        public List<UserFavourite> GetUserFavourites(int FromUser)
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

                int statusid = _statusRepository.GetStatusId("NEW");

                if (_statusRepository.GetStatusId("NEW") < 0)
                {
                    return EnumHelper.GetDescription(ErrorListEnum.Status_Error);
                }

                return _userRepository.AddDenunce(FromUser, ToUser, Comment, statusid, BlockUser);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error executing AddFavourite(From {0}, To {1})", FromUser.ToString(), ToUser.ToString());
                Logger.Error(error_message, ex);
                return EnumHelper.GetDescription(ErrorListEnum.Favourite_Error);
            }

        }

        public string BlockUser(int FromUser, int ToUser)
        {
            try
            {
                string message = string.Format("Executing BlockUser(From {0}, To {1})", FromUser.ToString(), ToUser.ToString());
                Logger.Info(message);

                if (!_userRepository.UserIdExists(FromUser) || !_userRepository.UserIdExists(ToUser))
                {
                    return EnumHelper.GetDescription(ErrorListEnum.Denunce_UserNotExistError);
                }

                if (!_userRepository.UserDenunceExists(FromUser, ToUser))
                {
                    return EnumHelper.GetDescription(ErrorListEnum.UserDenunceNotExists);
                }

                if (_userRepository.UserIsBlocked(FromUser, ToUser))
                {
                    return EnumHelper.GetDescription(ErrorListEnum.UserAlreadyBlocked);
                }

                return _userRepository.BlockUser(FromUser, ToUser);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error executing AddFavourite(From {0}, To {1})", FromUser.ToString(), ToUser.ToString());
                Logger.Error(error_message, ex);
                return EnumHelper.GetDescription(ErrorListEnum.Favourite_Error);
            }
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

        public string ResetPassword(string email)
        {
            try
            {
                string message = string.Format("Executing ResetPassword for user {0}", email);
                Logger.Info(message);

                if (_userRepository.UserExists(email))
                {
                    string temporalpassword = _userRepository.ResetPassword(email);

                    if (temporalpassword != "-1")
                    {
                        if (SendEmail(temporalpassword, email)) return EnumHelper.GetDescription(ErrorListEnum.OK);
                        else return EnumHelper.GetDescription(ErrorListEnum.Send_Email_Other_Error);
                    }
                    else
                    {
                        return EnumHelper.GetDescription(ErrorListEnum.Reset_Password_Other_Error);
                    }
                }
                else return EnumHelper.GetDescription(ErrorListEnum.Reset_Password_UserNotExist);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error Reset Password for user {0}", email);
                Logger.Error(error_message, ex);
                return EnumHelper.GetDescription(ErrorListEnum.Reset_Password_Other_Error);
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
                mail.Subject = "ContractorShare Reset Password";
                mail.Body = "ContractorShare recently received a request to reset your password. Your new temporal password is "
                    + temporalpassword + ". This password will expire in 24 hours, so please log into your ContractorShare account and change your password";
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
    }
}