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
        private readonly IIngredientRepository ingredientRepository;
        private readonly BeautyViContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public IngredientController(IIngredientRepository ingredientRepository,
            IWebHostEnvironment webHostEnviroment, [FromServices] BeautyViContext context)
        {
            this.ingredientRepository = ingredientRepository;
            this.webHostEnvironment = webHostEnviroment;
            this._context = context;
            //this._userManager = userManager;
        }

        public IActionResult Index()
        {
            var ingredients = ingredientRepository.GetAll();

            return View(ingredients);
        }

        [HttpGet]
        public IActionResult CheckIngredients()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CheckIngredients(string ingredientsInput)
        {
            // Розділяємо введені інгредієнти на окремі рядки
            var inputIngredients = ingredientsInput?
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(i => i.Trim())
                .ToList();

            if (inputIngredients == null || inputIngredients.Count == 0)
            {
                ViewBag.Message = "Будь ласка, введіть список інгредієнтів для перевірки.";
                return View();
            }

            // Шукаємо інгредієнти в базі даних
            var matchedIngredients = _context.Ingredients
                .Where(i => inputIngredients.Contains(i.Name))
                .ToList();

            // Передаємо знайдені інгредієнти у View
            return View(matchedIngredients);
        }

        // Метод для перегляду форми введення інгредієнтів
        /*[HttpGet]
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
            var allIngredients = ingredientRepository.GetAll();

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
        }*/



    }
}
