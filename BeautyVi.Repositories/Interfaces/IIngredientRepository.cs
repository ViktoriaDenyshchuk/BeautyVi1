/*using BeautyVi.Core.Entities;
using BeautyVi.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautyVi.Repositories.Interfaces
{
    public interface IIngredientRepository : ISave
    {
        Ingredient Get(int id);
        IEnumerable<Ingredient> GetAll();
        Task<Ingredient> GetAsync(int id);  // Оновлений асинхронний метод
        Task<IEnumerable<Ingredient>> GetAllAsync();  // Оновлений асинхронний метод
        Task<Ingredient> GetByNameAsync(string name);  // Асинхронний метод для пошуку за назвою
        Task SaveAsync();
    }
}*/
using System;
using BeautyVi.Core.Entities;

namespace BeautyVi.Repositories.Interfaces
{
    public interface IIngredientRepository : ISave
    {
        Ingredient Get(int id);
        IEnumerable<Ingredient> GetAll();
        void Add(Ingredient ingredient);
        void Update(Ingredient ingredient);
        void Delete(Ingredient ingredient);
    }

}



