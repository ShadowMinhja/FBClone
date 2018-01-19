using System;
using System.Collections.Generic;
using System.Linq;
using FBClone.Data;
using FBClone.Data.Infrastructure;
using FBClone.Data.Repositories;
using FBClone.Model;
using System.Linq.Expressions;

namespace FBClone.Service
{
    public interface IHubConnectionService
    {
        HubConnection GetById(string Id);
        HubConnection GetByUserName(string name);
        IQueryable<HubConnection> Query(Expression<Func<HubConnection, bool>> where);
        HubConnection Add(HubConnection hubConnection);
        HubConnection Update(HubConnection hubConnection);
        void Delete(string id);
    }
    public class HubConnectionService : IHubConnectionService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHubConnectionRepository hubConnectionRepository;
        public HubConnectionService(IUnitOfWork unitOfWork, IHubConnectionRepository hubConnectionRepository)
        {
            this.unitOfWork = unitOfWork;
            this.hubConnectionRepository = hubConnectionRepository;
        }

        public HubConnection GetById(string id)
        {
            return hubConnectionRepository.Query(x => x.ConnectionId == new Guid(id)).SingleOrDefault();
        }

        public HubConnection GetByUserName(string name)
        {
            return hubConnectionRepository.Query(x => x.UserId == name).SingleOrDefault();
        }

        public IQueryable<HubConnection> Query(Expression<Func<HubConnection, bool>> where)
        {
            return hubConnectionRepository.Query(where);
        }

        public HubConnection Add(HubConnection hubConnection)
        {
            hubConnectionRepository.Add(hubConnection);
            unitOfWork.SaveChanges();
            return hubConnection;
        }

        public HubConnection Update(HubConnection hubConnection)
        {
            hubConnectionRepository.Update(hubConnection);
            unitOfWork.SaveChanges();
            return hubConnection;
        }

        public void Delete(string id)
        {
            var hubConnection = GetById(id);
            if (hubConnection != null)
            {
                hubConnectionRepository.Delete(hubConnection);
                unitOfWork.SaveChanges();
            }
        }

    }
}
