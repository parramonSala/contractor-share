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

        public Result CreateTask(TaskInfo taskRequest)
        {
            try
            {
                DateTime currenttime = DateTime.Now;

                Task newTask = new Task()
                {
                    Name = taskRequest.Name,
                    Description = taskRequest.Description,
                    ServiceID = taskRequest.ServiceId,
                    StatusID = (int)TaskStatusEnum.Open,
                    Created = currenttime
                };

                db.Tasks.Add(newTask);
                db.SaveChanges();
                
                int id = newTask.ID;
                Logger.Info(String.Format("TaskRepository.CreateTaskx: created task with ID {0}", id));
                return new Result();
            }
            catch (Exception ex)
            {
                Logger.Error("Error TaskRepository.CreateTask", ex);
                return new Result(ex.ToString(), (int)(ErrorListEnum.Task_Create_Error));
            }
        }

        public Result EditTask(int taskId, TaskInfo taskRequest)
        {
            try
            {
                var task = (from t in db.Tasks
                            where t.ID == taskId
                            select t).FirstOrDefault();

                task.Name = taskRequest.Name;
                task.Description = taskRequest.Description;
                task.StatusID = taskRequest.StatusId;

                db.SaveChanges();
                return new Result();
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error TaskRepository.EditTask {0}: {1}", taskId.ToString(), ex.ToString());
                return new Result(ex.ToString(), (int)(ErrorListEnum.Task_Edit_Error));
            }
        }

        public TaskInfo GetTask(int taskId)
        {
            try
            {
                var task = (from t in db.Tasks
                            where t.ID == taskId
                            select t).FirstOrDefault();

                TaskInfo taskInfo = new TaskInfo();

                taskInfo.TaskId = task.ID;
                taskInfo.Name = task.Name;
                taskInfo.Description = task.Description;
                taskInfo.ServiceId = task.ServiceID;
                taskInfo.StatusId = task.StatusID;
                taskInfo.Created = task.Created;

                return taskInfo;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("ServiceRepository.GetTask {0}: {1}", taskId.ToString(), ex.ToString());
                return null;
            }
        }

        public Result ChangeTaskStatus(int taskId, int StatusId)
        {
            try
            {
                var task = (from t in db.Tasks
                            where t.ID == taskId
                            select t).FirstOrDefault();

                task.StatusID = StatusId;

                db.SaveChanges();
                return new Result();
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error TaskRepository.ChangeTaskStatus {0}: {1}", taskId.ToString(), ex.ToString());
                return new Result(ex.ToString(), (int)(ErrorListEnum.Task_Status_Error));
            }
        }

        public Result DeleteTask(int taskId)
        {
            try
            {
                Task task = (from t in db.Tasks
                                      where t.ID == taskId
                                      select t).First();

                db.Tasks.Remove(task);
                db.SaveChanges();

                Logger.Info(String.Format("TaskRepository.DeleteTask"));

                return new Result();
            }
            catch (Exception ex)
            {
                Logger.Error("Error TaskRepository.DeleteTask", ex);
                return new Result(ex.ToString(), (int)(ErrorListEnum.Task_Delete_Error));
            }
        }

        public List<TaskInfo> GetJobTasks(int jobId)
        {
            try
            {
                List<Task> jobtasks = (from task in db.Tasks
                                                 where task.ServiceID == jobId
                                                 select task).ToList();

                List<TaskInfo> taskinfolist = new List<TaskInfo>();

                foreach (var t in jobtasks)
                {
                    TaskInfo taskinfo = new TaskInfo();

                    taskinfo.TaskId = t.ID;
                    taskinfo.Name = t.Name;
                    taskinfo.Description = t.Description;
                    taskinfo.ServiceId = t.ServiceID;
                    taskinfo.StatusId = t.StatusID;
                    taskinfo.Created = t.Created;

                    taskinfolist.Add(taskinfo);
                }

                return taskinfolist;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("TaskRepository.GetJobTasks {0}: {1}", jobId.ToString(), ex.ToString());
                return null;
            }
        }
    
    }
}