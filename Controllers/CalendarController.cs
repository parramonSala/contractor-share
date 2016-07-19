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
    public class CalendarController
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(TaskController));
        private CalendarRepository _calendarRepository = new CalendarRepository();


        public Result CreateEvent(EventInfo eventinfo)
        {
            string message = string.Format("Executing Create Event");
            Logger.Info(message);

            return _calendarRepository.CreateEvent(eventinfo);
        }

        public Result EditEvent(int eventId, EventInfo eventinfo)
        {
            string message = string.Format("Executing Edit event {0}", eventId.ToString());
            Logger.Info(message);

            return _calendarRepository.EditEvent(eventId, eventinfo);

        }

        public Result DeleteEvent(int eventId)
        {

            string message = string.Format("Executing DeleteEvent {0}", eventId.ToString());
            Logger.Info(message);

            return _calendarRepository.DeleteEvent(eventId);

        }

        public List<EventInfo> GetUserEvents(int userId)
        {
            return _calendarRepository.GetUserEvents(userId);
        }
    
    }
}