using BeautyVi.Core.Context;
using BeautyVi.Core.Entities;
using BeautyVi.Repositories.Interfaces;

namespace BeautyVi.Repositories.Repos
{
    public class ProductIngredientRepository : IProductIngredientRepository
    {
        private readonly BeautyViContext _context;

        public ProductIngredientRepository(BeautyViContext context)
        {
            _context = context;
        }

        public void Add(ProductIngredient productIngredient)
        {
            _context.ProductIngredients.Add(productIngredient);
            Save();
        }

        public void Delete(ProductIngredient productIngredient)
        {
            _context.ProductIngredients.Remove(productIngredient);
            Save();
        }

        public ProductIngredient Get(int id)
        {
            return _context.ProductIngredients.Find(id);
        }

        public IEnumerable<ProductIngredient> GetAll()
        {
            return _context.ProductIngredients.ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(ProductIngredient productIngredient)
        {
            _context.ProductIngredients.Update(productIngredient);
            Save();
        }
    }
}
