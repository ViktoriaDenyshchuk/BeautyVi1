using BeautyVi.Core.Context;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeautyVi.Core.Entities;
using BeautyVi.Repositories.Interfaces;

namespace BeautyVi.Repositories.Repos
{
    public class OrderRepository : IOrderRepository
    {
        private BeautyViContext _context;
        public OrderRepository(BeautyViContext context)
        {
            _context = context;
        }
        public void Add(Order obj)
        {
            _context.Orders.Add(obj);
            Save();
        }

        public void Delete(Order obj)
        {
            _context.Set<Order>().Remove(obj);
            Save();
        }

        public Order Get(int id)
        {
            return _context.Orders.Find(id);

        }

        public IEnumerable<Order> GetAll()
        {
            return _context.Orders.ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Order obj)
        {
            _context.Orders.Update(obj);
        }

        /*public void UpdateStatus(int orderId, int statusId)
        {
            var order = _context.Orders.Find(orderId);
            if (order != null)
            {
                order.StatusId = statusId;
                _context.SaveChanges();
            }
        }*/
    }
}