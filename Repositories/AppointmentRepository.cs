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

        public List<AppointmentInfo> GetActiveAppointments(int UserId)
        {
            try
            {
                var appointments = from appointment in db.Appointments
                                   where (appointment.ClientID == UserId || appointment.ContractorID == UserId)
                                   && appointment.Active == true
                                   select appointment;

                List<AppointmentInfo> appointmentsinfolist = new List<AppointmentInfo>();

                foreach (var selectedappointment in appointments)
                {
                    AppointmentInfo appointmentinfo = new AppointmentInfo();

                    appointmentinfo.Active = selectedappointment.Active;
                    appointmentinfo.AproxDuration = selectedappointment.Duration;
                    appointmentinfo.ClientId = selectedappointment.ClientID;
                    appointmentinfo.ContractorId = selectedappointment.ContractorID;
                    appointmentinfo.JobId = selectedappointment.ServiceID;
                    appointmentinfo.ProposalId = selectedappointment.ProposalId;
                    appointmentinfo.StatusId = selectedappointment.StatusID;
                    appointmentinfo.MeetingTime = selectedappointment.MeetingTime;
                    appointmentinfo.LocationCoordX = selectedappointment.CoordX;
                    appointmentinfo.LocationCoordY = selectedappointment.CoordY;
                    appointmentinfo.AppointmentId = selectedappointment.ID;

                    appointmentsinfolist.Add(appointmentinfo);
                }

                return appointmentsinfolist;
            }
            catch (Exception ex)
            {
                Logger.Error("Error AppointmentRepository.GetActiveAppointments", ex);
                return null;
            }
        }

        public AppointmentInfo GetAppointment(int AppointmentId)
        {
            try
            {
                var appointment = (from app in db.Appointments
                                   where app.ID == AppointmentId
                                   select app).FirstOrDefault();
                
                AppointmentInfo appointmentinfo = new AppointmentInfo();

                appointmentinfo.Active = appointment.Active;
                appointmentinfo.AproxDuration = appointment.Duration;
                appointmentinfo.ClientId = appointment.ClientID;
                appointmentinfo.ContractorId = appointment.ContractorID;
                appointmentinfo.JobId = appointment.ServiceID;
                appointmentinfo.ProposalId = appointment.ProposalId;
                appointmentinfo.StatusId = appointment.StatusID;
                appointmentinfo.MeetingTime = appointment.MeetingTime;
                appointmentinfo.LocationCoordX = appointment.CoordX;
                appointmentinfo.LocationCoordY = appointment.CoordY;
                appointmentinfo.AppointmentId = appointment.ID;

                return appointmentinfo;
            }
            catch (Exception ex)
            {
                Logger.Error("Error AppointmentRepository.GetAppointment", ex);
                return null;
            }

        }

        public Result ChangeAppointmentStatus(int appointmentId, int statusId)
        {
            try
            {
                var selectedappointment = (from appointment in db.Appointments
                                           where appointment.ID == appointmentId
                                           select appointment).FirstOrDefault();

                if (selectedappointment.StatusID != statusId)
                {
                    selectedappointment.StatusID = statusId;

                    if ((statusId == (int)AppointmentStatusEnum.Cancelled || statusId == (int)AppointmentStatusEnum.Cancelled) && selectedappointment.Active)
                    {
                        selectedappointment.Active = false;
                    }

                    db.SaveChanges();
                }

                return new Result();
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error AppointmentRepository.ChangeAppointmentStatus {0}: {1}", appointmentId.ToString(), ex);
                return new Result(ex.ToString(), (int)ErrorListEnum.Appointment_Other_Error);
            }
        }
    }
}