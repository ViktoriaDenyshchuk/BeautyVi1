using BeautyVi.Core.Entities;
using BeautyVi.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautyVi.Repositories.Repos
{
    internal class OrderNoSQLRepository : IOrderRepository
    {
        //private readonly MongoDbConnection connection;
        public OrderNoSQLRepository()
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
        public void AddOrderWithItems(Order order, IEnumerable<OrderItem> orderItems)
        {
            throw new NotImplementedException();
        }
    }
}