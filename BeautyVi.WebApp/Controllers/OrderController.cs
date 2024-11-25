
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
        //[Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var orders = orderRepository.GetAll();

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
             return View(orderRepository.Get(id));
         }
    }
}
