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


        public string Create(TaskInfo taskRequest)
        {
            try
            {
                string message = string.Format("Executing Create TaskRequest");
                Logger.Info(message);

                int id = _taskRepository.CreateTask(taskRequest);

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

        public string EditTaskInfo(int taskId, TaskInfo taskRequest)
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

        public TaskInfo GetTaskInfo(int taskId)
        {
            try
            {
                string message = string.Format("Executing Get TaskInfo {0}", taskId.ToString());
                Logger.Info(message);

                return _taskRepository.GetTaskInfo(taskId);
            }
            catch (Exception ex)
            {
                string error_message = string.Format("Error Getting TaskRequest {0}", taskId.ToString());
                Logger.Error(error_message, ex);
                return null;
            }
        }

        public string CloseTask(int taskId)
        {
            try
            {
                string message = string.Format("Executing CloseTaskRequest {0}", taskId.ToString());
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