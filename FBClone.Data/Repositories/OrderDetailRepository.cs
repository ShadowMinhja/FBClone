using System;
using FBClone.Model;
using FBClone.Data.Infrastructure;

namespace FBClone.Data.Repositories
{
    public interface IOrderDetailRepository : IGenericRepository<OrderDetail>
    {
    }

    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}