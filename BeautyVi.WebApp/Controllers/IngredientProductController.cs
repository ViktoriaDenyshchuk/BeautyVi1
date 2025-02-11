using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BeautyVi.Core.Context;
using BeautyVi.Core.Entities;
using System.Linq;

namespace BeautyVi.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class IngredientProductController : Controller
    {
        private readonly BeautyViContext _context;

        public IngredientProductController(BeautyViContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult AddIngredientsToProduct(int productId, int[] selectedIngredients)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
            {
                return NotFound();
            }

            // Видаляємо старі зв’язки (щоб уникнути дублювань)
            var existingIngredients = _context.ProductIngredients.Where(pi => pi.ProductId == productId);
            _context.ProductIngredients.RemoveRange(existingIngredients);
            _context.SaveChanges();

            // Додаємо нові зв’язки
            if (selectedIngredients != null)
            {
                foreach (var ingredientId in selectedIngredients)
                {
                    _context.ProductIngredients.Add(new ProductIngredient
                    {
                        ProductId = productId,
                        IngredientId = ingredientId
                    });
                }
                _context.SaveChanges();
            }

            return RedirectToAction("Edit", "Product", new { id = productId });
        }
    }
}
