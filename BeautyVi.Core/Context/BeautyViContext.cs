using System.Net.NetworkInformation;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using BeautyVi.Core.Entities;
using BeautyVi.Core.Context;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

public class BeautyViContext : IdentityDbContext<IdentityUser>
{
    public BeautyViContext(DbContextOptions<BeautyViContext> options)
        : base(options)
    {

    }

    //public DbSet<User> Users => Set<User>();
    //public DbSet<UserPreferences> UsersPreferences => Set<UserPreferences>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Allergen> Allergens => Set<Allergen>();
    public DbSet<ProductAllergen> ProductAllergens => Set<ProductAllergen>();
    public DbSet<Ingredient> Ingredients => Set<Ingredient>();
    public DbSet<ProductIngredient> ProductIngredients => Set<ProductIngredient>();
    public DbSet<ProductRecommendation> ProductRecommendations => Set<ProductRecommendation>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Зв'язок між User та UserPreferences (1 до 1)
        builder.Entity<User>()
            .HasOne(u => u.UserPreferences)
            .WithOne(p => p.User)
            .HasForeignKey<UserPreferences>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Зв'язок між Product та Category (багато до одного)
        builder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Зв'язок між Order та User (багато до одного)
        builder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        //.OnDelete(DeleteBehavior.Cascade);

        // Зв'язок між OrderItem та Order (багато до одного)
        builder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Зв'язок між OrderItem та Product (багато до одного)
        builder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        // Зв'язок між ProductIngredient (багато до багатьох)
        builder.Entity<ProductIngredient>()
            .HasKey(pi => new { pi.ProductId, pi.IngredientId });

        builder.Entity<ProductIngredient>()
            .HasOne(pi => pi.Product)
            .WithMany(p => p.ProductIngredients)
            .HasForeignKey(pi => pi.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ProductIngredient>()
            .HasOne(pi => pi.Ingredient)
            .WithMany(i => i.ProductIngredients)
            .HasForeignKey(pi => pi.IngredientId)
            .OnDelete(DeleteBehavior.Cascade);

        // Зв'язок між Product та Allergen (багато до багатьох через ProductAllergen)
        builder.Entity<ProductAllergen>()
            .HasKey(pa => new { pa.ProductId, pa.AllergenId });

        builder.Entity<ProductAllergen>()
            .HasOne(pa => pa.Product)
            .WithMany(p => p.ProductAllergens)
            .HasForeignKey(pa => pa.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ProductAllergen>()
            .HasOne(pa => pa.Allergen)
            .WithMany(a => a.ProductAllergens)
            .HasForeignKey(pa => pa.AllergenId)
            .OnDelete(DeleteBehavior.Cascade);

        // Зв'язок між ProductRecommendation та User (багато до одного)
        builder.Entity<ProductRecommendation>()
            .HasOne(pr => pr.User)
            .WithMany(u => u.ProductRecommendations)
            .HasForeignKey(pr => pr.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Зв'язок між ProductRecommendation та Product (багато до одного)
        builder.Entity<ProductRecommendation>()
            .HasOne(pr => pr.Product)
            .WithMany(p => p.ProductRecommendations)
            .HasForeignKey(pr => pr.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        // Унікальний індекс для поля NameCategory у таблиці Category
        builder.Entity<Category>()
            .HasIndex(c => c.NameCategory)
            .IsUnique();

        // Вимоги до поля Email у таблиці User
       builder.Entity<User>()
            .Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256)
            .IsUnicode(false);

        base.OnModelCreating(builder);
        builder.Seed();

    }
}
