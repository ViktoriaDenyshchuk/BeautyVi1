////using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeautyVi.Core.Context;
using BeautyVi.Repositories.Interfaces;
using BeautyVi.Repositories.Repos;
using System.Data;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
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
        //private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager; // Specify the full namespace

        public ProductController(IProductRepository productRepository,
            IWebHostEnvironment webHostEnviroment, [FromServices] BeautyViContext context) // Specify the full namespace
        {
            this.productRepository = productRepository;
            this.webHostEnvironment = webHostEnviroment;
            this._context = context;
            //this._userManager = userManager;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        /*public IActionResult Index()
        {
            var allProducts = productRepository.GetAll();

            return View(allProducts);
        }*/
        public IActionResult Index()
        {
            // Оновіть ViewBag з правильними типами волосся і шкіри
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

        [HttpGet]
        public IActionResult Create()
        {
            var categories = _context.Categories.ToList();
            var effectTypes = _context.EffectTypes.ToList();
            var suitableForOptions = _context.SuitableForOptions.ToList();

            ViewBag.CategoryList = new SelectList(categories, "Id", "NameCategory");
            ViewBag.EffectTypeList = new SelectList(effectTypes, "Id", "NameEffectType");
            ViewBag.SuitableForList = new SelectList(suitableForOptions, "Id", "NameSuitableFor");

            return View(new Product());
        }

        [HttpPost]
        public IActionResult Create(Product model)
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
            ViewBag.CategoryList = new SelectList(categories, "Id", "NameCategory");
            ViewBag.EffectTypeList = new SelectList(effectTypes, "Id", "NameEffectType");
            ViewBag.SuitableForList = new SelectList(suitableForOptions, "Id", "NameSuitableFor");
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            var product = productRepository.Get(id);
            if (product == null) return NotFound();

            return View(product);
        }

        // Видалення продукту (POST)
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
        // GET: Product/Edit/2
        /*[HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            // Get the categories for the select list
            var categories = _context.Categories.ToList();
            ViewBag.CategoryList = new SelectList(categories, "Id", "NameCategory");

            return View(product);
        }

        // POST: Product/Edit/2
        [HttpPost]
        public IActionResult Edit(Product model)
        {
            if (ModelState.IsValid)
            {
                _context.Update(model); // Update the product in the context
                _context.SaveChanges(); // Save changes to the database

                return RedirectToAction(nameof(Index)); // Redirect to the product list
            }

            // If the model state is not valid, return the same view with validation errors
            var categories = _context.Categories.ToList();
            ViewBag.CategoryList = new SelectList(categories, "Id", "NameCategory");

            return View(model);
        }
        */

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
            ViewBag.CategoryList = new SelectList(categories, "Id", "NameCategory");
            ViewBag.EffectTypeList = new SelectList(effectTypes, "Id", "NameEffectType");
            ViewBag.SuitableForList = new SelectList(suitableForOptions, "Id", "NameSuitableFor");

            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(Product item)
        {
            if (ModelState.IsValid)
            {
                try
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

                            productRepository.Update(existingItem);
                            productRepository.Save();

                            return RedirectToAction(nameof(Index));
                        }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception
                    return View(item);
                }
            }
            // If ModelState is not valid, return to the edit view
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
    string[] avoidedIngredients)
        {
            // Ініціалізація даних для фільтра
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

            // Побудова запиту
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
                query = query.Where(p => p.ProductAllergens.Any(pa => pa.Allergen.Name.Contains(avoidedAllergens)));
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

            // Виконання запиту та повернення результатів
            var products = query.ToList();
            return View("FilteredProducts", products);
        }






    }
}



