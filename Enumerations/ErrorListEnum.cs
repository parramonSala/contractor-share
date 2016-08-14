using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace ContractorShareService.Enumerations
{
    //NEXT ID 48

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
        [Description("Pay service error")]
        Service_Pay_Error = -47,

        //TASK ERRORS
        [Description("Create task error")]
        Task_Create_Error = -14,
        [Description("Edit task error")]
        Task_Edit_Error = -15,
        [Description("Change task status error")]
        Task_Status_Error = -16,
        [Description("Delete task error")]
        Task_Delete_Error=-37,

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
        [Description("Error when deleting a Rate")]
        Rate_DeleteError = -38,

        //COMMENTS ERRORS
        [Description("Not controled Comment Error")]
        Comment_AddError = -25,

        [Description("Not controled Delete Comment Error")]
        Comment_DeleteError = -35,

        //Send Email
        [Description("Not controled Send Email Error")]
        Send_Email_Other_Error = -32,

        //Appointments Errors
        [Description("Not controled Appointment Error")]
        Appointment_Other_Error = -34,

        //Mesagge Errors
        [Description("Not controled Delete Message Error")]
        DeleteMessage_Other_Error = -36,

        [Description("Not controled Create Message Error")]
        CreateMessage_Other_Error = -40,

        //Proposal Errors
        [Description("Couldnt update proposal error")]
        Proposal_Update_Error = -37,

        [Description("Couldnt create proposal error")]
        Proposal_Create_Error = -39,

        //FAQS Errors
        [Description("Add Suggestion error")]
        Suggestion_Other_Error = -41,

        [Description("Add bug error")]
        Bug_Other_Error = -42,

        //Calendar Errors
        [Description("Create event error")]
        Create_Event_Error = -43,

        [Description("Edit event error")]
        Edit_Event_Error = -44,

        [Description("List events error")]
        List_Events_Error = -45,

        [Description("Delete event error")]
        Delete_Event_Error = -45,
    }
}