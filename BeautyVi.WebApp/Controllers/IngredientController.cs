using BeautyVi.Core.Entities; // Імпортуємо правильний простір імен
using BeautyVi.Repositories.Interfaces;
using BeautyVi.Repositories.Repos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeautyVi.Controllers
{
    public class IngredientController : Controller
    {
        private readonly IIngredientRepository _ingredientRepository;

        public IActionResult Index()
        {
            var allIngredients = _ingredientRepository.GetAll();

            return View(allIngredients);
        }

        public IngredientController(IIngredientRepository ingredientRepository)
        {
            _ingredientRepository = ingredientRepository;
        }

        // Метод для перегляду форми введення інгредієнтів
        [HttpGet]
        public IActionResult CheckIngredients()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CheckIngredients(string ingredientsList)
        {
            if (string.IsNullOrEmpty(ingredientsList))
            {
                ModelState.AddModelError(string.Empty, "Будь ласка, введіть список інгредієнтів.");
                return View();
            }

            // Розбиваємо введений список на окремі інгредієнти
            var ingredientNames = ingredientsList.Split(',').Select(x => x.Trim()).ToList();

            var ingredients = new List<Ingredient>();

            // Отримуємо всі інгредієнти з бази даних
            var allIngredients = await _ingredientRepository.GetAllAsync();

            foreach (var ingredientName in ingredientNames)
            {
                // Шукаємо інгредієнт по назві серед всіх інгредієнтів
                var ingredient = allIngredients.FirstOrDefault(i => i.Name.Equals(ingredientName, StringComparison.OrdinalIgnoreCase));

                if (ingredient != null)
                {
                    ingredients.Add(ingredient);
                }
                else
                {
                    ingredients.Add(new Ingredient
                    {
                        Name = ingredientName,
                        IsHarmful = false,
                        LevelOfDanger = 0,
                        Description = "Інгредієнт не знайдений в базі даних."
                    });
                }
            }

            return View("IngredientCheckResults", ingredients);
        }



    }
}
