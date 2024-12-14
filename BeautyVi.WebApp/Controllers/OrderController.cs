﻿
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeautyVi.Core.Context;
using BeautyVi.Core.Entities;
using BeautyVi.Repositories.Interfaces;
using BeautyVi.Repositories.Repos;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
using Microsoft.AspNetCore.Http.HttpResults;


namespace BeautyVi.WebApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository orderRepository;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly BeautyViContext _context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _userManager;
        //private readonly IProductRepository _productRepository;
        public OrderController(IOrderRepository orderRepository, IWebHostEnvironment webHostEnviroment, [FromServices] BeautyViContext context, Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager/*, IProductRepository productRepository*/)
        {
            this.orderRepository = orderRepository;
            this.webHostEnvironment = webHostEnviroment;
            this._context = context;
            this._userManager = userManager;
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
                var userId = _userManager.GetUserId(User);
                orders = orderRepository.GetAll().Where(order => order.UserId == userId);
            }

            return View(orders);
        }

        //[Authorize(Roles = "Client")]
        /*[HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //[Authorize(Roles = "Client")]
        [HttpPost]
        public IActionResult Create(Order order, int productId)
        {
            if (ModelState.IsValid)
            {
                order.UserId = _userManager.GetUserId(User);
                order.OrderDate = DateTime.UtcNow;
                order.Status = "Pending"; // Статус за замовчуванням
                orderRepository.Add(order);
                return RedirectToAction(nameof(Index));
            }

            return View(order);
        }*/

        public IActionResult Details(int id)
         {
             var order = _context.Orders
                 .Include(o => o.User)
                 .Include(o => o.OrderItems) 
                     .ThenInclude(oi => oi.Product)
                 .FirstOrDefault(o => o.Id == id);

             if (order == null)
             {
                 return NotFound();
             }
            // Перевірити, чи поточний користувач має право переглядати це замовлення
            if (_userManager.GetUserId(User) != order.UserId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            return View(orderRepository.Get(id));
         }

        public List<SelectListItem> ProductItems { get; set; } = new List<SelectListItem>();


        [HttpGet]
        public IActionResult Create()
        {
            // Отримуємо список всіх користувачів
            var users = _userManager.Users.Select(u => new SelectListItem
            {
                Value = u.Id,
                Text = u.UserName
            }).ToList();

            // Отримуємо список продуктів для випадаючого списку
            var products = _context.Products.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();

            ProductItems = products;

            // Перевірка, чи є продукти
            if (products == null || !products.Any())
            {
                // Якщо немає продуктів, передаємо порожній список
                ViewBag.Products = new List<SelectListItem>();
            }
            else
            {
                ViewBag.Products = products;
            }

            // Передаємо список користувачів в ViewBag
            ViewBag.Users = users;

            // Повертаємо порожній об'єкт Order
            return View();
        }

        // POST: Order/Create
        [HttpPost]
        /*[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order model)
        {
            if (ModelState.IsValid)
            {
                // Створюємо нове замовлення
                var order = new Order
                {
                    UserId = model.UserId,
                    ShippingAddress = model.ShippingAddress,
                    OrderDate = DateTime.UtcNow,
                    Status = "Pending", // За замовчуванням
                    TotalAmount = 0 // Потрібно буде обчислити
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync(); // Збереження замовлення, щоб отримати його Id

                // Обчислення загальної суми та додавання елементів замовлення
                decimal totalAmount = 0;

                foreach (var item in model.OrderItems)
                {
                    var product = await _context.Products.FindAsync(item.ProductId);
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

                        _context.OrderItems.Add(orderItem);
                        totalAmount += orderItem.TotalPrice;
                    }
                }

                // Оновлення загальної суми замовлення
                order.TotalAmount = totalAmount;
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index)); // Перенаправлення на список замовлень
            }

            // Якщо модель не валідна, повертаємо користувачу форму
            return View(model);
        }*/
        public async Task<IActionResult> Create(Order model)
        {
            // Створюємо нове замовлення
            var order = new Order
            {
                UserId = model.UserId,
                ShippingAddress = model.ShippingAddress,
                OrderDate = DateTime.UtcNow,
                Status = "Pending", // За замовчуванням
                TotalAmount = 0 // Потрібно буде обчислити
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync(); // Збереження замовлення, щоб отримати його Id

            // Обчислення загальної суми та додавання елементів замовлення
            decimal totalAmount = 0;

            foreach (var item in model.OrderItems)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
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

                    _context.OrderItems.Add(orderItem);
                    totalAmount += orderItem.TotalPrice;
                }
            }

            // Оновлення загальної суми замовлення
            order.TotalAmount = totalAmount;
            await _context.SaveChangesAsync();

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
            var users = _context.Users.ToList();
            var orderItems = _context.OrderItems.ToList();

            return View(item);
        }

        /*[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(Order order)
        {
            if (!ModelState.IsValid)
            {
                return View(order);
            }

            var existingOrder = await _context.Orders.FindAsync(order.Id);
            if (existingOrder == null)
            {
                return NotFound();
            }

            // Оновлюємо тільки статус
            existingOrder.Status = order.Status;

            orderRepository.Update(existingOrder);
            orderRepository.Save();

            return RedirectToAction(nameof(Index));
        }*/
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

            // Не оновлюємо OrderItems, якщо це не передається через форму
            // existingItem.OrderItems = item.OrderItems;

            orderRepository.Update(existingItem);
            orderRepository.Save();

            return RedirectToAction(nameof(Index));
        }





    }
}
