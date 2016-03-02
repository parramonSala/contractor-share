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
    public class AppointmentController
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(AppointmentController));
        private AppointmentRepository _appointmentRepository = new AppointmentRepository();

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
    }
}