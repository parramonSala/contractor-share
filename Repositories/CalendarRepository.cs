using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using ContractorShareService.Enumerations;
using ContractorShareService.Domain;

namespace ContractorShareService.Repositories
{
    public class CalendarRepository
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(ServiceRepository));
        private ContractorShareEntities db = new ContractorShareEntities();

        public Result CreateEvent(EventInfo eventinfo)
        {
            try
            {
                Event newEvent = new Event()
                {
                    EventName = eventinfo.Name,
                    UserID = eventinfo.UserId,
                    AppointmentID = eventinfo.AppointmentId,
                    Start_Date = eventinfo.Start_Date,
                    End_Date = eventinfo.End_Date
                };

                db.Events.Add(newEvent);
                db.SaveChanges();

                int id = newEvent.ID;
                Logger.Info(String.Format("CalendarRepository.CreateEvent: created event with ID {0}", id));
                return new Result();
            }
            catch (Exception ex)
            {
                Logger.Error("Error CalendarRepository.CreateEvent", ex);
                return new Result(ex.ToString(), (int)(ErrorListEnum.Create_Event_Error));
            }
        }

        public Result EditEvent(int eventId, EventInfo eventinfo)
        {
            try
            {
                var calendarevent = (from e in db.Events
                                     where e.ID == eventId
                                    select e).FirstOrDefault();

                calendarevent.EventName = eventinfo.Name;
                calendarevent.UserID = eventinfo.UserId;
                calendarevent.AppointmentID = eventinfo.AppointmentId;
                calendarevent.Start_Date = eventinfo.Start_Date;
                calendarevent.End_Date = eventinfo.End_Date;

                db.SaveChanges();
                return new Result();
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error CalendarRepository.EditEvent {0}: {1}", eventId.ToString(), ex.ToString());
                return new Result(ex.ToString(), (int)(ErrorListEnum.Edit_Event_Error));
            }
        }

        public Result DeleteEvent(int eventId)
        {
            try
            {
                Event calendarevent = (from e in db.Events
                                       where e.ID == eventId
                                      select e).First();

                db.Events.Remove(calendarevent);
                db.SaveChanges();

                Logger.Info(String.Format("CalendarRepository.DeleteEvent"));

                return new Result();
            }
            catch (Exception ex)
            {
                Logger.Error("Error CalendarRepository.DeleteEvent", ex);
                return new Result(ex.ToString(), (int)(ErrorListEnum.Delete_Event_Error));
            }
        }

        public List<EventInfo> GetUserEvents(int userId)
        {
            try
            {
                List<Event> userevents = (from e in db.Events
                                          where e.UserID == userId
                                          select e).ToList();

                List<EventInfo> eventlist = new List<EventInfo>();

                foreach (var e in userevents)
                {
                    EventInfo eventinfo = new EventInfo();

                    eventinfo.EventId = e.ID;
                    eventinfo.Name = e.EventName;
                    eventinfo.UserId = e.UserID;
                    eventinfo.AppointmentId = e.AppointmentID;
                    eventinfo.Start_Date = e.Start_Date;
                    eventinfo.End_Date = e.End_Date;

                    eventlist.Add(eventinfo);
                }

                return eventlist;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("CalendarRepository.GetUserEvents {0}: {1}", userId.ToString(), ex.ToString());
                return null;
            }
        }
    
    }
}