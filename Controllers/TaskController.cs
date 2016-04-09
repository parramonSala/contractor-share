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


        public Result CreateTask(TaskInfo taskinfo)
        {
            string message = string.Format("Executing Create Task");
            Logger.Info(message);

            return _taskRepository.CreateTask(taskinfo);
        }

        public Result EditTask(int taskId, TaskInfo taskRequest)
        { 
            string message = string.Format("Executing Edit TaskRequest {0}", taskId.ToString());
            Logger.Info(message);

            return _taskRepository.EditTask(taskId, taskRequest);

        }

        public TaskInfo GetTask(int taskId)
        {
            
            string message = string.Format("Executing Get Task {0}", taskId.ToString());
            Logger.Info(message);

            return _taskRepository.GetTask(taskId);

        }

        public Result ChangeTaskStatus(int taskId, int StatusId)
        {
            string message = string.Format("Executing Close TaskR equest {0}", taskId.ToString());
            Logger.Info(message);

            return _taskRepository.ChangeTaskStatus(taskId, StatusId);
        }

        public List<TaskInfo> GetJobTasks(int jobId)
        {
            return _taskRepository.GetJobTasks(jobId);
        }
    
    }
}