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
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BeautyVi.Core.Entities;
using System.Reflection;

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

        public IActionResult Index()
        {
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
                || product.Price.ToString().Contains(searchTerm))
                .ToList();

            return View("Index", searchResults);
        }

       
        public IActionResult Details(int id)
        {
            var product = _context.Products
                .Include(p => p.Category)
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
            // Отримайте список категорій з бази даних
            var categories = _context.Categories.ToList();

            // Передайте список категорій у ваше представлення
            ViewBag.CategoryList = new SelectList(categories, "Id", "NameCategory");

            // Створіть пустий об'єкт Listing та передайте його у представлення
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
            ViewBag.CategoryList = new SelectList(categories, "Id", "NameCategory");
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
                ViewBag.CategoryList = new SelectList(categories, "Id", "NameCategory");

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
            ViewBag.CategoryList = new SelectList(categories, "Id", "NameCategory");
            return View(item);
        }
    }
}



