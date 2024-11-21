/*using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using BeautyVi.Repositories.Interfaces;
using BeautyVi.Repositories.Repos;
using BeautyVi.Core.Entities;
//using BeautyVi.WebApp.Data; // Переконайтеся, що цей простір імен правильний

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(); // Якщо ви використовуєте контролери з представленнями (MVC)
builder.Services.AddRazorPages(); // Якщо ви використовуєте Razor Pages

// Додавання репозиторіїв
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
//builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Конфігурація PostgreSQL як провайдера бази даних
builder.Services.AddDbContext<BeautyViContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("BeautyVi.Core") // Вкажіть збірку для міграцій
    )
);

// Додавання Identity (якщо потрібно)
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<BeautyViContext>(); // Замість ApplicationDbContext використовуйте ваш BeautyViContext

builder.Services.AddAuthentication();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddIdentityApiEndpoints<IdentityUser>()
  //  .AddEntityFrameworkStores<BeautyViContext>();
var app = builder.Build();

// Налаштування HTTP конвеєра запитів
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint(); // Якщо ви працюєте в режимі розробки
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Встановлює HSTS для продакшн-середовища
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Додавання авторизації
app.UseAuthentication();
app.UseAuthorization();

//app.MapGroup("/identity").MapIdentityApi<IdentityUser>();

// Налаштування маршрутів для контролерів і представлень
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages(); // Якщо у вас є Razor Pages

app.Run();
/*using BeautyVi.Core.Entities;
using BeautyVi.Repositories.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BeautyVi.Repositories.Repos;
using BeautyVi.Core.Context;

//using SweetCreativity1.WebApp.Hubs;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Конфігурація PostgreSQL як провайдера бази даних
builder.Services.AddDbContext<BeautyViContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("BeautyVi.Core") // Вкажіть збірку для міграцій
    )
);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();//?

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    //options.Password.RequiredLength = 4;
}).AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<BeautyViContext>();

//builder.Services.AddSingleton<UserManager<User>>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
//?
//builder.Services.AddFluentValidationAutoValidation();
// enable client-side validation
//builder.Services.AddFluentValidationClientsideAdapters();
// Load an assembly reference rather than using a marker type.
//builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

//app.MapControllerRoute(
//    name: "ConfirmDelete",
//    pattern: "{controller=Listing}/{action=DeleteConfirmed}/{id?}",
//    defaults: new { controller = "Listing", action = "DeleteConfirmed" }
//);

//app.MapControllerRoute(
//    name: "Analitic",
//    pattern: "/user/analitic",
//    defaults: new { controller = "User", action = "Analitic" });


//app.MapControllerRoute(
//    name: "MyListings",
//    pattern: "/listing/mylistings",
//    defaults: new { controller = "Listing", action = "MyListings" });

//app.MapControllerRoute(
//    name: "Favorite",
//    pattern: "/favorite/index",
//    defaults: new { controller = "Favorite", action = "Index" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
        name: "favorites",
        pattern: "{controller=Favorite}/{action=RemoveFromFavorites}/{id?}");

app.MapRazorPages();

//app.MapHub<ChatHub>("/chatHub"); // Map the ChatHub

app.Run();*/

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BeautyVi.Repositories.Interfaces;
using BeautyVi.Repositories.Repos;
using BeautyVi.Core.Entities;
using Npgsql.EntityFrameworkCore.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<BeautyViContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add services to the container.
builder.Services.AddControllersWithViews(); // Якщо ви використовуєте контролери з представленнями (MVC)
builder.Services.AddRazorPages(); // Якщо ви використовуєте Razor Pages

// Додавання репозиторіїв
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
//builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

/*builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();*/
builder.Services.AddControllersWithViews();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    //options.Password.RequiredLength = 4;
}).AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<BeautyViContext>();

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
