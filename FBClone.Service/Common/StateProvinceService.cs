using FBClone.Data.Infrastructure;
using FBClone.Data.Repositories;
using FBClone.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FBClone.Service
{
    public interface IStateProvinceService
    {
        IEnumerable<StateProvince> GetAll();
        IQueryable<StateProvince> Query(Expression<Func<StateProvince, bool>> where);
    }

    public class StateProvinceService : IStateProvinceService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IStateProvinceRepository stateProvinceRepository;

        public StateProvinceService(IUnitOfWork unitOfWork, IStateProvinceRepository stateProvinceRepository)
        {
            this.unitOfWork = unitOfWork;
            this.stateProvinceRepository = stateProvinceRepository;
        }
        public IEnumerable<StateProvince> GetAll()
        {
            return stateProvinceRepository.GetAll();
        }

        public IQueryable<StateProvince> Query(Expression<Func<StateProvince, bool>> where)
        {
            return stateProvinceRepository.Query(where);
        }
    }
}
