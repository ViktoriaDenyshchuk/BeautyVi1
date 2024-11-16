using BeautyVi.Repositories.Interfaces;
using BeautyVi.Core.Entities;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace BeautyVi.Repositories.Interfaces
{
    public interface IProductRepository : ISave
    {
        Product Get(int id);
        IEnumerable<Product> GetAll();
        void Add(Product obj);
        void Update(Product obj);
        void Delete(Product obj);

        Task<Product> GetByIdAsync(int id);
        Task SaveChangesAsync();
        int Find(int id);
        Product GetListingById(int listingId);
        //IEnumerable<Product> SearchProducts(string searchTerm);
        IEnumerable<Product> GetByCategory(int categoryId);
    }
}
