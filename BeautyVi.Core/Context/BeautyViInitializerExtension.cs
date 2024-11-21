
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
            seedOrder(builder, new string[] { admId });
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
                //FullName = "Olena Demchuk",
                EmailConfirmed = true,
                NormalizedEmail = "CLIENT@BEAUTYVI.COM",
                NormalizedUserName = "CLIENT@BEAUTYVI.COM"
            };

            var admin = new User
            {
                Id = ADMIN_ID,
                UserName = "admin@beautyvi.com",
                Email = "admin@beautyvi.com",
                //FullName = "Alla Svoboda",
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
        private static void seedOrder(ModelBuilder builder, string[] clientIds)
        {
            builder.Entity<Order>().HasData(
                new Order
                {
                    Id = 1,
                    UserId = clientIds[0],
                    OrderDate = DateTime.UtcNow,
                    Status = "Completed",
                    TotalAmount = 50.00m,
                    ShippingAddress = "123 Main St"
                }
            );
        }
        
    }
}
