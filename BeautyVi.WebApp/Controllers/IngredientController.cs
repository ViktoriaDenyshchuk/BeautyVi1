using BeautyVi.Core.Entities; 
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

        /*[HttpPost]
        public IActionResult CheckIngredients(string ingredientsInput)
        {
            var inputIngredients = ingredientsInput?
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(i => i.Trim())
                .ToList();

            if (inputIngredients == null || inputIngredients.Count == 0)
            {
                ViewBag.Message = "Будь ласка, введіть список інгредієнтів для перевірки.";
                return View();
            }

            var matchedIngredients = _context.Ingredients
                .Where(i => inputIngredients.Contains(i.Name))
                .ToList();

            if (matchedIngredients.Any())
            {
                ViewBag.AverageDangerLevel = matchedIngredients.Average(i => i.LevelOfDanger);
            }

            return View(matchedIngredients);
        }*/
        [HttpPost]
        public IActionResult CheckIngredients(string ingredientsInput)
        {
            var inputIngredients = ingredientsInput?
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(i => i.Trim())
                .ToList();

            if (inputIngredients == null || inputIngredients.Count == 0)
            {
                ViewBag.Message = "Будь ласка, введіть список інгредієнтів для перевірки.";
                return View();
            }

            // Отримати знайдені інгредієнти з бази даних
            var matchedIngredients = _context.Ingredients
                .Where(i => inputIngredients.Contains(i.Name))
                .ToList();

            // Знайти інгредієнти, яких немає в базі даних
            var unknownIngredients = inputIngredients
                .Where(i => !matchedIngredients.Any(m => m.Name == i))
                .ToList();

            // Передати знайдені та невідомі інгредієнти у View
            ViewBag.UnknownIngredients = unknownIngredients;
            ViewBag.AverageDangerLevel = matchedIngredients.Any()
                ? matchedIngredients.Average(i => i.LevelOfDanger)
                : (double?)null;

            return View(matchedIngredients);
        }

    }
}
