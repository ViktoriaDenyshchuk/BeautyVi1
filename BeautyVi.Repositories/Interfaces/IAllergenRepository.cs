using System;
using BeautyVi.Core.Entities;

namespace BeautyVi.Repositories.Interfaces
{
    public interface IAllergenRepository : ISave
    {
        Allergen Get(int id);
        IEnumerable<Allergen> GetAll();
        void Add(Allergen allergen);
        void Update(Allergen allergen);
        void Delete(Allergen allergen);
    }

}

