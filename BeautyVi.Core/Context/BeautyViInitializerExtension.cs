
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
            seedEffectType(builder);
            seedSuitableFor(builder);
            seedIngredient(builder);
            seedAllergen(builder);
            // seedUser(builder);
            seedProductIngredients(builder);
            seedProductAllergens(builder);
            seedOrder(builder, new string[] { admId }, new int[] { 1, 2});
        }

        private static void seedCategory(ModelBuilder builder)
        {
            builder.Entity<Category>().HasData(
                 new Category
                 {
                     Id = 1,
                     NameCategory = "Волосся"
                 },
                  new Category
                  {
                      Id = 2,
                      NameCategory = "Шкіра"
                  },
                  new Category
                  {
                      Id = 3,
                      NameCategory = "Лице"
                  },
                  new Category
                  {
                      Id = 4,
                      NameCategory = "Тіло"
                  }
                 );
        }
        private static void seedEffectType(ModelBuilder builder)
        {
            builder.Entity<EffectType>().HasData(
                 new EffectType
                 {
                     Id = 1,
                     NameEffectType = "Зволоження"
                 },
                  new EffectType
                  {
                      Id = 2,
                      NameEffectType = "Живлення"
                  },
                  new EffectType
                  {
                      Id = 3,
                      NameEffectType = "Проти старіння"
                  },
                  new EffectType
                  {
                      Id = 4,
                      NameEffectType = "Очищення"
                  },
                  new EffectType
                  {
                      Id = 5,
                      NameEffectType = "Відновлення"
                  }
                 );
        }
        private static void seedSuitableFor(ModelBuilder builder)
        {
            builder.Entity<SuitableFor>().HasData(
                 new SuitableFor
                 {
                     Id = 1,
                     NameSuitableFor = "Суха шкіра"
                 },
                  new SuitableFor
                  {
                      Id = 2,
                      NameSuitableFor = "Жирна шкіра"
                  },
                  new SuitableFor
                  {
                      Id = 3,
                      NameSuitableFor = "Чутлива шкіра"
                  },
                  new SuitableFor
                  {
                      Id = 4,
                      NameSuitableFor = "Нормальне волосся"
                  },
                  new SuitableFor
                  {
                      Id = 5,
                      NameSuitableFor = "Пошкоджене волосся"
                  }
                 );
        }
        private static void seedAllergen(ModelBuilder builder)
        {
            builder.Entity<Allergen>().HasData(
                new Allergen
                {
                    Id = 1,
                    Name = "Консерванти (Парабени)"
                },
                new Allergen
                {
                    Id = 2,
                    Name = "Ланолін"
                },
                new Allergen
                {
                    Id = 3,
                    Name = "Альфа-гідроксикислоти (AHAs)"
                },
                new Allergen
                {
                    Id = 4,
                    Name = "Лаванда"
                }
            );
        }
        private static void seedIngredient(ModelBuilder builder)
        {
            builder.Entity<Ingredient>().HasData(
                new Ingredient
                {
                    Id = 1,
                    Name = "Вітамін C",
                    Category = "Активні інгредієнти",
                    LevelOfDanger = 1,
                    IsHarmful = false,
                    Description = "Вітамін C допомагає зволожувати шкіру та підвищує її еластичність."
                },
                new Ingredient
                {
                    Id = 2,
                    Name = "Саліцилова кислота",
                    Category = "Активні інгредієнти",
                    LevelOfDanger = 2,
                    IsHarmful = false,
                    Description = "Саліцилова кислота допомагає в боротьбі з акне."
                },
                new Ingredient
                {
                    Id = 3,
                    Name = "Парабени",
                    Category = "Консерванти",
                    LevelOfDanger = 4,
                    IsHarmful = true,
                    Description = "Парабени використовуються для продовження терміну зберігання, але можуть викликати алергії."
                },
                new Ingredient
                {
                    Id = 4,
                    Name = "Ментол",
                    Category = "Активні інгредієнти",
                    LevelOfDanger = 1,
                    IsHarmful = false,
                    Description = "Ментол заспокоює шкіру та дає охолоджуючий ефект."
                }
            );
        }

        private static List<Product> _products = new List<Product>{
            new Product
            {
                Id = 1,
                Name = "Зволожуючий крем",
                Description = "Крем для інтенсивного зволоження шкіри.",
                Price = 499.99m,
                CategoryId = 1,
                EffectTypeId = 1,
                SuitableForId = 1
              },
                new Product
                {
                    Id = 2,
                    Name = "Шампунь для сухого волосся",
                    Description = "Відновлюючий шампунь для сухого та пошкодженого волосся.",
                    Price = 299.99m,
                    CategoryId = 2,
                    EffectTypeId = 2,
                    SuitableForId = 2
                } };
        static void seedProduct(ModelBuilder builder)
        {
            builder.Entity<Product>().HasData(_products);
        }
        private static void seedProductIngredients(ModelBuilder builder)
        {
            builder.Entity<ProductIngredient>().HasData(
                new ProductIngredient
                {
                    ProductId = 1,  // Id продукту "Зволожуючий крем"
                    IngredientId = 1  // Id інгредієнта "Парабени"
                },
                new ProductIngredient
                {
                    ProductId = 1,  // Id продукту "Зволожуючий крем"
                    IngredientId = 3  // Id інгредієнта "Ланолін"
                },
                new ProductIngredient
                {
                    ProductId = 2,  // Id продукту "Шампунь для сухого волосся"
                    IngredientId = 2  // Id інгредієнта "Фталати"
                },
                new ProductIngredient
                {
                    ProductId = 2,  // Id продукту "Шампунь для сухого волосся"
                    IngredientId = 4  // Id інгредієнта "Сульфати"
                }
            );
        }
        private static void seedProductAllergens(ModelBuilder builder)
        {
            builder.Entity<ProductAllergen>().HasData(
                new ProductAllergen
                {
                    ProductId = 1, // Продукт "Зволожуючий крем"
                    AllergenId = 1 // Глютен
                },
                new ProductAllergen
                {
                    ProductId = 1, // Продукт "Зволожуючий крем"
                    AllergenId = 2 // Лактоза
                },
                new ProductAllergen
                {
                    ProductId = 2, // Продукт "Шампунь для сухого волосся"
                    AllergenId = 3 // Арахіс
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
        private static void seedOrder(ModelBuilder builder, string[] clientIds, int[] productIds)
        {
            builder.Entity<Order>().HasData(
                new Order
                {
                    Id = 1,
                    UserId = clientIds[0],
                    //ProductId = productIds[0],
                    OrderDate = DateTime.UtcNow,
                    Status = "Completed",
                    TotalAmount = 50.00m,
                    ShippingAddress = "123 Main St"
                }
            );
        }
        
    }
}
