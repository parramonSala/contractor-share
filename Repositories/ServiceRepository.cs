using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using ContractorShareService.Enumerations;
using ContractorShareService.Domain;

namespace ContractorShareService.Repositories
{
    public class ServiceRepository
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(ServiceRepository));
        private ContractorShareEntities db = new ContractorShareEntities();

        public string CreateService(JobInfo servicerequest)
        {
            try
            {
                DateTime currenttime = DateTime.Now;
                string sqlFormattedPostedDate = currenttime.ToString("yyyy-MM-dd HH:mm:ss");

                Service newservice = new Service()
                {
                    Name = servicerequest.Name,
                    Description = servicerequest.Description,
                    StatusID = servicerequest.StatusID,
                    Address = servicerequest.Address,
                    PostalCode = servicerequest.PostalCode,
                    City = servicerequest.City,
                    Country = servicerequest.Country,
                    CoordX = servicerequest.CoordX,
                    CoordY = servicerequest.CoordY,
                    ClientID = servicerequest.ClientID,
                    CategoryID = servicerequest.CategoryID,
                    PostedDate = sqlFormattedPostedDate,
                    ContractorID = null
                };

                db.Services.Add(newservice);
                db.SaveChanges();

                Logger.Info(String.Format("ServiceRepository.CreateService: created service"));

                return EnumHelper.GetDescription(ErrorListEnum.OK);
            }
            catch (Exception ex)
            {
                Logger.Error("Error ServiceRepository.CreateService", ex);
                return ex.ToString();
            }
        }

        public Result EditJob(int jobId, JobInfo servicerequest)
        {
            Result result = new Result();
            result.message = EnumHelper.GetDescription(ErrorListEnum.OK);
            result.resultCode = (int)ErrorListEnum.OK;
            try
            {
                var jobs = from service in db.Services
                               where service.ID == jobId
                               select service;

                Service selectedJob = jobs.FirstOrDefault();

                selectedJob.Name = servicerequest.Name;
                selectedJob.Description = servicerequest.Description;
                selectedJob.StatusID = servicerequest.StatusID;
                selectedJob.Address = servicerequest.Address;
                selectedJob.PostalCode = servicerequest.PostalCode;
                selectedJob.City = servicerequest.City;
                selectedJob.Country = servicerequest.Country;
                selectedJob.CoordX = servicerequest.CoordX;
                selectedJob.CoordY = servicerequest.CoordY;
                selectedJob.ClientID = servicerequest.ClientID;
                selectedJob.CategoryID = servicerequest.CategoryID;

                db.SaveChanges();
                return result;
            }
            catch (Exception ex)
            {
                string message = String.Format("Error ServiceRepository.EditService {0}: {1}", jobId.ToString(), ex);
                Logger.ErrorFormat(message);

                result.message = message;
                result.resultCode = (int)ErrorListEnum.Service_Edit_Error;
               
                return result;
            }
        }

        public JobInfo GetServiceInfo(int ServiceId)
        {
            try
            {
                var services = from service in db.Services
                               where service.ID == ServiceId
                               select service;

                Service serviceselected = services.FirstOrDefault();

                JobInfo serviceinfo = new JobInfo();

                serviceinfo.Id = serviceselected.ID;
                serviceinfo.Name = serviceselected.Name;
                serviceinfo.Description = serviceselected.Description;
                serviceinfo.StatusID = serviceselected.StatusID;
                serviceinfo.Address = serviceselected.Address;
                serviceinfo.PostalCode = serviceselected.PostalCode;
                serviceinfo.City = serviceselected.City;
                serviceinfo.Country = serviceselected.Country;
                serviceinfo.CoordX = serviceselected.CoordX;
                serviceinfo.CoordY = serviceselected.CoordY;
                serviceinfo.ClientID = serviceselected.ClientID;
                serviceinfo.CategoryID = serviceselected.CategoryID;
                serviceinfo.PostedDate = serviceselected.PostedDate;
                serviceinfo.ContractorID = serviceselected.ContractorID;

                return serviceinfo;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("ServiceRepository.GetServiceInfo {0}: {1}", ServiceId.ToString(), ex);
                return null;
            }
        }

        public Result ChangeServiceStatus(int ServiceId, int StatusId)
        {
            Result result = new Result();
            result.resultCode = -1;
            try
            {
                var services = from service in db.Services
                               where service.ID == ServiceId
                               select service;

                Service serviceselected = services.FirstOrDefault();
                serviceselected.StatusID = StatusId;
                db.SaveChanges();

                result.message = EnumHelper.GetDescription(ErrorListEnum.OK);
                result.resultCode = (int)ErrorListEnum.OK;

                return result;
            }
            catch (Exception ex)
            {
                result.message = string.Format("Error ServiceRepository.ChangeServiceStatus {0}: {1}", ServiceId.ToString(), ex);
                result.resultCode = (int)ErrorListEnum.Service_Edit_Error;
                return result;
            }
        }

        public bool ServiceIdExists(int ServiceId)
        {
            try
            {
                var services = from service in db.Services
                            where service.ID == ServiceId
                            select service;
                List<Service> result = services.ToList();
                return (result.Count() != 0);
            }
            catch (Exception ex)
            {
                Logger.Error("Error ServiceRepository.ServiceIdExists", ex);
                return false;
            }
        }

        public List<JobInfo> GetMyCurrentServices(int client)
        {
            try
            {
                var services = from service in db.Services
                               where service.ClientID == client
                               && (service.StatusID == (int)ServiceStatusEnum.Open|| service.StatusID == (int)ServiceStatusEnum.InProgress)
                               select service;

                List<JobInfo> serviceinfolist = new List<JobInfo>();

                foreach (var s in services)
                {
                    JobInfo serviceinfo = new JobInfo();

                    serviceinfo.Id = s.ID;
                    serviceinfo.Name = s.Name;
                    serviceinfo.Description = s.Description;
                    serviceinfo.StatusID = s.StatusID;
                    serviceinfo.Address = s.Address;
                    serviceinfo.PostalCode = s.PostalCode;
                    serviceinfo.City = s.City;
                    serviceinfo.Country = s.Country;
                    serviceinfo.CoordX = s.CoordX;
                    serviceinfo.CoordY = s.CoordY;
                    serviceinfo.ClientID = s.ClientID;
                    serviceinfo.CategoryID = s.CategoryID;
                    serviceinfo.PostedDate = s.PostedDate;
                    serviceinfo.ContractorID = s.ContractorID;

                    serviceinfolist.Add(serviceinfo);
                }

                return serviceinfolist;
            }
            catch (Exception ex)
            {
                Logger.Error("Error ServiceRepository.GetMyCurrentServices", ex);
                return null;
            }
        }

        public List<JobInfo> GetMyCompletedServices(int client)
        {
            try
            {
                var services = from service in db.Services
                               where service.ClientID == client
                               && (service.StatusID == (int)ServiceStatusEnum.Completed || service.StatusID == (int)ServiceStatusEnum.Cancelled)
                               select service;

                List<JobInfo> serviceinfolist = new List<JobInfo>();

                foreach (var s in services)
                {
                    JobInfo serviceinfo = new JobInfo();

                    serviceinfo.Id = s.ID;
                    serviceinfo.Name = s.Name;
                    serviceinfo.Description = s.Description;
                    serviceinfo.StatusID = s.StatusID;
                    serviceinfo.Address = s.Address;
                    serviceinfo.PostalCode = s.PostalCode;
                    serviceinfo.City = s.City;
                    serviceinfo.Country = s.Country;
                    serviceinfo.CoordX = s.CoordX;
                    serviceinfo.CoordY = s.CoordY;
                    serviceinfo.ClientID = s.ClientID;
                    serviceinfo.CategoryID = s.CategoryID;
                    serviceinfo.PostedDate = s.PostedDate;
                    serviceinfo.ContractorID = s.ContractorID;

                    serviceinfolist.Add(serviceinfo);
                }

                return serviceinfolist;
            }
            catch (Exception ex)
            {
                Logger.Error("Error ServiceRepository.GetMyCurrentServices", ex);
                return null;
            }
        }

        
        public List<JobInfo> GetOpenServicesAssignedToMe(int contractor)
        {
            try
            {
                var services = from service in db.Services
                               where service.ContractorID == contractor
                               && (service.StatusID == (int)ServiceStatusEnum.Open|| service.StatusID == (int)ServiceStatusEnum.InProgress)
                               select service;

                List<JobInfo> serviceinfolist = new List<JobInfo>();

                foreach (var s in services)
                {
                    JobInfo serviceinfo = new JobInfo();

                    serviceinfo.Id = s.ID;
                    serviceinfo.Name = s.Name;
                    serviceinfo.Description = s.Description;
                    serviceinfo.StatusID = s.StatusID;
                    serviceinfo.Address = s.Address;
                    serviceinfo.PostalCode = s.PostalCode;
                    serviceinfo.City = s.City;
                    serviceinfo.Country = s.Country;
                    serviceinfo.CoordX = s.CoordX;
                    serviceinfo.CoordY = s.CoordY;
                    serviceinfo.ClientID = s.ClientID;
                    serviceinfo.CategoryID = s.CategoryID;
                    serviceinfo.PostedDate = s.PostedDate;
                    serviceinfo.ContractorID = s.ContractorID;

                    serviceinfolist.Add(serviceinfo);
                }

                return serviceinfolist;
            }
            catch (Exception ex)
            {
                Logger.Error("Error ServiceRepository.GetOpenServicesAssignedToMe", ex);
                return null;
            }
        }

        public List<JobInfo> GetClosedServicesAssignedToMe(int contractor)
        {
            try
            {
                var services = from service in db.Services
                               where service.ContractorID == contractor
                               && (service.StatusID == (int)ServiceStatusEnum.Cancelled || service.StatusID == (int)ServiceStatusEnum.Completed)
                               select service;

                List<JobInfo> serviceinfolist = new List<JobInfo>();

                foreach (var s in services)
                {
                    JobInfo serviceinfo = new JobInfo();

                    serviceinfo.Id = s.ID;
                    serviceinfo.Name = s.Name;
                    serviceinfo.Description = s.Description;
                    serviceinfo.StatusID = s.StatusID;
                    serviceinfo.Address = s.Address;
                    serviceinfo.PostalCode = s.PostalCode;
                    serviceinfo.City = s.City;
                    serviceinfo.Country = s.Country;
                    serviceinfo.CoordX = s.CoordX;
                    serviceinfo.CoordY = s.CoordY;
                    serviceinfo.ClientID = s.ClientID;
                    serviceinfo.CategoryID = s.CategoryID;
                    serviceinfo.PostedDate = s.PostedDate;
                    serviceinfo.ContractorID = s.ContractorID;

                    serviceinfolist.Add(serviceinfo);
                }

                return serviceinfolist;
            }
            catch (Exception ex)
            {
                Logger.Error("Error ServiceRepository.GetClosedServicesAssignedToMe", ex);
                return null;
            }
        }

        public Result AddComment(int serviceID, CommentInfo Commentinfo)
        {
            Result result = new Result();

            try
            {
                Comment newcomment = new Comment()
                {
                    Title = Commentinfo.Title,
                    CommentText = Commentinfo.Message,
                    ServiceID = Commentinfo.JobId,
                    CreatedByUserID = Commentinfo.CreatedByUserId,
                    Created = Commentinfo.Created
                };

                db.Comments.Add(newcomment);
                db.SaveChanges();
                int id = (int)newcomment.ID;

                Logger.Info(String.Format("ServiceRepository.AddComment: created comment with ID {0}", id));

                result.message = EnumHelper.GetDescription(ErrorListEnum.OK);
                result.resultCode = (int)ErrorListEnum.OK;

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("Error ServiceRepository.AddComment", ex);

                result.message = ex.ToString();
                result.resultCode = (int)(ErrorListEnum.Comment_AddError);

                return result;
            }
        }

        public List<CommentInfo> GetServiceComments(int ServiceId)
        {
            try
            {
                List<Comment> servicecomments = (from comment in db.Comments
                                                 where comment.ServiceID == ServiceId
                                                 select comment).ToList();

                List<CommentInfo> commentinfolist = new List<CommentInfo>();

                foreach (var c in servicecomments)
                {
                    CommentInfo commentinfo = new CommentInfo();

                    commentinfo.CommentId = c.ID;
                    commentinfo.JobId = c.ServiceID;
                    commentinfo.Message = c.CommentText;
                    commentinfo.Title = c.Title;
                    commentinfo.Created = c.Created;
                    commentinfo.CreatedByUserId = c.CreatedByUserID;

                    commentinfolist.Add(commentinfo);
                }

                return commentinfolist;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("ServiceRepository.GetServiceComment {0}: {1}", ServiceId.ToString(), ex.ToString());
                return null;
            }
        }

        public List<JobInfo> GetListServices(int categoryid, string city, string postcode)
        {
            try
            {
                var services = from service in
                               db.Services
                               where  service.StatusID == (int)ServiceStatusEnum.Open
                               && (categoryid == 0 || service.CategoryID == categoryid)
                               && (city == null || service.City.Contains(city))
                               && (postcode == null || service.PostalCode.Contains(postcode))
                               select service;

                List<JobInfo> serviceinfolist = new List<JobInfo>();

                foreach (var s in services)
                {
                    JobInfo serviceinfo = new JobInfo();

                    serviceinfo.Id = s.ID;
                    serviceinfo.Name = s.Name;
                    serviceinfo.Description = s.Description;
                    serviceinfo.StatusID = s.StatusID;
                    serviceinfo.Address = s.Address;
                    serviceinfo.PostalCode = s.PostalCode;
                    serviceinfo.City = s.City;
                    serviceinfo.Country = s.Country;
                    serviceinfo.CoordX = s.CoordX;
                    serviceinfo.CoordY = s.CoordY;
                    serviceinfo.ClientID = s.ClientID;
                    serviceinfo.CategoryID = s.CategoryID;
                    serviceinfo.PostedDate = s.PostedDate;
                    serviceinfo.ContractorID = s.ContractorID;

                    serviceinfolist.Add(serviceinfo);
                }

                return serviceinfolist;

            }
            catch (Exception ex)
            {
                Logger.Error("Error when getting List of Services", ex);
                return null;
            }
        }

    }
}