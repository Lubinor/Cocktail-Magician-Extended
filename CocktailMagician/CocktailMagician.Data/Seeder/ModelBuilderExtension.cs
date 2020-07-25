using CocktailMagician.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace CocktailMagician.Data.Seeder
{
    public static class ModelBuilderExtension
    {
        public static void Seeder(this ModelBuilder builder)
        {
            builder.Entity<Role>().HasData(
                new Role
                {
                    Id = 1,
                    Name = "Bar Crawler",
                    NormalizedName = "BAR CRAWLER"
                },
                new Role
                {
                    Id = 2,
                    Name = "Cocktail Magician",
                    NormalizedName = "COCKTAIL MAGICIAN"
                }
                );

            var hasher = new PasswordHasher<User>();

            User adminUser = new User
            {
                Id = 1,
                UserName = "admin@admin.com",
                NormalizedUserName = "ADMIN@ADMIN.COM",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                CreatedOn = DateTime.UtcNow,
                LockoutEnabled = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            adminUser.PasswordHash = hasher.HashPassword(adminUser, "admin");
            builder.Entity<User>().HasData(adminUser);

            User userUser = new User
            {
                Id = 2,
                UserName = "user@user.com",
                NormalizedUserName = "USER@USER.COM",
                Email = "user@user.com",
                NormalizedEmail = "USER@USER.COM",
                CreatedOn = DateTime.UtcNow,
                LockoutEnabled = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            userUser.PasswordHash = hasher.HashPassword(userUser, "user");
            builder.Entity<User>().HasData(userUser);

            builder.Entity<IdentityUserRole<int>>().HasData(
                new IdentityUserRole<int>
                {
                    RoleId = 2,
                    UserId = adminUser.Id
                });

            builder.Entity<IdentityUserRole<int>>().HasData(
                new IdentityUserRole<int>
                {
                    RoleId = 1,
                    UserId = userUser.Id
                });


            builder.Entity<City>().HasData(
                new City
                {
                    Id = 1,
                    Name = "Sofia",
                    CreatedOn = DateTime.UtcNow
                },
                new City
                {
                    Id = 2,
                    Name = "Plovdiv",
                    CreatedOn = DateTime.UtcNow
                },
                new City
                {
                    Id = 3,
                    Name = "Varna",
                    CreatedOn = DateTime.UtcNow
                },
                new City
                {
                    Id = 4,
                    Name = "Burgas",
                    CreatedOn = DateTime.UtcNow
                });
            builder.Entity<Ingredient>().HasData(
                new Ingredient
                {
                    Id = 1,
                    Name = "Vodka",
                    CreatedOn = DateTime.UtcNow
                },
                new Ingredient
                {
                    Id = 2,
                    Name = "Gin",
                    CreatedOn = DateTime.UtcNow
                },
                new Ingredient
                {
                    Id = 3,
                    Name = "Rum",
                    CreatedOn = DateTime.UtcNow
                },                
                new Ingredient
                {
                    Id = 4,
                    Name = "Soda",
                    CreatedOn = DateTime.UtcNow
                },                  
                new Ingredient
                {
                    Id = 5,
                    Name = "Coke",
                    CreatedOn = DateTime.UtcNow
                },                
                new Ingredient
                {
                    Id = 6,
                    Name = "Lemon juice",
                    CreatedOn = DateTime.UtcNow
                },                
                new Ingredient
                {
                    Id = 7,
                    Name = "Sugar",
                    CreatedOn = DateTime.UtcNow
                },                
                new Ingredient
                {
                    Id = 8,
                    Name = "Milk",
                    CreatedOn = DateTime.UtcNow
                },                
                new Ingredient
                {
                    Id = 9,
                    Name = "Coffee liqueur",
                    CreatedOn = DateTime.UtcNow
                },                
                new Ingredient
                {
                    Id = 10,
                    Name = "Orange juice",
                    CreatedOn = DateTime.UtcNow
                },                
                new Ingredient
                {
                    Id = 11,
                    Name = "Tomato juice",
                    CreatedOn = DateTime.UtcNow
                },                
                new Ingredient
                {
                    Id = 12,
                    Name = "Tabasco",
                    CreatedOn = DateTime.UtcNow
                },                
                new Ingredient
                {
                    Id = 13,
                    Name = "Lime",
                    CreatedOn = DateTime.UtcNow
                }
                );

            builder.Entity<Cocktail>().HasData(
                new Cocktail
                {
                    Id = 1,
                    Name = "Mojito",
                    CreatedOn = DateTime.UtcNow,
                    CreatorId = 2
                },
                new Cocktail
                {
                    Id = 2,
                    Name = "Cuba Libre",
                    CreatedOn = DateTime.UtcNow,
                    CreatorId = 2
                },
                new Cocktail
                {
                    Id = 3,
                    Name = "Sex on the Beach",
                    CreatedOn = DateTime.UtcNow,
                    CreatorId = 2
                },
                new Cocktail
                {
                    Id = 4,
                    Name = "Mai Tai",
                    CreatedOn = DateTime.UtcNow,
                    CreatorId = 1
                },
                new Cocktail
                {
                    Id = 5,
                    Name = "Gin Fizz",
                    CreatedOn = DateTime.UtcNow,
                    CreatorId = 1
                },
                new Cocktail
                {
                    Id = 6,
                    Name = "Bloody Mary",
                    CreatedOn = DateTime.UtcNow,
                    CreatorId = 2
                }
                );

            builder.Entity<Bar>().HasData(
                new Bar
                {
                    Id = 1,
                    Name = "Memento",
                    Address = "104 Vitosha blvd.",
                    Phone = "0889 555 682",
                    CreatedOn = DateTime.UtcNow,
                    CityId = 1
                },
                new Bar
                {
                    Id = 2,
                    Name = "Bilkova",
                    Address = "22 Tsar Ivan Shishman str.",
                    Phone = "0898 639 068",
                    CreatedOn = DateTime.UtcNow,
                    CityId = 1
                },
                new Bar
                {
                    Id = 3,
                    Name = "Petnoto",
                    Address = "36 Yoakim Gruev str.",
                    Phone = "0878 509 703",
                    CreatedOn = DateTime.UtcNow,
                    CityId = 2
                },
                new Bar
                {
                    Id = 4,
                    Name = "Cubo",
                    Address = "Central Beach",
                    Phone = "0898 425 232",
                    CreatedOn = DateTime.UtcNow,
                    CityId = 3
                },
                new Bar
                {
                    Id = 5,
                    Name = "Barcode",
                    Address = "1 Tsar Peter str.",
                    Phone = "0895 509 659",
                    CreatedOn = DateTime.UtcNow,
                    CityId = 4
                },
                new Bar
                {
                    Id = 6,
                    Name = "Fabric Club",
                    Address = "53 Stefan Stambolov blvd.",
                    Phone = "0887 909 019",
                    CreatedOn = DateTime.UtcNow,
                    CityId = 4
                }
                );

            builder.Entity<IngredientsCocktails>().HasData(
                new IngredientsCocktails
                {
                    CocktailId = 1,
                    IngredientId = 3,
                },
                new IngredientsCocktails
                {
                    CocktailId = 1,
                    IngredientId = 4,
                },
                new IngredientsCocktails
                {
                    CocktailId = 1,
                    IngredientId = 7,
                },
                new IngredientsCocktails
                {
                    CocktailId = 1,
                    IngredientId = 13,
                },
                new IngredientsCocktails
                {
                    CocktailId = 6,
                    IngredientId = 1,
                },
                new IngredientsCocktails
                {
                    CocktailId = 6,
                    IngredientId = 11,
                },
                new IngredientsCocktails
                {
                    CocktailId = 6,
                    IngredientId = 12,
                },
                new IngredientsCocktails
                {
                    CocktailId = 2,
                    IngredientId = 3,
                },
                new IngredientsCocktails
                {
                    CocktailId = 2,
                    IngredientId = 5,
                },
                new IngredientsCocktails
                {
                    CocktailId = 2,
                    IngredientId = 13,
                },
                new IngredientsCocktails
                {
                    CocktailId = 3,
                    IngredientId = 1,
                },
                new IngredientsCocktails
                {
                    CocktailId = 3,
                    IngredientId = 10,
                },
                new IngredientsCocktails
                {
                    CocktailId = 4,
                    IngredientId = 3,
                },
                new IngredientsCocktails
                {
                    CocktailId = 4,
                    IngredientId = 6,
                },
                new IngredientsCocktails
                {
                    CocktailId = 5,
                    IngredientId = 6,
                },
                new IngredientsCocktails
                {
                    CocktailId = 5,
                    IngredientId = 2,
                }
                );
            
            builder.Entity<BarsCocktails>().HasData(
                new BarsCocktails
                {
                    BarId = 1,
                    CocktailId = 1,
                },
                new BarsCocktails
                {
                    BarId = 1,
                    CocktailId = 2,
                },
                new BarsCocktails
                {
                    BarId = 1,
                    CocktailId = 6,
                },
                new BarsCocktails
                {
                    BarId = 2,
                    CocktailId = 1,
                },
                new BarsCocktails
                {
                    BarId = 2,
                    CocktailId = 3,
                },
                new BarsCocktails
                {
                    BarId = 2,
                    CocktailId = 6,
                },
                new BarsCocktails
                {
                    BarId = 3,
                    CocktailId = 1,
                },
                new BarsCocktails
                {
                    BarId = 3,
                    CocktailId = 4,
                },
                new BarsCocktails
                {
                    BarId = 3,
                    CocktailId = 6,
                },                  
                new BarsCocktails
                {
                    BarId = 4,
                    CocktailId = 1,
                },
                 new BarsCocktails
                 {
                     BarId = 4,
                     CocktailId = 5,
                 },
                new BarsCocktails
                {
                    BarId = 4,
                    CocktailId = 6,
                },                  
                new BarsCocktails
                {
                    BarId = 5,
                    CocktailId = 1,
                },                  
                new BarsCocktails
                {
                    BarId = 5,
                    CocktailId = 6,
                },                  
                new BarsCocktails
                {
                    BarId = 6,
                    CocktailId = 1,
                },
                new BarsCocktails
                {
                    BarId = 6,
                    CocktailId = 3,
                },
                new BarsCocktails
                {
                    BarId = 6,
                    CocktailId = 6,
                });
        }
    }
}
