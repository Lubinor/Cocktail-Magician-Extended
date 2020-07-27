using CocktailMagician.Data;
using CocktailMagician.Models;
using CocktailMagician.Services.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CocktailMagician.Tests
{
    public static class Utils
    {
        public static DbContextOptions<CocktailMagicianContext> GetOptions(string databaseName)
        {
            return new DbContextOptionsBuilder<CocktailMagicianContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
        }

        public static void GetInMemoryDataBase(DbContextOptions<CocktailMagicianContext> options)
        {
            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    UserName = "George",
                },
                new User
                {
                    Id = 2,
                    UserName = "Jim"
                }
            };
            var barCocktails = new List<BarsCocktails>
            {
                new BarsCocktails
                {
                    BarId = 1,
                    CocktailId = 1
                },
                new BarsCocktails
                {
                    BarId = 1,
                    CocktailId = 2
                },
                new BarsCocktails
                {
                    BarId = 2,
                    CocktailId = 2
                },
                new BarsCocktails
                {
                    BarId = 3,
                    CocktailId = 1
                },
            };
            var bars = new List<Bar>
            {
                new Bar
                {
                    Id = 1,
                    Name = "Lorka",
                    CityId = 1,
                    Address = "Vitosha bul.",
                    Phone = "0888 888 888",
                    AverageRating = 3.5,
                    BarCocktails = new List<BarsCocktails>
                    {
                        barCocktails[0], barCocktails[1]
                    }
                },
                new Bar
                {
                    Id = 2,
                    Name = "Bilkova",
                    CityId = 1,
                    Address = "Shishman str.",
                    Phone = "0888 888 444",
                    AverageRating = 4,
                    BarCocktails = new List<BarsCocktails>
                    {
                        barCocktails[2]
                    }
                },
                new Bar
                {
                    Id = 3,
                    Name = "The Beach",
                    CityId = 2,
                    Address = "Obikolna str.",
                    Phone = "0888 777 444",
                    AverageRating = 4.5,
                    BarCocktails = new List<BarsCocktails>
                    {
                        barCocktails[3]
                    }
                },
            };
            var cities = new List<City>
            {
                new City
                {
                    Id = 1,
                    Name = "Sofia",
                    Bars = new List<Bar>
                    {
                        bars[0], bars[1]
                    },
                    IsDeleted = false
                },
                new City
                {
                    Id = 2,
                    Name = "Varna",
                    Bars = new List<Bar>
                    {
                        bars[2]
                    },
                    IsDeleted = false
                },
            };

            var barReviews = new List<BarsUsersReviews>
            {
                new BarsUsersReviews
                {
                    BarId = 1,
                    UserId = 1,
                    Comment = "George says Lorka is amazing!",
                    Rating = 5
                },
                new BarsUsersReviews
                {
                    BarId = 1,
                    UserId = 2,
                    Comment = "Jim doesn't like Lorka",
                    Rating = 1
                },
                new BarsUsersReviews
                {
                    BarId = 3,
                    UserId = 2,
                    Comment = "Jim is a fan of The Beach",
                    Rating = 5
                },
                new BarsUsersReviews
                {
                    BarId = 2,
                    UserId = 1,
                    Comment = "George finds Bilkova just average",
                    Rating = 3
                },
                new BarsUsersReviews
                {
                    BarId = 2,
                    UserId = 2,
                    Comment = "Jim Likes Bilkova just above average",
                    Rating = 3.5
                }
            };
            var cocktailReviews = new List<CocktailsUsersReviews>
            {
                new CocktailsUsersReviews
                {
                    CocktailId = 1,
                    UserId = 1,
                    Comment = "George says Mojito is amazing!",
                    Rating = 5
                },
                new CocktailsUsersReviews
                {
                    CocktailId = 1,
                    UserId = 2,
                    Comment = "Jim doesn't like Mojito",
                    Rating = 2
                },
                new CocktailsUsersReviews
                {
                    CocktailId = 2,
                    UserId = 1,
                    Comment = "George likes Gin Fizz !",
                    Rating = 4
                },
                new CocktailsUsersReviews
                {
                    CocktailId = 2,
                    UserId = 2,
                    Comment = "Jim doesn't like Gin Fizz",
                    Rating = 1
                }
            };
            var ingredientCocktails = new List<IngredientsCocktails>
            {
                new IngredientsCocktails
                {
                    CocktailId = 1,
                    IngredientId = 1
                },
                new IngredientsCocktails
                {
                    CocktailId = 1,
                    IngredientId = 2
                },
                new IngredientsCocktails
                {
                    CocktailId = 2,
                    IngredientId = 3
                },
                new IngredientsCocktails
                {
                    CocktailId = 2,
                    IngredientId = 4
                }
            };
            var ingredients = new List<Ingredient>
            {
                new Ingredient
                {
                    Id = 1,
                    Name = "Rum",
                    IsDeleted = false,
                    IngredientsCocktails = new List<IngredientsCocktails>
                    {
                        ingredientCocktails[0]
                    }
                },
                new Ingredient
                {
                    Id = 2,
                    Name = "Soda",
                    IsDeleted = false,
                    IngredientsCocktails = new List<IngredientsCocktails>
                    {
                        ingredientCocktails[1]
                    }
                },
                new Ingredient
                {
                    Id = 3,
                    Name = "Dry Gin",
                    IsDeleted = false,
                    IngredientsCocktails = new List<IngredientsCocktails>
                    {
                        ingredientCocktails[2]
                    }
                },
                new Ingredient
                {
                    Id = 4,
                    Name = "Tonic",
                    IsDeleted = false,
                    IngredientsCocktails = new List<IngredientsCocktails>
                    {
                        ingredientCocktails[3]
                    }
                },
                new Ingredient
                {
                    Id = 5,
                    Name = "Mineral Water",
                    IsDeleted = false,
                    IngredientsCocktails = new List<IngredientsCocktails>()
                },
            };
            var cocktails = new List<Cocktail>
            {
                new Cocktail
                {
                    Id = 1,
                    Name = "Mojito",
                    AverageRating = 4.5,
                    IsDeleted = false,
                    IngredientsCocktails = new List<IngredientsCocktails>
                    {
                        ingredientCocktails[0],ingredientCocktails[1]
                    },
                    CreatorId = 1
                },
                new Cocktail
                {
                    Id = 2,
                    Name = "Gin Fizz",
                    AverageRating = 4.9,
                    IsDeleted = false,
                    IngredientsCocktails = new List<IngredientsCocktails>
                    {
                        ingredientCocktails[2],ingredientCocktails[3]
                    },
                    CreatorId = 2

                },
                new Cocktail
                {
                    Id = 3,
                    Name = "Bozdugan",
                    AverageRating = 3.8,
                    IsDeleted = false,
                    IngredientsCocktails = new List<IngredientsCocktails>
                    {
                        ingredientCocktails[0],ingredientCocktails[2]
                    },
                    CreatorId = 2
                }
            };
            using (var arrangeContext = new CocktailMagicianContext(options))
            {
                arrangeContext.Cocktails.AddRange(cocktails);
                arrangeContext.Ingredients.AddRange(ingredients);
                arrangeContext.IngredientsCocktails.AddRange(ingredientCocktails);
                arrangeContext.BarsCocktails.AddRange(barCocktails);
                arrangeContext.Bars.AddRange(bars);
                arrangeContext.BarsUsersReviews.AddRange(barReviews);
                arrangeContext.Cities.AddRange(cities);
                arrangeContext.CocktailsUsersReviews.AddRange(cocktailReviews);
                arrangeContext.Users.AddRange(users);
                arrangeContext.SaveChanges();
            }
        }
    }
}
