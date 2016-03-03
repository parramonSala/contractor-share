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
    public class AppointmentController
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(AppointmentController));
        private AppointmentRepository _appointmentRepository = new AppointmentRepository();
        private UserController _userController = new UserController();

        public string Create(AppointmentInfo appointment)
        {
            try
            {
                string message = string.Format("Executing Create Appointment");
                Logger.Info(message);

                return _appointmentRepository.CreateAppointment(appointment);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error Creating Appointment");
                Logger.Error(error_message, ex);
                return ex.ToString();
            }

        }

        public List<AppointmentInfo> GetActiveAppointments(int UserId)
        {
            string message = string.Format("Executing GetActiveAppointments");
            Logger.Info(message);

            return _appointmentRepository.GetActiveAppointments(UserId);
        }

        public AppointmentInfo GetAppointment(int appointmentId)
        {
            string message = string.Format("Executing GetAppointment");
            Logger.Info(message);

            return _appointmentRepository.GetAppointment(appointmentId);
        }

        public Result ChangeAppointmentStatus(int appointmentId, int statusId)
        {
            string message = string.Format("Executing ChangeAppointmentStatus");
            Logger.Info(message);

            return _appointmentRepository.ChangeAppointmentStatus(appointmentId, statusId);
        }

        public Result CancelAppointment(int appointmentId)
        {
            string message = string.Format("Executing ChangeAppointmentStatus");
            Logger.Info(message);

            if (ChangeAppointmentStatus(appointmentId, (int)AppointmentStatusEnum.Cancelled).resultCode == (int)ErrorListEnum.OK)
            {
                //Notifications need to be sent when an appointment is cancelled
                AppointmentInfo appointment = GetAppointment(appointmentId);

                string subject = "FindYourHandyMan: An appointment has been cancelled";
                string body = string.Concat("Your appointment for ", appointment.MeetingTime, " has been cancelled.");

                if (SendEmail(subject, body, appointment.ContractorId) && SendEmail(subject, body, appointment.ClientId))
                {
                    return new Result();
                }
            }

            return new Result(ErrorListEnum.Appointment_Other_Error.ToString(), (int)ErrorListEnum.Appointment_Other_Error);
        }

        public Result CloseAppointment(int appointmentId)
        {
            string message = string.Format("Executing CloseAppointment");
            Logger.Info(message);

            return ChangeAppointmentStatus(appointmentId, (int)AppointmentStatusEnum.Closed);
        }

        private bool SendEmail(string subject, string body, int userId)
        {
            try
            {
                string message = string.Format("Executing SendEmail");
                Logger.Info(message);


                string emailaddress = _userController.GetUserProfile(userId).Email;

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("findmyhandyman@gmail.com");
                mail.To.Add(emailaddress);
                mail.Subject = subject;
                mail.Body = body;
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