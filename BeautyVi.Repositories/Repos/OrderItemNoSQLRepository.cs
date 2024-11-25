using BeautyVi.Core.Entities;
using BeautyVi.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautyVi.Repositories.Repos
{
    internal class OrderItemNoSQLRepository : IOrderItemRepository
    {
        //private readonly MongoDbConnection connection;
        public OrderItemNoSQLRepository()
        {

        }
        public void Add(Order obj)
        {
            throw new NotImplementedException();
        }

        public void Delete(Order obj)
        {
            throw new NotImplementedException();
        }

        public Order Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> GetAll()
        {
            throw new NotImplementedException();
        }

        /*public IQueryable<Order> GetAll()
        {
            throw new NotImplementedException();
        }*/

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(Order obj)
        {
            throw new NotImplementedException();
        }

        void IOrderItemRepository.Add(OrderItem obj)
        {
            throw new NotImplementedException();
        }

        void IOrderItemRepository.Delete(OrderItem obj)
        {
            throw new NotImplementedException();
        }

        OrderItem IOrderItemRepository.Get(int id)
        {
            throw new NotImplementedException();
        }

        IEnumerable<OrderItem> IOrderItemRepository.GetAll()
        {
            throw new NotImplementedException();
        }

        void ISave.Save()
        {
            throw new NotImplementedException();
        }

        void IOrderItemRepository.Update(OrderItem obj)
        {
            throw new NotImplementedException();
        }
    }
}