using BeautyVi.Core.Entities;
using BeautyVi.Repositories.Interfaces;
using BeautyVi.Repositories.Repos;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace SweetCreativity1.Reposotories.Repos
{
    internal class ProductNoSQLRepository : IProductRepository
    {
        public ProductNoSQLRepository()
        {
        }

        public void Add(Product obj)
        {
            throw new NotImplementedException();
        }

        public void Delete(Product obj)
        {
            throw new NotImplementedException();
        }

        public Product Get(int id)
        {
            throw new NotImplementedException();
        }

       public IEnumerable<Product> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(Product obj)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        int IProductRepository.Find(int id)
        {
            throw new NotImplementedException();
        }

        Task IProductRepository.SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Product GetListingById(int listingId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetByCategory(int categoryId)
        {
            throw new NotImplementedException();
        }

        //public IEnumerable<Listing> SearchListings(string searchTerm)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
