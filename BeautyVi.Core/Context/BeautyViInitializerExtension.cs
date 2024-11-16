/*using Microsoft.EntityFrameworkCore;
using BeautyVi.Core.Entities;

namespace BeautyVi.Core.Context
{
    public static class BeautyViInitializerExtension
    {
        public static void Seed(this ModelBuilder builder)
        {
            seedUsers(builder);
            seedCategories(builder);
            seedProducts(builder);
            seedIngredients(builder);
            seedUserPreferences(builder);
            seedAllergens(builder);
            seedProductAllergens(builder);
            seedProductIngredients(builder);
            seedOrders(builder);
            seedOrderItems(builder);
            seedProductRecommendations(builder);
        }

        private static void seedUsers(ModelBuilder builder)
        {
            builder.Entity<User>().HasData(
                new User
                {
                    //Id = 1,
                    Name = "John Doe",
                    Email = "john.doe@example.com",
                    Password = "password123"
                },
                new User
                {
                    //Id = 2,
                    Name = "Jane Smith",
                    Email = "jane.smith@example.com",
                    Password = "jane_password"
                }
            );
        }
        private static void seedCategories(ModelBuilder builder)
        {
            builder.Entity<Category>().HasData(
                new Category { Id = 1, NameCategory = "Skincare" },
                new Category { Id = 2, NameCategory = "Makeup" },
                new Category { Id = 3, NameCategory = "Haircare" }
            );
        }
        private static void seedProducts(ModelBuilder builder)
        {
            builder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Hydrating Facial Cream", Description = "A deeply moisturizing facial cream.", Price = 25.99m, CategoryId = 1 },
                new Product { Id = 2, Name = "Matte Lipstick", Description = "A long-lasting matte lipstick.", Price = 15.50m, CategoryId = 2 }
            );
        }
        private static void seedIngredients(ModelBuilder builder)
        {
            builder.Entity<Ingredient>().HasData(
                new Ingredient { Id = 1, Name = "Aloe Vera", IsHarmful = false },
                new Ingredient { Id = 2, Name = "Parabens", IsHarmful = true },
                new Ingredient { Id = 3, Name = "Vitamin E", IsHarmful = false }
            );
        }
        private static void seedUserPreferences(ModelBuilder builder)
        {
            builder.Entity<UserPreferences>().HasData(
                new UserPreferences { Id = 1, UserId = 1, HairType = "Curly", SkinType = "Dry", AvoidAllergens = true, AvoidedAllergens = "Parabens" },
                new UserPreferences { Id = 2, UserId = 2, HairType = "Straight", SkinType = "Oily", AvoidAllergens = false, AvoidedAllergens = " " }
            );
        }
        private static void seedAllergens(ModelBuilder builder)
        {
            builder.Entity<Allergen>().HasData(
                new Allergen { Id = 1, Name = "Fragrance" },
                new Allergen { Id = 2, Name = "Parabens" },
                new Allergen { Id = 3, Name = "Sulfates" }
            );
        }

        private static void seedProductAllergens(ModelBuilder builder)
        {
            builder.Entity<ProductAllergen>().HasData(
                new ProductAllergen { ProductId = 1, AllergenId = 2 }, 
                new ProductAllergen { ProductId = 2, AllergenId = 3 }  
            );
        }

        private static void seedProductIngredients(ModelBuilder builder)
        {
            builder.Entity<ProductIngredient>().HasData(
                new ProductIngredient { ProductId = 1, IngredientId = 1 }, 
                new ProductIngredient { ProductId = 1, IngredientId = 3 }, 
                new ProductIngredient { ProductId = 2, IngredientId = 2 }  
            );
        }
        private static void seedOrders(ModelBuilder builder)
        {
            builder.Entity<Order>().HasData(
                new Order { Id = 1, UserId = 1, OrderDate = new DateTime(2024, 11, 8, 12, 0, 0, DateTimeKind.Utc), Status = "Completed", TotalAmount = 50.00m, ShippingAddress = "123 Main St" },
                new Order { Id = 2, UserId = 2, OrderDate = new DateTime(2024, 10, 7, 11, 0, 0, DateTimeKind.Utc), Status = "Pending", TotalAmount = 20.00m, ShippingAddress = "456 Elm St" }
            );
        }
        private static void seedOrderItems(ModelBuilder builder)
        {
            builder.Entity<OrderItem>().HasData(
                new OrderItem { Id = 1, OrderId = 1, ProductId = 1, Quantity = 1, UnitPrice = 25.99m, TotalPrice = 25.99m },
                new OrderItem { Id = 2, OrderId = 1, ProductId = 2, Quantity = 1, UnitPrice = 15.50m, TotalPrice = 15.50m },
                new OrderItem { Id = 3, OrderId = 2, ProductId = 2, Quantity = 2, UnitPrice = 15.50m, TotalPrice = 31.00m }
            );
        }
        private static void seedProductRecommendations(ModelBuilder builder)
        {
            builder.Entity<ProductRecommendation>().HasData(
                new ProductRecommendation { Id = 1, UserId = 1, ProductId = 2 }, // Recommend product 2 to user 1
                new ProductRecommendation { Id = 2, UserId = 2, ProductId = 1 }  // Recommend product 1 to user 2
            );
        }
    }
}
*/
/*using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BeautyVi.Core.Entities;
using System;

namespace BeautyVi.Core.Context
{
    public static class BeautyViInitializerExtension
    {
        public static void Seed(this ModelBuilder builder)
        {
            (string adminId, string clientId) = SeedUsersAndRoles(builder);
            SeedCategories(builder);
            SeedProducts(builder);
            SeedOrders(builder, new string[] { clientId });
        }

        private static (string, string) SeedUsersAndRoles(ModelBuilder builder)
        {
            // Створення ролей
            string ADMIN_ROLE_ID = Guid.NewGuid().ToString();
            string CLIENT_ROLE_ID = Guid.NewGuid().ToString();

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = ADMIN_ROLE_ID, Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = CLIENT_ROLE_ID, Name = "Client", NormalizedName = "CLIENT" }
            );

            // Створення користувачів
            string ADMIN_ID = Guid.NewGuid().ToString();
            string CLIENT_ID = Guid.NewGuid().ToString();

            var admin = new User
            {
                Id = ADMIN_ID,
                Name = "admin@beautyvi.com",
                Email = "admin@beautyvi.com",
                EmailConfirmed = true,
                NormalizedEmail = "ADMIN@BEAUTYVI.COM",
                NormalizedUserName = "ADMIN@BEAUTYVI.COM"
            };

            var client = new User
            {
                Id = CLIENT_ID,
                Name = "client@beautyvi.com",
                Email = "client@beautyvi.com",
                EmailConfirmed = true,
                NormalizedEmail = "CLIENT@BEAUTYVI.COM",
                NormalizedUserName = "CLIENT@BEAUTYVI.COM"
            };

            PasswordHasher<User> hasher = new PasswordHasher<User>();
            admin.PasswordHash = hasher.HashPassword(admin, "admin$pass");
            client.PasswordHash = hasher.HashPassword(client, "client$pass");

            builder.Entity<User>().HasData(admin, client);

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = ADMIN_ROLE_ID,
                    UserId = ADMIN_ID
                },
                new IdentityUserRole<string>
                {
                    RoleId = CLIENT_ROLE_ID,
                    UserId = CLIENT_ID
                }
            );

            return (ADMIN_ID, CLIENT_ID);
        }

        private static void SeedCategories(ModelBuilder builder)
        {
            builder.Entity<Category>().HasData(
                new Category { Id = 1, NameCategory = "Догляд за шкірою" },
                new Category { Id = 2, NameCategory = "Догляд за волоссям" },
                new Category { Id = 3, NameCategory = "Макіяж" }
            );
        }

        private static void SeedProducts(ModelBuilder builder)
        {
            builder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Зволожуючий крем",
                    Description = "Крем для інтенсивного зволоження шкіри.",
                    Price = 499.99m,
                    CategoryId = 1
                },
                new Product
                {
                    Id = 2,
                    Name = "Шампунь для сухого волосся",
                    Description = "Відновлюючий шампунь для сухого та пошкодженого волосся.",
                    Price = 299.99m,
                    CategoryId = 2
                }
            );
        }

        private static void SeedOrders(ModelBuilder builder, string[] clientIds)
        {
            builder.Entity<Order>().HasData(
                new Order
                {
                    Id = 1,
                    UserId = clientIds[0],
                    OrderDate = DateTime.Now,
                    Status = "Completed",
                    TotalAmount = 50.00m,
                    ShippingAddress = "123 Main St"
                },
                new Order
                {
                    Id = 2,
                    UserId = clientIds[1],
                    OrderDate = DateTime.Now,
                    Status = "Pending",
                    TotalAmount = 20.00m,
                    ShippingAddress = "456 Elm St"
                }
            );
        }

    }
}
*/

