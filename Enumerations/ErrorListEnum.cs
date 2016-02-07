using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace ContractorShareService.Enumerations
{
    public enum ErrorListEnum
    {
        //-------USER ERRORS----------
        //Login Errors
        [Description("OK")]
        OK = 0,
        [Description("Client doesn't exist")]
        Login_Client_NoExists = -1,
        [Description("Password has expired")]
        Login_Password_Expired = -2,
        [Description("Incorrect password")]
        Login_Incorrect_Password = -3,
        [Description("Not controlled login error")]
        Login_Other_Error = -4,

        //Register Errors
        [Description("Not controlled register error")]
        Register_Other_Error = -5,
        [Description("Register: error creating user")]
        Register_User_Error = -6,
        [Description("Register: error user already exists")]
        Register_User_Exists = -7,

        //Reset Password Errors
        [Description("Not controlled reset password error")]
        Reset_Password_Other_Error = -30,
        [Description("Reset Password: error user doesn't exist")]
        Reset_Password_UserNotExist = -31,

        //Change Password Errors
        [Description("Change Password: Incorrect current password")]
        Change_Password_IncorrectOldPassword = -33,

        //Profile Errors
        [Description("Client firstname or surname missing")]
        Profile_ClientNameMissing = -8,
        [Description("Contractor company name missing")]
        Profile_CompanyNameMissing = -9,
        [Description("Edit profile: other error")]
        Profile_Other_Error = -10,

        //SERVICE ERRORS
        [Description("Create service error")]
        Service_Create_Error = -11,
        [Description("Edit service error")]
        Service_Edit_Error = -12,
        [Description("Close service error")]
        Service_Close_Error = -13,

        //TASK ERRORS
        [Description("Create task error")]
        Task_Create_Error = -14,
        [Description("Edit task error")]
        Task_Edit_Error = -15,
        [Description("Close task error")]
        Task_Close_Error = -16,

        //SEARCH ERRORS
        [Description("Search Contractor error")]
        Search_Contractor = -17,

        //USER ERRORS
        [Description("AddFavourite: One of the user doesnt exist")]
        Favourite_UserNotExistError = -18,
        [Description("Add favourite other error")]
        Favourite_Error = -19,
        
        [Description("AddDenunce: One of the user doesnt exist")]
        Denunce_UserNotExistError = -20,
        [Description("Add denunce other error")]
        Denunce_Error = -21,
        [Description("BlockUser: There's no denunce")]
        UserDenunceNotExists = -26,
        [Description("BlockUser: User is already blocked")]
        UserAlreadyBlocked = -27,
        [Description("BlockUser: Other Error")]
        BlockUserOtherError = -28,

        //STATUS ERROR
        [Description("GetStatusID other error")]
        Status_Error = -22,
        //RATE ERROR
        [Description("Rate user or service doesnt exist")]
        Rate_NonExistError = -23,
        [Description("Rate user or service doesnt exist")]
        Rate_OtherError = -24,
        [Description("Error when updating user's AverageRate")]
        Rate_UpdateUserAverageRate = -29,

        //COMMENTS ERROR
        [Description("Not controled Comment Error")]
        Comment_AddError = -25,

        //Send Email
        [Description("Not controled Send Email Error")]
        Send_Email_Other_Error = -32

    }
}