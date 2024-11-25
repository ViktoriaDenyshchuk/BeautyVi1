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
    public interface IOrderItemRepository : ISave
    {
        OrderItem Get(int id);
        IEnumerable<OrderItem> GetAll();
        void Add(OrderItem obj);
        void Update(OrderItem obj);
        void Delete(OrderItem obj);
    }
}
