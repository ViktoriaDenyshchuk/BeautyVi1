using System;
using BeautyVi.Core.Entities;
using BeautyVi.Repositories.Interfaces;

namespace BeautyVi.Repositories.Repos
{
    internal class IngredientNoSQLRepository : IIngredientRepository
    {
        public IngredientNoSQLRepository()
        {

        }
        public void Add(Ingredient obj)
        {
            throw new NotImplementedException();
        }

        public void Delete(Ingredient obj)
        {
            throw new NotImplementedException();
        }

        public Ingredient Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Ingredient> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(Ingredient obj)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

    }
}


/*using BeautyVi.Core.Entities;
using BeautyVi.Repositories.Interfaces;
using BeautyVi.Repositories.Repos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BeautyVi.Reposotories.Repos
{
    internal class IngredientNoSQLRepository : IIngredientRepository
    {
        public IngredientNoSQLRepository()
        {
        }
         public Ingredient Get(int id)
         {
             throw new NotImplementedException();
         }

         public IEnumerable<Ingredient> GetAll()
         {
             throw new NotImplementedException();
         }
         public void Save()
         {
             throw new NotImplementedException();
         }
        public async Task<Ingredient> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        // Асинхронний метод для отримання всіх інгредієнтів
        public async Task<IEnumerable<Ingredient>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        // Асинхронний метод для перевірки наявності інгредієнта за назвою
        public async Task<Ingredient> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

            // Метод для збереження змін (у разі використання репозиторію для збереження даних)
        public async Task SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}*/