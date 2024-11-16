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
            //return _context.Set<Product>().Find(id);
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

        public Task<Product> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        int IProductRepository.Find(int id)
        {
            throw new NotImplementedException();
        }
        public Task SaveChangesAsync()
        {
            // Реалізація збереження змін в базу даних
            throw new NotImplementedException();
        }

        public Product GetProductById(int productId)
        {
            return _context.Products.Find(productId);
        }

        public Product GetListingById(int listingId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetByCategory(int categoryId) => _context.Products.Where(p => p.CategoryId == categoryId).ToList();
        //public IEnumerable<Listing> SearchListings(string searchTerm)
        //{
        //    // Ваш код для виконання пошукового запиту в базі даних
        //    // Наприклад, використовуйте LINQ для фільтрації результатів

        //    if (string.IsNullOrEmpty(searchTerm))
        //    {
        //        // Якщо пошуковий термін не вказано, поверніть усі оголошення
        //        return _context.Listings.ToList();
        //    }
        //    else
        //    {
        //        // Використовуйте LINQ для фільтрації результатів за назвою та описом
        //        return _context.Listings
        //            .Where(listing => listing.Title.Contains(searchTerm) || listing.Description.Contains(searchTerm))
        //            .ToList();
        //    }
        //}


    }
}