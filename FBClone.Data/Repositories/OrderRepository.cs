using System;
using FBClone.Model;
using FBClone.Data.Infrastructure;

namespace FBClone.Data.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
    }

    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}