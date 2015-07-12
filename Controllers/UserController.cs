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
    public class UserController
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(UserController));
        private UserRepository _userRepository = new UserRepository();
        private StatusRepository _statusRepository = new StatusRepository();

        public string Login(string email, string password, int TypeOfUser)
        {
            try
            {
                string message = string.Format("Executing Login for user {0}", email);
                Logger.Info(message);

                bool exists = _userRepository.UserExists(email, TypeOfUser);

                if (exists)
                {
                    return _userRepository.Authenticate(email, password);
                }
                else
                {
                    string error_message = string.Format("Error Login: user with mail {0} and type {1} doesn't exist the DB", email, Enum.GetName(typeof(ModelEnum), TypeOfUser));
                    Logger.Error(error_message);
                    return EnumHelper.GetDescription(ErrorListEnum.Login_Client_NoExists);
                }
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Login Error for user {0}, Exception: {1}", email, ex.InnerException.Message ?? ex.Message);
                Logger.ErrorFormat(error_message);
                return EnumHelper.GetDescription(ErrorListEnum.Login_Other_Error);
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
                Logger.ErrorFormat("Error: {0}, exception: {1}", error_message, ex);
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
                Logger.ErrorFormat("Error: {0}, exception: {1}", error_message, ex);
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
                Logger.ErrorFormat("Error: {0}, exception: {1}", error_message, ex);
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
                Logger.ErrorFormat("Error: {0}, exception: {1}", error_message, ex);
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

                return _userRepository.AddFavourite(FromUser,ToUser);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error executing AddFavourite(From {0}, To {1})", FromUser.ToString(), ToUser.ToString());
                Logger.ErrorFormat("Error: {0}, exception: {1}", error_message, ex);
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
                Logger.ErrorFormat("Error: {0}, exception: {1}", error_message, ex);
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
                Logger.ErrorFormat("Error: {0}, exception: {1}", error_message, ex);
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
                Logger.ErrorFormat("Error: {0}, exception: {1}", error_message, ex);
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

                if (!_userRepository.UserDenunceExists(FromUser,ToUser))
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
                Logger.ErrorFormat("Error: {0}, exception: {1}", error_message, ex);
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
                Logger.ErrorFormat("Error: {0}, exception: {1}", error_message, ex);
                return -1;
            }
        }
    }
}