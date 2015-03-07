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

        public int CreateService(ServiceInfo servicerequest)
        {
            try
            {
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
                    CategoryID = servicerequest.CategoryID
                };

                db.Services.Add(newservice);
                db.SaveChanges();
                int id = (int)newservice.ID;

                Logger.Info(String.Format("ServiceRepository.CreateService: created service with ID {0}", id));

                return id;
            }
            catch (Exception ex)
            {
                Logger.Error("Error ServiceRepository.CreateService", ex);
                return (int)(ErrorListEnum.Service_Create_Error);
            }
        }

        public string EditService(int ServiceId, ServiceInfo servicerequest)
        {
            try
            {
                var services = from service in db.Services
                               where service.ID == ServiceId
                               select service;

                Service serviceselected = services.FirstOrDefault();

                serviceselected.Name = servicerequest.Name;
                serviceselected.Description = servicerequest.Description;
                serviceselected.StatusID = servicerequest.StatusID;
                serviceselected.Address = servicerequest.Address;
                serviceselected.PostalCode = servicerequest.PostalCode;
                serviceselected.City = servicerequest.City;
                serviceselected.Country = servicerequest.Country;
                serviceselected.CoordX = servicerequest.CoordX;
                serviceselected.CoordY = servicerequest.CoordY;
                serviceselected.ClientID = servicerequest.ClientID;
                serviceselected.CategoryID = servicerequest.CategoryID;

                db.SaveChanges();
                return EnumHelper.GetDescription(ErrorListEnum.OK);
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error ServiceRepository.EditService {0}: {1}", ServiceId.ToString(), ex);
                return EnumHelper.GetDescription(ErrorListEnum.Service_Edit_Error);
            }
        }

        public ServiceInfo GetServiceInfo(int ServiceId)
        {
            try
            {
                var services = from service in db.Services
                               where service.ID == ServiceId
                               select service;

                Service serviceselected = services.FirstOrDefault();

                ServiceInfo serviceinfo = new ServiceInfo();

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

                return serviceinfo;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("ServiceRepository.GetServiceInfo {0}: {1}", ServiceId.ToString(), ex);
                return null;
            }
        }

        public string ChangeServiceStatus(int ServiceId, int StatusId)
        {
            try
            {
                var services = from service in db.Services
                               where service.ID == ServiceId
                               select service;

                Service serviceselected = services.FirstOrDefault();

                serviceselected.StatusID = StatusId;

                db.SaveChanges();
                return EnumHelper.GetDescription(ErrorListEnum.OK);
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error ServiceRepository.ChangeServiceStatus {0}: {1}", ServiceId.ToString(), ex);
                return EnumHelper.GetDescription(ErrorListEnum.Service_Edit_Error);
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

        public int AddComment(int serviceID, int UserID, string Comment_Title, string Comment_Text)
        {
            try
            {
                Comment newcomment = new Comment()
                {
                    Title = Comment_Title,
                    CommentText = Comment_Text,
                    ServiceID = serviceID,
                    CreatedByUserID = UserID
                };

                db.Comments.Add(newcomment);
                db.SaveChanges();
                int id = (int)newcomment.ID;

                Logger.Info(String.Format("ServiceRepository.AddComment: created comment with ID {0}", id));

                return id;
            }
            catch (Exception ex)
            {
                Logger.Error("Error ServiceRepository.AddComment", ex);
                return (int)(ErrorListEnum.Comment_AddError);
            }
        }

        public List<Comment> GetServiceComments(int ServiceId)
        {
            try
            {
                List<Comment> servicecomments = (from comment in db.Comments
                                                 where comment.ServiceID == ServiceId
                                                 select comment).ToList();
                return servicecomments;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("ServiceRepository.GetServiceComment {0}: {1}", ServiceId.ToString(), ex);
                return null;
            }
        }

        public List<GetListServices_Result> GetListServices(SearchService SearchParams)
        {
            try
            {
                var services = from service in
                                   db.GetListServices(SearchParams.CategoryId, SearchParams.City, SearchParams.PostCode)
                               select service;

                return services.ToList();
            }
            catch (Exception ex)
            {
                Logger.Error("Error when getting List of Services", ex);
                return null;
            }
        }

    }
}