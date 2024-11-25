/*using BeautyVi.Core.Context;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeautyVi.Core.Entities;
using BeautyVi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeautyVi.Repositories.Repos
{
    public class IngredientRepository : IIngredientRepository
    {
        private BeautyViContext _context;
        public IngredientRepository(BeautyViContext context)
        {
            _context = context;
        }
        public Ingredient Get(int id)
        {
            return _context.Ingredients.Find(id);
            //return _context.Set<Product>().Find(id);
        }

        public IEnumerable<Ingredient> GetAll()
        {
            return _context.Ingredients.ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}*/
using BeautyVi.Core.Context;
using BeautyVi.Core.Entities;
using BeautyVi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeautyVi.Repositories.Repos
{
    public class IngredientRepository : IIngredientRepository
    {
        private readonly BeautyViContext _context;

        public IngredientRepository(BeautyViContext context)
        {
            _context = context;
        }

        public Ingredient Get(int id)
        {
            return _context.Ingredients.Find(id);
            //return _context.Set<Product>().Find(id);
        }

        public IEnumerable<Ingredient> GetAll()
        {
            return _context.Ingredients.ToList();
        }
        // Асинхронний метод для отримання інгредієнта за ID
        public async Task<Ingredient> GetAsync(int id)
        {
            return await _context.Ingredients.FindAsync(id);
        }

        // Асинхронний метод для отримання всіх інгредієнтів
        public async Task<IEnumerable<Ingredient>> GetAllAsync()
        {
            return await _context.Ingredients.ToListAsync();
        }

        // Асинхронний метод для перевірки наявності інгредієнта за назвою

        public async Task<Ingredient> GetByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            // Отримуємо всі інгредієнти з бази даних
            var ingredients = await _context.Ingredients.ToListAsync();

            // Знаходимо інгредієнт по назві, порівнюючи з урахуванням регістру
            var ingredient = ingredients
                .FirstOrDefault(i => i.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            return ingredient;
        }


        // Метод для збереження змін (у разі використання репозиторію для збереження даних)
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
