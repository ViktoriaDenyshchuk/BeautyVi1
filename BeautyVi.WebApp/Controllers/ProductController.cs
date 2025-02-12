using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeautyVi.Core.Context;
using BeautyVi.Repositories.Interfaces;
using BeautyVi.Repositories.Repos;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BeautyVi.Core.Entities;
using System.Reflection;
using System.Security.Claims;

namespace BeautyVi.WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly BeautyViContext _context;

        public ProductController(IProductRepository productRepository,
            IWebHostEnvironment webHostEnviroment, [FromServices] BeautyViContext context)
        {
            this.productRepository = productRepository;
            this.webHostEnvironment = webHostEnviroment;
            this._context = context;
        }

        public IActionResult Index()
        {
            ViewBag.HairTypes = _context.SuitableForOptions
                .Where(s => s.NameSuitableFor.Contains("волосся"))
                .ToList();

            ViewBag.SkinTypes = _context.SuitableForOptions
                .Where(s => s.NameSuitableFor.Contains("шкіра"))
                .ToList();

            ViewBag.AvoidedAllergens = _context.Allergens.ToList();
            ViewBag.EffectTypes = _context.EffectTypes.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Ingredients = _context.Ingredients.ToList();

            var allProducts = productRepository.GetAll();

            return View(allProducts);
        }

        [HttpGet]
        public IActionResult Search(string searchTerm)
        {
            var searchResults = _context.Products
                .Where(product => product.Name.Contains(searchTerm)
                || product.Description.Contains(searchTerm)
                || product.Category.NameCategory.Contains(searchTerm)
                || product.EffectType.NameEffectType.Contains(searchTerm)
                || product.SuitableFor.NameSuitableFor.Contains(searchTerm)
                || product.Price.ToString().Contains(searchTerm))
                .ToList();

            return View("Index", searchResults);
        }

        public IActionResult Details(int id)
        {
            var product = _context.Products
                .Include(p => p.Category)
                .Include(p => p.EffectType)
                .Include(p => p.SuitableFor)
                .Include(p => p.ProductIngredients)
                    .ThenInclude(pi => pi.Ingredient)
                .Include(p => p.ProductAllergens)
                    .ThenInclude(pi => pi.Allergen)
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            ViewBag.OwnerCategory = product.Category?.NameCategory;

            return View(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            var categories = _context.Categories.ToList();
            var effectTypes = _context.EffectTypes.ToList();
            var suitableForOptions = _context.SuitableForOptions.ToList();
            var ingredients = _context.Ingredients.ToList(); // Отримуємо всі інгредієнти
            var allergens = _context.Allergens.ToList();

            ViewBag.CategoryList = new SelectList(categories, "Id", "NameCategory");
            ViewBag.EffectTypeList = new SelectList(effectTypes, "Id", "NameEffectType");
            ViewBag.SuitableForList = new SelectList(suitableForOptions, "Id", "NameSuitableFor");
            ViewBag.IngredientList = new MultiSelectList(ingredients, "Id", "Name"); // Формуємо список для множинного вибору
            ViewBag.AllergenList = new MultiSelectList(allergens, "Id", "Name");

            return View(new Product());
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(Product model, int[] selectedIngredients, int[] selectedAllergens)
        {
            if (ModelState.IsValid)
            {
                productRepository.Add(model);
                productRepository.Save();

                if (selectedIngredients != null)
                {
                    foreach (var ingredientId in selectedIngredients)
                    {
                        var productIngredient = new ProductIngredient
                        {
                            ProductId = model.Id,
                            IngredientId = ingredientId
                        };
                        _context.ProductIngredients.Add(productIngredient);
                    }
                    _context.SaveChanges();
                }

                if (selectedAllergens != null)
                {
                    foreach (var allergenId in selectedAllergens)
                    {
                        var productAllergen = new ProductAllergen
                        {
                            ProductId = model.Id,
                            AllergenId = allergenId
                        };
                        _context.ProductAllergens.Add(productAllergen);
                    }
                    _context.SaveChanges();
                }
                if (model.CoverFile != null)
                {
                    string wwwRootPath = webHostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(model.CoverFile.FileName);
                    string extension = Path.GetExtension(model.CoverFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    model.CoverPath = "/img/product/" + fileName;
                    string path = Path.Combine(wwwRootPath, "img/product", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        model.CoverFile.CopyTo(fileStream);
                    }
                    _context.SaveChanges();
                }
                
                return RedirectToAction(nameof(Index));
            }

            // Повторно заповнюємо ViewBag для повернення форми з помилками
            ViewBag.CategoryList = new SelectList(_context.Categories, "Id", "NameCategory");
            ViewBag.EffectTypeList = new SelectList(_context.EffectTypes, "Id", "NameEffectType");
            ViewBag.SuitableForList = new SelectList(_context.SuitableForOptions, "Id", "NameSuitableFor");
            ViewBag.IngredientList = new MultiSelectList(_context.Ingredients, "Id", "Name");
            ViewBag.AllergenList = new MultiSelectList(_context.Allergens, "Id", "Name");

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var product = productRepository.Get(id);
            if (product == null) return NotFound();

            return View(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = productRepository.Get(id);
            if (product != null)
            {
                productRepository.Delete(product);
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var item = productRepository.Get(id);

            if (item == null)
            {
                return NotFound();
            }
            var categories = _context.Categories.ToList();
            var effectTypes = _context.EffectTypes.ToList();
            var suitableForOptions = _context.SuitableForOptions.ToList();

            var selectedIngredients = _context.ProductIngredients
                .Where(pi => pi.ProductId == id)
                .Select(pi => pi.IngredientId)
                .ToList();

            var selectedAllergens = _context.ProductAllergens
                .Where(pa => pa.ProductId == id)
                .Select(pa => pa.AllergenId)
                .ToList();

            ViewBag.AllIngredients = _context.Ingredients.ToList();
            ViewBag.SelectedIngredients = selectedIngredients;

            ViewBag.AllAllergens = _context.Allergens.ToList();
            ViewBag.SelectedAllergens = selectedAllergens;

            ViewBag.CategoryList = new SelectList(categories, "Id", "NameCategory");
            ViewBag.EffectTypeList = new SelectList(effectTypes, "Id", "NameEffectType");
            ViewBag.SuitableForList = new SelectList(suitableForOptions, "Id", "NameSuitableFor");

            return View(item);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(Product item, int[]? selectedIngredients, int[]? selectedAllergens)
        {
            if (ModelState.IsValid)
            {
                var existingItem = productRepository.Get(item.Id);
                if (existingItem == null)
                {
                    return NotFound();
                }

                // Оновлюємо основні дані продукту
                existingItem.Name = item.Name;
                existingItem.Description = item.Description;
                existingItem.Price = item.Price;
                existingItem.CategoryId = item.CategoryId;
                existingItem.EffectTypeId = item.EffectTypeId;
                existingItem.SuitableForId = item.SuitableForId;

                // Оновлення фото
                if (item.CoverFile != null)
                {
                    string wwwRootPath = webHostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(item.CoverFile.FileName);
                    string extension = Path.GetExtension(item.CoverFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    existingItem.CoverPath = "/img/product/" + fileName;
                    string path = Path.Combine(wwwRootPath, "img/product", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        item.CoverFile.CopyTo(fileStream);
                    }
                }

                // *** Перевіряємо, чи прийшли нові інгредієнти ***
                if (selectedIngredients != null)
                {
                    var existingIngredients = _context.ProductIngredients
                        .Where(pi => pi.ProductId == item.Id)
                        .ToList();

                    // Видаляємо тільки ті, яких немає в новому списку
                    var ingredientsToRemove = existingIngredients
                        .Where(pi => !selectedIngredients.Contains(pi.IngredientId))
                        .ToList();
                    _context.ProductIngredients.RemoveRange(ingredientsToRemove);

                    // Додаємо нові інгредієнти, яких ще немає
                    foreach (var ingredientId in selectedIngredients)
                    {
                        if (!existingIngredients.Any(pi => pi.IngredientId == ingredientId))
                        {
                            _context.ProductIngredients.Add(new ProductIngredient
                            {
                                ProductId = item.Id,
                                IngredientId = ingredientId
                            });
                        }
                    }
                }

                // *** Аналогічно для алергенів ***
                if (selectedAllergens != null)
                {
                    var existingAllergens = _context.ProductAllergens
                        .Where(pa => pa.ProductId == item.Id)
                        .ToList();

                    var allergensToRemove = existingAllergens
                        .Where(pa => !selectedAllergens.Contains(pa.AllergenId))
                        .ToList();
                    _context.ProductAllergens.RemoveRange(allergensToRemove);

                    foreach (var allergenId in selectedAllergens)
                    {
                        if (!existingAllergens.Any(pa => pa.AllergenId == allergenId))
                        {
                            _context.ProductAllergens.Add(new ProductAllergen
                            {
                                ProductId = item.Id,
                                AllergenId = allergenId
                            });
                        }
                    }
                }

                _context.SaveChanges();
                productRepository.Update(existingItem);
                productRepository.Save();

                return RedirectToAction(nameof(Index));
            }

            var categories = _context.Categories.ToList();
            var effectTypes = _context.EffectTypes.ToList();
            var suitableForOptions = _context.SuitableForOptions.ToList();
            ViewBag.CategoryList = new SelectList(categories, "Id", "NameCategory");
            ViewBag.EffectTypeList = new SelectList(effectTypes, "Id", "NameEffectType");
            ViewBag.SuitableForList = new SelectList(suitableForOptions, "Id", "NameSuitableFor");

            return View(item);
        }


        public IActionResult FilterByPreferences(
    string hairType,
    string skinType,
    string avoidedAllergens,
    string effectType,
    string category,
    string[] avoidedIngredients,
    bool avoidAllergens = false)
    {
        ViewBag.HairTypes = _context.SuitableForOptions
            .Where(s => s.NameSuitableFor.Contains("волосся"))
            .ToList();
        ViewBag.SkinTypes = _context.SuitableForOptions
            .Where(s => s.NameSuitableFor.Contains("шкіра"))
            .ToList();
        ViewBag.AvoidedAllergens = _context.Allergens.ToList();
        ViewBag.EffectTypes = _context.EffectTypes.ToList();
        ViewBag.Categories = _context.Categories.ToList();
        ViewBag.Ingredients = _context.Ingredients.ToList();

        var query = _context.Products
            .Include(p => p.Category)
            .Include(p => p.SuitableFor)
            .Include(p => p.EffectType)
            .Include(p => p.ProductIngredients)
                .ThenInclude(pi => pi.Ingredient)
            .Include(p => p.ProductAllergens)
                .ThenInclude(pa => pa.Allergen)
            .AsQueryable();

        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(p => p.Category.NameCategory.Contains(category));
        }

        if (!string.IsNullOrEmpty(hairType))
        {
            query = query.Where(p => p.SuitableFor.NameSuitableFor.Contains(hairType));
        }

        if (!string.IsNullOrEmpty(skinType))
        {
            query = query.Where(p => p.SuitableFor.NameSuitableFor.Contains(skinType));
        }

        if (!string.IsNullOrEmpty(avoidedAllergens))
        {
            if (avoidAllergens)
            {
                query = query.Where(p => p.ProductAllergens.Any(pa => pa.Allergen.Name == avoidedAllergens));
            }
            else
            {
                query = query.Where(p => !p.ProductAllergens.Any(pa => pa.Allergen.Name == avoidedAllergens));
            }
        }

        if (!string.IsNullOrEmpty(effectType))
        {
            query = query.Where(p => p.EffectType.NameEffectType.Contains(effectType));
        }

        if (avoidedIngredients != null && avoidedIngredients.Length > 0)
        {
            query = query.Where(p => !p.ProductIngredients
                .Any(pi => avoidedIngredients.Contains(pi.Ingredient.Name)));
        }
        var products = query.ToList();
        return View("FilteredProducts", products);
        }
    }
}



