using System;
using System.Collections.Generic;
using System.Linq;
using FBClone.Data;
using System.Data.Entity;
using FBClone.Data.Infrastructure;
using FBClone.Data.Repositories;
using FBClone.Model;
using System.Linq.Expressions;

namespace FBClone.Service
{
    public interface IApplicationSettingService
    {
        IEnumerable<ApplicationSetting> GetAll();
        IQueryable<ApplicationSetting> Query(Expression<Func<ApplicationSetting, bool>> where);

        ApplicationSetting GetById(string userid);
        ApplicationSetting Update(ApplicationSetting applicationSetting);
    }
    public class ApplicationSettingService : IApplicationSettingService
    {
        private readonly IUnitOfWork unitOfWork;
        private FBCloneContext dataContext;

        public ApplicationSettingService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.dataContext = new FBCloneContext();
        }

        public IEnumerable<ApplicationSetting> GetAll()
        {
            return dataContext.ApplicationSettings.ToList();
        }

        public IQueryable<ApplicationSetting> Query(Expression<Func<ApplicationSetting, bool>> where)
        {
            return dataContext.ApplicationSettings.Where(where);
        }

        public ApplicationSetting GetById(string userid)
        {
            return dataContext.ApplicationSettings.Where(x => x.UserId == userid).SingleOrDefault();
        }

        public ApplicationSetting Update(ApplicationSetting applicationSetting)
        {
            dataContext.ApplicationSettings.Attach(applicationSetting);
            dataContext.Entry(applicationSetting).State = EntityState.Modified;
            dataContext.SaveChanges();
            return applicationSetting;
        }

    }
}
