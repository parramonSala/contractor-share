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
    public class TaskController
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(TaskController));
        private TaskRepository _taskRepository = new TaskRepository();


        public string CreateTask(string name, string description, int serviceId)
        {
            try
            {
                string message = string.Format("Executing Create TaskRequest");
                Logger.Info(message);

                TaskInfo taskInfo = new TaskInfo();
                taskInfo.Name = name;
                taskInfo.Description = description;
                taskInfo.ServiceId = serviceId;

                int id = _taskRepository.CreateTask(taskInfo);
                if (id < 0) return EnumHelper.GetDescription(ErrorListEnum.Task_Create_Error);
                else return id.ToString();
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error Creating TaskRequest");
                Logger.Error(error_message, ex);
                return EnumHelper.GetDescription(ErrorListEnum.Task_Create_Error);
            }

        }

        public string EditTask(int taskId, TaskInfo taskRequest)
        {
            try
            {
                string message = string.Format("Executing Edit TaskRequest {0}", taskId.ToString());
                Logger.Info(message);

                return _taskRepository.EditTask(taskId, taskRequest);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error Editing TaskRequest {0}", taskId.ToString());
                Logger.Error(error_message, ex);
                return EnumHelper.GetDescription(ErrorListEnum.Task_Edit_Error);
            }
        }

        public TaskInfo GetTask(int taskId)
        {
            try
            {
                string message = string.Format("Executing Get Task Request {0}", taskId.ToString());
                Logger.Info(message);

                return _taskRepository.GetTask(taskId);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error Getting Task Request {0}", taskId.ToString());
                Logger.Error(error_message, ex);
                return null;
            }
        }

        public string CloseTask(int taskId)
        {
            try
            {
                string message = string.Format("Executing Close TaskR equest {0}", taskId.ToString());
                Logger.Info(message);

                return _taskRepository.CloseTask(taskId);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error CloseTaskRequest {0}", taskId.ToString());
                Logger.Error(error_message, ex);
                return EnumHelper.GetDescription(ErrorListEnum.Task_Close_Error);
            }
        }
    }
}