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
    public interface IOrderService
    {
        Order GetById(long Id);
        IEnumerable<Order> GetAll();
        IQueryable<Order> Query(Expression<Func<Order, bool>> where);
        Order Add(Order order);
        Order Update(Order order);
        void Delete(long id);

        OrderDetail Add(OrderDetail order);
        OrderDetail Update(OrderDetail order);
        void DeleteOrderDetail(long id);
    }
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IOrderRepository orderRepository;
        private readonly IOrderDetailRepository orderDetailRepository;
        private readonly ILocationRepository locationRepository;
        public OrderService(IUnitOfWork unitOfWork, IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, ILocationRepository locationRepository)
        {
            this.unitOfWork = unitOfWork;
            this.orderRepository = orderRepository;
            this.orderDetailRepository = orderDetailRepository;
            this.locationRepository = locationRepository;
        }

        public Order GetById(long id)
        {
            return orderRepository.GetById(id);
        }

        public IEnumerable<Order> GetAll()
        {
            return orderRepository.GetAll();
        }

        public IQueryable<Order> Query(Expression<Func<Order, bool>> where)
        {
            return orderRepository.Query(where);
        }

        public Order Add(Order order)
        {
            Order lastOrder = orderRepository.Query(x => x.LocationId == order.LocationId)
                .OrderByDescending(x => x.OrderNumber)
                .FirstOrDefault();
            string newOrderNumber = (lastOrder == null ? 1 : (Convert.ToInt32(lastOrder.OrderNumber) + 1)).ToString("0000");
            order.OrderNumber = newOrderNumber.Substring(newOrderNumber.Length - 4);
            order.Status = "Waiting";
            order.Location = locationRepository.GetById(order.LocationId);
            //This requires DataContext to be set as InSingletonScope/InThreadScope with Ninject. Otherwise, have create new dbcontext below
            orderRepository.Add(order);
            unitOfWork.SaveChanges();
            //using (var dbContext = new FBCloneContext())
            //{
            //    dbContext.Orders.Add(order);
            //    dbContext.SaveChanges();
            //}
            return order;
        }

        public Order Update(Order order)
        {
            orderRepository.Update(order);
            unitOfWork.SaveChanges();
            return order;
        }

        public void Delete(long id)
        {
            var order = orderRepository.GetById(id);
            var orders = orderRepository.GetMany(x => x.Id == id); //<--Anything that has foreign key relationships

            foreach (var item in orders)
            {
                orderRepository.Delete(item);
            }

            if (order != null)
            {
                orderRepository.Delete(order);
                unitOfWork.SaveChanges();
            }
        }

        //Order Details
        public OrderDetail Add(OrderDetail orderDetail)
        {
            orderDetailRepository.Add(orderDetail);
            unitOfWork.SaveChanges();
            return orderDetail;
        }

        public OrderDetail Update(OrderDetail orderDetail)
        {
            orderDetailRepository.Update(orderDetail);
            unitOfWork.SaveChanges();
            return orderDetail;
        }

        public void DeleteOrderDetail(long id)
        {
            var orderDetail = orderDetailRepository.GetById(id);
            var orderDetails = orderDetailRepository.GetMany(x => x.Id == id); //<--Anything that has foreign key relationships

            foreach (var item in orderDetails)
            {
                orderDetailRepository.Delete(item);
            }

            if (orderDetail != null)
            {
                orderDetailRepository.Delete(orderDetail);
                unitOfWork.SaveChanges();
            }
        }

    }
}
