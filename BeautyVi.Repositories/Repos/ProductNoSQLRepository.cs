using BeautyVi.Core.Entities;
using BeautyVi.Repositories.Interfaces;
using BeautyVi.Repositories.Repos;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BeautyVi.Reposotories.Repos
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

        public Product GetProductById(int productId)
        {
            throw new NotImplementedException();
        }
    }
}
