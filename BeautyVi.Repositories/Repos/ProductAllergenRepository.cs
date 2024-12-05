using BeautyVi.Core.Context;
using BeautyVi.Core.Entities;
using BeautyVi.Repositories.Interfaces;

namespace BeautyVi.Repositories.Repos
{
    public class ProductAllergenRepository : IProductAllergenRepository
    {
        private readonly BeautyViContext _context;

        public ProductAllergenRepository(BeautyViContext context)
        {
            _context = context;
        }

        public void Add(ProductAllergen productAllergen)
        {
            _context.ProductAllergens.Add(productAllergen);
            Save();
        }

        public void Delete(ProductAllergen productAllergen)
        {
            _context.ProductAllergens.Remove(productAllergen);
            Save();
        }

        public ProductAllergen Get(int id)
        {
            return _context.ProductAllergens.Find(id);
        }

        public IEnumerable<ProductAllergen> GetAll()
        {
            return _context.ProductAllergens.ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(ProductAllergen productAllergen)
        {
            _context.ProductAllergens.Update(productAllergen);
            Save();
        }
    }
}
