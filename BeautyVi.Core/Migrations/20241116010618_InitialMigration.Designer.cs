﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BeautyVi.Core.Migrations
{
    [DbContext(typeof(BeautyViContext))]
    [Migration("20241116010618_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BeautyVi.Core.Entities.Allergen", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Allergens");
                });

            modelBuilder.Entity("BeautyVi.Core.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("NameCategory")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.HasIndex("NameCategory")
                        .IsUnique();

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            NameCategory = "Догляд за шкірою"
                        },
                        new
                        {
                            Id = 2,
                            NameCategory = "Догляд за волоссям"
                        },
                        new
                        {
                            Id = 3,
                            NameCategory = "Макіяж"
                        });
                });

            modelBuilder.Entity("BeautyVi.Core.Entities.Ingredient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsHarmful")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("BeautyVi.Core.Entities.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ShippingAddress")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("BeautyVi.Core.Entities.OrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("OrderId")
                        .HasColumnType("integer");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("BeautyVi.Core.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<string>("CoverPath")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryId = 1,
                            CoverPath = "\\img\\product\\no_cover.jpg",
                            Description = "Крем для інтенсивного зволоження шкіри.",
                            Name = "Зволожуючий крем",
                            Price = 499.99m
                        },
                        new
                        {
                            Id = 2,
                            CategoryId = 2,
                            CoverPath = "\\img\\product\\no_cover.jpg",
                            Description = "Відновлюючий шампунь для сухого та пошкодженого волосся.",
                            Name = "Шампунь для сухого волосся",
                            Price = 299.99m
                        });
                });

            modelBuilder.Entity("BeautyVi.Core.Entities.ProductAllergen", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<int>("AllergenId")
                        .HasColumnType("integer");

                    b.HasKey("ProductId", "AllergenId");

                    b.HasIndex("AllergenId");

                    b.ToTable("ProductAllergens");
                });

            modelBuilder.Entity("BeautyVi.Core.Entities.ProductIngredient", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<int>("IngredientId")
                        .HasColumnType("integer");

                    b.HasKey("ProductId", "IngredientId");

                    b.HasIndex("IngredientId");

                    b.ToTable("ProductIngredients");
                });

            modelBuilder.Entity("BeautyVi.Core.Entities.ProductRecommendation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("ProductRecommendations");
                });

            modelBuilder.Entity("BeautyVi.Core.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(256)
                        .IsUnicode(false)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FullName")
                        .HasColumnType("text");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "93f189f1-1b81-40e4-a605-ffc86b596e00",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "4050da3b-f631-4173-bf96-c522c7e0baa5",
                            Email = "client@beautyvi.com",
                            EmailConfirmed = true,
                            FullName = "Olena Demchuk",
                            LockoutEnabled = false,
                            NormalizedEmail = "CLIENT@BEAUTYVI.COM",
                            NormalizedUserName = "CLIENT@BEAUTYVI.COM",
                            PasswordHash = "AQAAAAIAAYagAAAAEMWXUJFvWLW9yk29BuSGADCT4QQ5A0UTnjO7wlvgVWdzOLXoElj3Jp2RbAHUxjBX/w==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "4035ec2a-4c60-4b6f-912a-669373dce439",
                            TwoFactorEnabled = false,
                            UserName = "client@beautyvi.com"
                        },
                        new
                        {
                            Id = "3fe2ecee-0d86-4690-8801-b651666ed062",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "daa78860-b56d-445a-a66a-ea4b7f70b0d6",
                            Email = "admin@beautyvi.com",
                            EmailConfirmed = true,
                            FullName = "Alla Svoboda",
                            LockoutEnabled = false,
                            NormalizedEmail = "ADMIN@BEAUTYVI.COM",
                            NormalizedUserName = "ADMIN@BEAUTYVI.COM",
                            PasswordHash = "AQAAAAIAAYagAAAAEOpyllBHknJKO76tPWxGHMer6HJxRyasOODxDX/fpqjwJAdye0wutCYwPYSSa7xtQA==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "4ea02a04-fb33-428e-a044-d98eb3c9e504",
                            TwoFactorEnabled = false,
                            UserName = "admin@beautyvi.com"
                        });
                });

            modelBuilder.Entity("BeautyVi.Core.Entities.UserPreferences", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("AvoidAllergens")
                        .HasColumnType("boolean");

                    b.Property<string>("AvoidedAllergens")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("HairType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SkinType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("UserPreferences");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "d64892fa-dcfa-47df-90b5-40f60503c162",
                            Name = "Client",
                            NormalizedName = "CLIENT"
                        },
                        new
                        {
                            Id = "57d6747e-2fa7-4459-b387-af43c4bbb9fe",
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);

                    b.HasData(
                        new
                        {
                            UserId = "3fe2ecee-0d86-4690-8801-b651666ed062",
                            RoleId = "57d6747e-2fa7-4459-b387-af43c4bbb9fe"
                        },
                        new
                        {
                            UserId = "93f189f1-1b81-40e4-a605-ffc86b596e00",
                            RoleId = "d64892fa-dcfa-47df-90b5-40f60503c162"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("BeautyVi.Core.Entities.Order", b =>
                {
                    b.HasOne("BeautyVi.Core.Entities.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("User");
                });

            modelBuilder.Entity("BeautyVi.Core.Entities.OrderItem", b =>
                {
                    b.HasOne("BeautyVi.Core.Entities.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BeautyVi.Core.Entities.Product", "Product")
                        .WithMany("OrderItems")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("BeautyVi.Core.Entities.Product", b =>
                {
                    b.HasOne("BeautyVi.Core.Entities.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Category");
                });

            modelBuilder.Entity("BeautyVi.Core.Entities.ProductAllergen", b =>
                {
                    b.HasOne("BeautyVi.Core.Entities.Allergen", "Allergen")
                        .WithMany("ProductAllergens")
                        .HasForeignKey("AllergenId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BeautyVi.Core.Entities.Product", "Product")
                        .WithMany("ProductAllergens")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Allergen");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("BeautyVi.Core.Entities.ProductIngredient", b =>
                {
                    b.HasOne("BeautyVi.Core.Entities.Ingredient", "Ingredient")
                        .WithMany("ProductIngredients")
                        .HasForeignKey("IngredientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BeautyVi.Core.Entities.Product", "Product")
                        .WithMany("ProductIngredients")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ingredient");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("BeautyVi.Core.Entities.ProductRecommendation", b =>
                {
                    b.HasOne("BeautyVi.Core.Entities.Product", "Product")
                        .WithMany("ProductRecommendations")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BeautyVi.Core.Entities.User", "User")
                        .WithMany("ProductRecommendations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BeautyVi.Core.Entities.UserPreferences", b =>
                {
                    b.HasOne("BeautyVi.Core.Entities.User", "User")
                        .WithOne("UserPreferences")
                        .HasForeignKey("BeautyVi.Core.Entities.UserPreferences", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("User");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("BeautyVi.Core.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("BeautyVi.Core.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BeautyVi.Core.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("BeautyVi.Core.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BeautyVi.Core.Entities.Allergen", b =>
                {
                    b.Navigation("ProductAllergens");
                });

            modelBuilder.Entity("BeautyVi.Core.Entities.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("BeautyVi.Core.Entities.Ingredient", b =>
                {
                    b.Navigation("ProductIngredients");
                });

            modelBuilder.Entity("BeautyVi.Core.Entities.Order", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("BeautyVi.Core.Entities.Product", b =>
                {
                    b.Navigation("OrderItems");

                    b.Navigation("ProductAllergens");

                    b.Navigation("ProductIngredients");

                    b.Navigation("ProductRecommendations");
                });

            modelBuilder.Entity("BeautyVi.Core.Entities.User", b =>
                {
                    b.Navigation("Orders");

                    b.Navigation("ProductRecommendations");

                    b.Navigation("UserPreferences");
                });
#pragma warning restore 612, 618
        }
    }
}