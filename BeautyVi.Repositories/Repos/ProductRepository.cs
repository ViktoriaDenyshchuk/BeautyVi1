using Microsoft.EntityFrameworkCore.Metadata.Internal;
using BeautyVi.Core.Context;
using BeautyVi.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeautyVi.Repositories.Interfaces;
using System.Reflection;

namespace BeautyVi.Repositories.Repos
{
    public class ProductRepository : IProductRepository
    {
        private BeautyViContext _context;
        public ProductRepository(BeautyViContext context)
        {
            _context = context;
        }
        public void Add(Product obj)
        {
            _context.Products.Add(obj);
            Save();
        }

        public void Delete(Product obj)
        {
            _context.Set<Product>().Remove(obj);
            Save();
        }

        public Product Get(int id)
        {
            return _context.Products.Find(id);
        }

       public IEnumerable<Product> GetAll()
        {
            return _context.Products.ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Product obj)
        {
            _context.Products.Update(obj);
        }
        public Product Find(int id)
        {
            return _context.Products.FirstOrDefault(product => product.Id == id);
        }

        public Product GetProductById(int productId)
        {
            return _context.Products.Find(productId);
        }
    }
}