using System;
using BeautyVi.Core.Entities;

namespace BeautyVi.Repositories.Interfaces
{
    public interface IProductIngredientRepository : ISave
    {
        ProductIngredient Get(int id);
        IEnumerable<ProductIngredient> GetAll();
        void Add(ProductIngredient productIngredient);
        void Update(ProductIngredient productIngredient);
        void Delete(ProductIngredient productIngredient);
    }

}

