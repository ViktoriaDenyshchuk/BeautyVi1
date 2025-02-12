using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeautyVi.Core.Context;
using BeautyVi.Core.Entities;
using BeautyVi.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using BeautyVi.Repositories.Repos;

namespace BeautyVi.WebApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository orderRepository;
       // private readonly IWebHostEnvironment webHostEnvironment;
        private readonly BeautyViContext context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager;
        //private readonly IProductRepository _productRepository;
        public OrderController(IOrderRepository orderRepository, /*IWebHostEnvironment webHostEnviroment,*/ [FromServices] BeautyViContext context, Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager/*, IProductRepository productRepository*/)
        {
            this.orderRepository = orderRepository;
            //this.webHostEnvironment = webHostEnviroment;
            this.context = context;
            this.userManager = userManager;
           // this._productRepository = productRepository;
        }

        public IActionResult Index()
        {
            IEnumerable<Order> orders;

            if (User.IsInRole("Admin"))
            {
                orders = orderRepository.GetAll();
            }
            else
            {
                var userId = userManager.GetUserId(User);
                orders = orderRepository.GetAll().Where(order => order.UserId == userId);
            }

            return View(orders);
        }

        public IActionResult Details(int id)
         {
             var order = context.Orders
                 .Include(o => o.User)
                 .Include(o => o.OrderItems) 
                     .ThenInclude(oi => oi.Product)
                 .FirstOrDefault(o => o.Id == id);

             if (order == null)
             {
                 return NotFound();
             }
            // Перевірити, чи поточний користувач має право переглядати це замовлення
            if (userManager.GetUserId(User) != order.UserId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            // return View(orderRepository.Get(id));
            return View(order);
        }

        /* public List<SelectListItem> ProductItems { get; set; } = new List<SelectListItem>();

         [HttpGet]
         public IActionResult Create()
         {
             var products = context.Products.Select(p => new SelectListItem
             {
                 Value = p.Id.ToString(),
                 Text = p.Name,
             }).ToList();

             ProductItems = products;

             // Перевірка, чи є продукти
             //if (products == null || !products.Any())
             if (!products.Any())
             {
                 // Якщо немає продуктів, передаємо порожній список
                 ViewBag.Products = new List<SelectListItem>();
             }
             else
             {
                 ViewBag.Products = products;
             }

             var order = new Order
             {
                 UserId = userManager.GetUserId(User)
             };

             return View(order);
         }*/
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Products = context.Products
                .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name })
                .ToList();

            return View(new Order { UserId = userManager.GetUserId(User) });
        }
        [HttpPost]
        public async Task<IActionResult> Create(Order model)
        {
            // Створюємо нове замовлення
            var order = new Order
            {
                UserId = userManager.GetUserId(User),
                ShippingAddress = model.ShippingAddress,
                OrderDate = DateTime.UtcNow,
                Status = "Pending", // За замовчуванням
                TotalAmount = 0 // Потрібно буде обчислити
            };

            context.Orders.Add(order);
            await context.SaveChangesAsync(); // Збереження замовлення, щоб отримати його Id

            // Обчислення загальної суми та додавання елементів замовлення
            decimal totalAmount = 0;

            foreach (var item in model.OrderItems)
            {
                var product = await context.Products.FindAsync(item.ProductId);
                if (product != null)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = product.Price,
                        TotalPrice = item.Quantity * product.Price
                    };

                    context.OrderItems.Add(orderItem);
                    totalAmount += orderItem.TotalPrice;
                }
            }

            // Оновлення загальної суми замовлення
            order.TotalAmount = totalAmount;
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); // Перенаправлення на список замовлень

        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var item = orderRepository.Get(id);

            if (item == null)
            {
                return NotFound();
            }
            //var users = context.Users.ToList();
            //var orderItems = context.OrderItems.ToList();

            return View(item);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(Order item)
        {
            if (!ModelState.IsValid)
            {
                // Якщо модель не валідна, повертаємо форму
                return View(item);
            }

            var existingItem = orderRepository.Get(item.Id);

            if (existingItem == null)
            {
                return NotFound();
            }

            // Оновлення потрібних полів
            existingItem.Status = item.Status;

            orderRepository.Update(existingItem);
            orderRepository.Save();

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var order = orderRepository.Get(id);
            if (order == null) return NotFound();

            return View(order);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var order = orderRepository.Get(id);
            if (order != null)
            {
                orderRepository.Delete(order);
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
    }
}
