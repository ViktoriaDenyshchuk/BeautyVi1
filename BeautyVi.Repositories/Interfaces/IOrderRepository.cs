using BeautyVi.Core.Entities;
using BeautyVi.Core.Entities;
using BeautyVi.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautyVi.Repositories.Interfaces
{
    public interface IOrderRepository : ISave
    {
        Order Get(int id);
        IEnumerable<Order> GetAll();
        void Add(Order obj);
        void Update(Order obj);
        void Delete(Order obj);
        void AddOrderWithItems(Order order, IEnumerable<OrderItem> orderItems);


        //int Find(int id);
    }
}