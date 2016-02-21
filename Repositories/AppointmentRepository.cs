using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using ContractorShareService.Enumerations;
using ContractorShareService.Domain;

namespace ContractorShareService.Repositories
{
    public class AppointmentRepository
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(ServiceRepository));
        private ContractorShareEntities db = new ContractorShareEntities();

        public string CreateAppointment(AppointmentInfo appointment)
        {
            try
            {
                int statusid = (int)AppointmentStatusEnum.Open;

                Appointment newappointment = new Appointment()
                {
                    ServiceID = appointment.JobId,
                    ProposalId = appointment.ProposalId,
                    ClientID = appointment.ClientId,
                    ContractorID = appointment.ContractorId,
                    Active = true,
                    StatusID = statusid,
                    Duration = appointment.AproxDuration,
                    MeetingTime = appointment.MeetingTime,
                    CoordX = appointment.LocationCoordX,
                    CoordY = appointment.LocationCoordY
                };

                db.Appointments.Add(newappointment);
                db.SaveChanges();

                Logger.Info(String.Format("ProposalRepository.CreateAppointment: created appointment"));

                return EnumHelper.GetDescription(ErrorListEnum.OK);
            }
            catch (Exception ex)
            {
                Logger.Error("Error ProposalRepository.CreateAppointment", ex);
                return ex.ToString();
            }
        }

        
    }
}