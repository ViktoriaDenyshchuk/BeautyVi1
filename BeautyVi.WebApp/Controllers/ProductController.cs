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
            var ingredients = _context.Ingredients.ToList();

            ViewBag.CategoryList = new SelectList(categories, "Id", "NameCategory");
            ViewBag.EffectTypeList = new SelectList(effectTypes, "Id", "NameEffectType");
            ViewBag.SuitableForList = new SelectList(suitableForOptions, "Id", "NameSuitableFor");
            //ViewBag.ProductIngredientList = new SelectList(ingredients, "Id", "NameIngredient");
            ViewBag.IngredientList = ingredients;
            return View(new Product());
        }

        /*[Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(Product model, int[] ProductIngredients)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = webHostEnvironment.WebRootPath;

                string fileName = Path.GetFileNameWithoutExtension(model.CoverFile.FileName);

                string extension = Path.GetExtension(model.CoverFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                model.CoverPath = "/img/product/" + fileName;
                string path = Path.Combine(wwwRootPath + "/img/product/", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    model.CoverFile.CopyTo(fileStream);
                }

                productRepository.Add(model);
                

                return RedirectToAction(nameof(Index));
            }
            var categories = _context.Categories.ToList();
            var effectTypes = _context.EffectTypes.ToList();
            var suitableForOptions = _context.SuitableForOptions.ToList();
            var ingredients = _context.ProductIngredients.ToList();
            ViewBag.CategoryList = new SelectList(categories, "Id", "NameCategory");
            ViewBag.EffectTypeList = new SelectList(effectTypes, "Id", "NameEffectType");
            ViewBag.SuitableForList = new SelectList(suitableForOptions, "Id", "NameSuitableFor");
            ViewBag.ProductIngredientList = new SelectList(ingredients, "Id", "NameIngredient");

            return View(model);
        }*/
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(Product model, int[] ProductIngredients, int[] ProductAllergens)
        {
            if (ModelState.IsValid)
            {
                // Збереження основних даних продукту
                string wwwRootPath = webHostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(model.CoverFile.FileName);
                string extension = Path.GetExtension(model.CoverFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                model.CoverPath = "/img/product/" + fileName;
                string path = Path.Combine(wwwRootPath + "/img/product/", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    model.CoverFile.CopyTo(fileStream);
                }

                // Додаємо зв'язки для інгредієнтів
                if (ProductIngredients != null)
                {
                    foreach (var ingredientId in ProductIngredients)
                    {
                        _context.ProductIngredients.Add(new ProductIngredient
                        {
                            ProductId = model.Id,
                            IngredientId = ingredientId
                        });
                    }
                }

                // Додаємо зв'язки для алергенів
                if (ProductAllergens != null)
                {
                    foreach (var allergenId in ProductAllergens)
                    {
                        _context.ProductAllergens.Add(new ProductAllergen
                        {
                            ProductId = model.Id,
                            AllergenId = allergenId
                        });
                    }
                }

                _context.SaveChanges(); // Збереження всіх даних у базі
                return RedirectToAction(nameof(Index));
            }

            // Перевірка та перенесення ViewBag у випадку помилки валідації
            ViewBag.CategoryList = new SelectList(_context.Categories, "Id", "NameCategory");
            ViewBag.EffectTypeList = new SelectList(_context.EffectTypes, "Id", "NameEffectType");
            ViewBag.SuitableForList = new SelectList(_context.SuitableForOptions, "Id", "NameSuitableFor");
            ViewBag.IngredientList = new SelectList(_context.Ingredients, "Id", "NameIngredient");
            ViewBag.AllergenList = new SelectList(_context.Allergens, "Id", "NameAllergen");

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
            var ingredients = _context.Ingredients.ToList();
            var allergens = _context.Ingredients.ToList();

            ViewBag.CategoryList = new SelectList(categories, "Id", "NameCategory");
            ViewBag.EffectTypeList = new SelectList(effectTypes, "Id", "NameEffectType");
            ViewBag.SuitableForList = new SelectList(suitableForOptions, "Id", "NameSuitableFor");
            ViewBag.IngredientList = new SelectList(ingredients, "Id", "NameIngredient");
            ViewBag.AllergenList = new SelectList(allergens, "Id", "NameAllergen");

            return View(item);
        }

        /*[Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(Product item)
        {
            if (ModelState.IsValid)
            {

                var existingItem = productRepository.Get(item.Id);

                if (existingItem != null)
                {
                    existingItem.Name = item.Name;
                    existingItem.Description = item.Description;
                    existingItem.Price = item.Price;
                    existingItem.Category = item.Category;
                    existingItem.EffectType = item.EffectType;
                    existingItem.SuitableFor = item.SuitableFor;

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
                    existingItem.CategoryId = item.CategoryId;
                    existingItem.EffectTypeId = item.EffectTypeId;
                    existingItem.SuitableForId = item.SuitableForId;
                    // Оновлення інгредієнтів та алергенів
                    existingItem.ProductIngredients = item.ProductIngredients; // Прив'язуємо вибрані інгредієнти
                    existingItem.ProductAllergens = item.ProductAllergens; // Прив'язуємо вибрані алергени

                    productRepository.Update(existingItem);
                    productRepository.Save();

                    return RedirectToAction(nameof(Index));
                }
            else
            {
                return NotFound();
            }
        }
        var categories = _context.Categories.ToList();
        var effectTypes = _context.EffectTypes.ToList();
        var suitableForOptions = _context.SuitableForOptions.ToList();
        ViewBag.CategoryList = new SelectList(categories, "Id", "NameCategory");
        ViewBag.EffectTypeList = new SelectList(effectTypes, "Id", "NameEffectType");
        ViewBag.SuitableForList = new SelectList(suitableForOptions, "Id", "NameSuitableFor");
        ViewBag.IngredientList = new SelectList(_context.Ingredients, "Id", "NameIngredient");
        ViewBag.AllergenList = new SelectList(_context.Allergens, "Id", "NameAllergen");

        return View(item);
    }*/
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(Product item, int[] ProductIngredients, int[] ProductAllergens)
        {
            if (ModelState.IsValid)
            {
                var existingItem = productRepository.Get(item.Id);

                if (existingItem != null)
                {
                    existingItem.Name = item.Name;
                    existingItem.Description = item.Description;
                    existingItem.Price = item.Price;
                    existingItem.Category = item.Category;
                    existingItem.EffectType = item.EffectType;
                    existingItem.SuitableFor = item.SuitableFor;

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

                    existingItem.ProductIngredients = ProductIngredients.Select(id => new ProductIngredient
                    {
                        ProductId = item.Id,
                        IngredientId = id
                    }).ToList();

                    existingItem.ProductAllergens = ProductAllergens.Select(id => new ProductAllergen
                    {
                        ProductId = item.Id,
                        AllergenId = id
                    }).ToList();

                    productRepository.Update(existingItem);
                    productRepository.Save();

                    return RedirectToAction(nameof(Index));
                }
            }

            var categories = _context.Categories.ToList();
            var effectTypes = _context.EffectTypes.ToList();
            var suitableForOptions = _context.SuitableForOptions.ToList();
            ViewBag.CategoryList = new SelectList(categories, "Id", "NameCategory");
            ViewBag.EffectTypeList = new SelectList(effectTypes, "Id", "NameEffectType");
            ViewBag.SuitableForList = new SelectList(suitableForOptions, "Id", "NameSuitableFor");
            ViewBag.IngredientList = new SelectList(_context.Ingredients, "Id", "NameIngredient");
            ViewBag.AllergenList = new SelectList(_context.Allergens, "Id", "NameAllergen");

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



