using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using ContractorShareService.Enumerations;
using ContractorShareService.Domain;

namespace ContractorShareService.Repositories
{
    public class TaskRepository
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(ServiceRepository));
        private ContractorShareEntities db = new ContractorShareEntities();

        public int CreateTask(TaskInfo taskRequest)
        {
            try
            {
                Task newTask = new Task()
                {
                    Name = taskRequest.Name,
                    Description = taskRequest.Description,
                    ServiceID = taskRequest.ServiceId,
                    StatusID = (int)TaskStatusEnum.Open
                };

                db.Task.Add(newTask);
                db.SaveChanges();
                
                int id = newTask.ID;
                Logger.Info(String.Format("TaskRepository.CreateTaskx: created task with ID {0}", id));
                return id;
            }
            catch (Exception ex)
            {
                Logger.Error("Error TaskRepository.CreateTask", ex);
                return (int)(ErrorListEnum.Service_Create_Error);
                throw new Exception("Error TaskRepository.CreateTask: Couldn't create the task", ex);
            }
        }

        public string EditTask(int taskId, TaskInfo taskRequest)
        {
            try
            {
                var task = (from t in db.Task
                            where t.ID == taskId
                            select t).FirstOrDefault();

                task.Name = taskRequest.Name;
                task.Description = taskRequest.Description;
                task.ServiceID = taskRequest.ServiceId;

                db.SaveChanges();
                return EnumHelper.GetDescription(ErrorListEnum.OK);
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error TaskRepository.EditTask {0}: {1}", taskId.ToString(), ex);
                return EnumHelper.GetDescription(ErrorListEnum.Task_Edit_Error);
            }
        }

        public TaskInfo GetTask(int taskId)
        {
            try
            {
                var task = (from t in db.Task
                            where t.ID == taskId
                            select t).FirstOrDefault();

                TaskInfo taskInfo = new TaskInfo();

                taskInfo.Name = task.Name;
                taskInfo.Description = task.Description;
                taskInfo.ServiceId = task.ServiceID;

                return taskInfo;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("ServiceRepository.GetServiceInfo {0}: {1}", taskId.ToString(), ex);
                return null;
            }
        }

        public string CloseTask(int ServiceId)
        {
            try
            {
                var services = from service in db.Service
                               where service.ID == ServiceId
                               select service;

                Service selectedService = services.FirstOrDefault();
                selectedService.StatusID = (int)ServiceStatusEnum.Closed;

                db.SaveChanges();
                return EnumHelper.GetDescription(ErrorListEnum.OK);
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error ServiceRepository.CloseService {0}: {1}", ServiceId.ToString(), ex);
                return EnumHelper.GetDescription(ErrorListEnum.Service_Close_Error);
            }
        }
    }
}