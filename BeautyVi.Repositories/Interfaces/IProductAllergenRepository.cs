using System;
using BeautyVi.Core.Entities;

namespace BeautyVi.Repositories.Interfaces
{
    public interface IProductAllergenRepository : ISave
    {
        ProductAllergen Get(int id);
        IEnumerable<ProductAllergen> GetAll();
        void Add(ProductAllergen productAllergen);
        void Update(ProductAllergen productAllergen);
        void Delete(ProductAllergen productAllergen);
    }

}

