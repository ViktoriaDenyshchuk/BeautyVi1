using BeautyVi.Core.Context;
using BeautyVi.Core.Entities;
using BeautyVi.Repositories.Interfaces;

namespace BeautyVi.Repositories.Repos
{
    public class AllergenRepository : IAllergenRepository
    {
        private readonly BeautyViContext _context;

        public AllergenRepository(BeautyViContext context)
        {
            _context = context;
        }

        public void Add(Allergen allergen)
        {
            _context.Allergens.Add(allergen);
            Save();
        }

        public void Delete(Allergen allergen)
        {
            _context.Allergens.Remove(allergen);
            Save();
        }

        public Allergen Get(int id)
        {
            return _context.Allergens.Find(id);
        }

        public IEnumerable<Allergen> GetAll()
        {
            return _context.Allergens.ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Allergen allergen)
        {
            _context.Allergens.Update(allergen);
            Save();
        }
    }
}