using BeautyVi.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BeautyVi.Core.Context
{
    public static class BeautyViInitializerExtension
    {
        public static void Seed(this ModelBuilder builder)
        {
            (string admId, string clId) = seedUsersAndRoles(builder);
            seedProduct(builder);
            seedCategory(builder);
            // seedUser(builder);
            //seedOrder(builder, new string[] { admId });
        }

        private static void seedCategory(ModelBuilder builder)
        {
            builder.Entity<Category>().HasData(
                 new Category
                 {
                     Id = 1,
                     NameCategory = "Догляд за шкірою"
                 },
                  new Category
                  {
                      Id = 2,
                      NameCategory = "Догляд за волоссям"
                  },
                  new Category
                  {
                      Id = 3,
                      NameCategory = "Макіяж"
                  }
                 );
        }
        
        static void seedProduct(ModelBuilder builder)
        {
            builder.Entity<Product>().HasData(
              new Product
              {
                  Id = 1,
                  Name = "Зволожуючий крем",
                  Description = "Крем для інтенсивного зволоження шкіри.",
                  Price = 499.99m,
                  CategoryId = 1
              },
                new Product
                {
                    Id = 2,
                    Name = "Шампунь для сухого волосся",
                    Description = "Відновлюючий шампунь для сухого та пошкодженого волосся.",
                    Price = 299.99m,
                    CategoryId = 2
                }
            );
        }

        static (string, string) seedUsersAndRoles(ModelBuilder builder)
        {
            string CLIENT_ROLE_ID = Guid.NewGuid().ToString();
            string ADMIN_ROLE_ID = Guid.NewGuid().ToString();

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = CLIENT_ROLE_ID, Name = "Client", NormalizedName = "CLIENT" },
                new IdentityRole { Id = ADMIN_ROLE_ID, Name = "Admin", NormalizedName = "ADMIN" }
                );

            string CLIENT_ID = Guid.NewGuid().ToString();
            string ADMIN_ID = Guid.NewGuid().ToString();

            var client = new User
            {
                Id = CLIENT_ID,
                UserName = "client@beautyvi.com",
                Email = "client@beautyvi.com",
                FullName = "Olena Demchuk",
                EmailConfirmed = true,
                NormalizedEmail = "CLIENT@BEAUTYVI.COM",
                NormalizedUserName = "CLIENT@BEAUTYVI.COM"
            };

            var admin = new User
            {
                Id = ADMIN_ID,
                UserName = "admin@beautyvi.com",
                Email = "admin@beautyvi.com",
                FullName = "Alla Svoboda",
                EmailConfirmed = true,
                NormalizedEmail = "ADMIN@BEAUTYVI.COM",
                NormalizedUserName = "ADMIN@BEAUTYVI.COM"
            };

            PasswordHasher<User> hasher = new PasswordHasher<User>();
            client.PasswordHash = hasher.HashPassword(client, "client$pass");
            admin.PasswordHash = hasher.HashPassword(admin, "admin$pass");

            builder.Entity<User>().HasData(client, admin);

            builder.Entity<IdentityUserRole<string>>().HasData(

                new IdentityUserRole<string>
                {
                    RoleId = ADMIN_ROLE_ID,
                    UserId = ADMIN_ID
                },

                new IdentityUserRole<string>
                {
                    RoleId = CLIENT_ROLE_ID,
                    UserId = CLIENT_ID
                }
                );
            return (CLIENT_ID, ADMIN_ID);
        }
        /*private static void seedOrder(ModelBuilder builder, string[] clientIds)
        {
            builder.Entity<Order>().HasData(
                new Order
                {
                    Id = 1,
                    UserId = clientIds[0],
                    OrderDate = DateTime.Now,
                    Status = "Completed",
                    TotalAmount = 50.00m,
                    ShippingAddress = "123 Main St"
                },
                new Order
                {
                    Id = 2,
                    UserId = clientIds[1],
                    OrderDate = DateTime.Now,
                    Status = "Pending",
                    TotalAmount = 20.00m,
                    ShippingAddress = "456 Elm St"
                }
            );
        }
        */
    }
}


