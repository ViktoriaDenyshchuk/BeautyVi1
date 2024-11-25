using BeautyVi.Core.Context;
using BeautyVi.Core.Entities;
using BeautyVi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeautyVi.Repositories.Repos
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly BeautyViContext _context;

        public OrderItemRepository(BeautyViContext context)
        {
            _context = context;
        }

        public void Add(OrderItem obj)
        {
            _context.OrderItems.Add(obj);
            Save();
        }

        public void Delete(OrderItem obj)
        {
            _context.OrderItems.Remove(obj);
            Save();
        }

        public OrderItem Get(int id)
        {
            return _context.OrderItems.Include(x => x.Product).FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<OrderItem> GetAll()
        {
            return _context.OrderItems.Include(x => x.Product).AsNoTracking();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(OrderItem obj)
        {
            _context.OrderItems.Update(obj);
            Save();
        }
    }
}
