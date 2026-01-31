using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using BepTroLy.Infrastructure.Persistence;
using BepTroLy.Domain.Entities;

namespace BepTroLy.Infrastructure.Seeders
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Check if already seeded
                if (dbContext.IngredientCategories.Any() || dbContext.Units.Any())
                    return;

                // Seed Units
                var units = new[]
                {
                    new Unit { UnitName = "kg" },
                    new Unit { UnitName = "g" },
                    new Unit { UnitName = "ml" },
                    new Unit { UnitName = "l" },
                    new Unit { UnitName = "tbsp" },
                    new Unit { UnitName = "tsp" },
                    new Unit { UnitName = "cup" },
                    new Unit { UnitName = "piece" }
                };
                dbContext.Units.AddRange(units);
                await dbContext.SaveChangesAsync();

                // Seed Ingredient Categories
                var ingredientCategories = new[]
                {
                    new IngredientCategory { CategoryName = "Vegetables" },
                    new IngredientCategory { CategoryName = "Fruits" },
                    new IngredientCategory { CategoryName = "Meat & Poultry" },
                    new IngredientCategory { CategoryName = "Seafood" },
                    new IngredientCategory { CategoryName = "Dairy" },
                    new IngredientCategory { CategoryName = "Grains" },
                    new IngredientCategory { CategoryName = "Spices & Herbs" },
                    new IngredientCategory { CategoryName = "Oils & Condiments" }
                };
                dbContext.IngredientCategories.AddRange(ingredientCategories);
                await dbContext.SaveChangesAsync();

                // Seed Ingredients
                var vegetables = ingredientCategories.FirstOrDefault(ic => ic.CategoryName == "Vegetables");
                var meat = ingredientCategories.FirstOrDefault(ic => ic.CategoryName == "Meat & Poultry");
                var seafood = ingredientCategories.FirstOrDefault(ic => ic.CategoryName == "Seafood");
                var dairy = ingredientCategories.FirstOrDefault(ic => ic.CategoryName == "Dairy");
                var grains = ingredientCategories.FirstOrDefault(ic => ic.CategoryName == "Grains");
                var spices = ingredientCategories.FirstOrDefault(ic => ic.CategoryName == "Spices & Herbs");

                var ingredients = new List<Ingredient>
                {
                    // Vegetables
                    new Ingredient { IngredientName = "Tomato", CategoryId = vegetables.CategoryId },
                    new Ingredient { IngredientName = "Onion", CategoryId = vegetables.CategoryId },
                    new Ingredient { IngredientName = "Garlic", CategoryId = vegetables.CategoryId },
                    new Ingredient { IngredientName = "Carrot", CategoryId = vegetables.CategoryId },
                    new Ingredient { IngredientName = "Bell Pepper", CategoryId = vegetables.CategoryId },
                    new Ingredient { IngredientName = "Broccoli", CategoryId = vegetables.CategoryId },
                    new Ingredient { IngredientName = "Spinach", CategoryId = vegetables.CategoryId },
                    new Ingredient { IngredientName = "Cucumber", CategoryId = vegetables.CategoryId },

                    // Meat
                    new Ingredient { IngredientName = "Chicken Breast", CategoryId = meat.CategoryId },
                    new Ingredient { IngredientName = "Ground Beef", CategoryId = meat.CategoryId },
                    new Ingredient { IngredientName = "Pork Chop", CategoryId = meat.CategoryId },
                    new Ingredient { IngredientName = "Beef Steak", CategoryId = meat.CategoryId },

                    // Seafood
                    new Ingredient { IngredientName = "Salmon", CategoryId = seafood.CategoryId },
                    new Ingredient { IngredientName = "Shrimp", CategoryId = seafood.CategoryId },
                    new Ingredient { IngredientName = "Tuna", CategoryId = seafood.CategoryId },

                    // Dairy
                    new Ingredient { IngredientName = "Milk", CategoryId = dairy.CategoryId },
                    new Ingredient { IngredientName = "Cheese", CategoryId = dairy.CategoryId },
                    new Ingredient { IngredientName = "Butter", CategoryId = dairy.CategoryId },
                    new Ingredient { IngredientName = "Egg", CategoryId = dairy.CategoryId },

                    // Grains
                    new Ingredient { IngredientName = "Rice", CategoryId = grains.CategoryId },
                    new Ingredient { IngredientName = "Pasta", CategoryId = grains.CategoryId },
                    new Ingredient { IngredientName = "Bread", CategoryId = grains.CategoryId },

                    // Spices
                    new Ingredient { IngredientName = "Salt", CategoryId = spices.CategoryId },
                    new Ingredient { IngredientName = "Black Pepper", CategoryId = spices.CategoryId },
                    new Ingredient { IngredientName = "Olive Oil", CategoryId = spices.CategoryId }
                };
                dbContext.Ingredients.AddRange(ingredients);
                await dbContext.SaveChangesAsync();

                // Seed Recipe Categories
                var recipeCategories = new[]
                {
                    new RecipeCategory { CategoryName = "Vietnamese Cuisine" },
                    new RecipeCategory { CategoryName = "Asian Cuisine" },
                    new RecipeCategory { CategoryName = "Western Cuisine" },
                    new RecipeCategory { CategoryName = "Salads" },
                    new RecipeCategory { CategoryName = "Soups" },
                    new RecipeCategory { CategoryName = "Desserts" },
                    new RecipeCategory { CategoryName = "Breakfast" }
                };
                dbContext.RecipeCategories.AddRange(recipeCategories);
                await dbContext.SaveChangesAsync();

                // Seed Sample Recipes
                var vietnameseCategory = recipeCategories.FirstOrDefault(rc => rc.CategoryName == "Vietnamese Cuisine");
                var kgUnit = units.FirstOrDefault(u => u.UnitName == "kg");
                var gUnit = units.FirstOrDefault(u => u.UnitName == "g");
                var tbspUnit = units.FirstOrDefault(u => u.UnitName == "tbsp");
                var cupUnit = units.FirstOrDefault(u => u.UnitName == "cup");

                var recipes = new List<Recipe>
                {
                    new Recipe
                    {
                        RecipeName = "Pho (Vietnamese Beef Noodle Soup)",
                        CategoryId = vietnameseCategory.CategoryId,
                        Description = "Traditional Vietnamese beef noodle soup with aromatic broth",
                        PrepTimeMinutes = 20,
                        CookTimeMinutes = 120,
                        Servings = 4
                    },
                    new Recipe
                    {
                        RecipeName = "Grilled Chicken Breast",
                        CategoryId = recipeCategories.FirstOrDefault(rc => rc.CategoryName == "Western Cuisine").CategoryId,
                        Description = "Simple grilled chicken with herbs and spices",
                        PrepTimeMinutes = 15,
                        CookTimeMinutes = 25,
                        Servings = 2
                    },
                    new Recipe
                    {
                        RecipeName = "Vegetable Stir Fry",
                        CategoryId = recipeCategories.FirstOrDefault(rc => rc.CategoryName == "Asian Cuisine").CategoryId,
                        Description = "Quick and healthy vegetable stir fry with soy sauce",
                        PrepTimeMinutes = 15,
                        CookTimeMinutes = 10,
                        Servings = 3
                    },
                    new Recipe
                    {
                        RecipeName = "Salmon with Vegetables",
                        CategoryId = recipeCategories.FirstOrDefault(rc => rc.CategoryName == "Western Cuisine").CategoryId,
                        Description = "Baked salmon fillet with roasted vegetables",
                        PrepTimeMinutes = 10,
                        CookTimeMinutes = 20,
                        Servings = 2
                    },
                    new Recipe
                    {
                        RecipeName = "Caesar Salad",
                        CategoryId = recipeCategories.FirstOrDefault(rc => rc.CategoryName == "Salads").CategoryId,
                        Description = "Fresh salad with parmesan and croutons",
                        PrepTimeMinutes = 15,
                        CookTimeMinutes = 0,
                        Servings = 2
                    }
                };

                dbContext.Recipes.AddRange(recipes);
                await dbContext.SaveChangesAsync();

                // Add recipe ingredients to first recipe (Pho)
                var pho = recipes.FirstOrDefault(r => r.RecipeName == "Pho (Vietnamese Beef Noodle Soup)");
                var beefIngredient = ingredients.FirstOrDefault(i => i.IngredientName == "Beef Steak");
                var onionIngredient = ingredients.FirstOrDefault(i => i.IngredientName == "Onion");
                var garlicIngredient = ingredients.FirstOrDefault(i => i.IngredientName == "Garlic");

                if (pho != null && beefIngredient != null && onionIngredient != null && garlicIngredient != null)
                {
                    var phoIngredients = new List<RecipeIngredient>
                    {
                        new RecipeIngredient { RecipeId = pho.RecipeId, IngredientId = beefIngredient.IngredientId, Quantity = 1m, UnitId = kgUnit.UnitId },
                        new RecipeIngredient { RecipeId = pho.RecipeId, IngredientId = onionIngredient.IngredientId, Quantity = 2m, UnitId = (dbContext.Units.FirstOrDefault(u => u.UnitName == "piece").UnitId) },
                        new RecipeIngredient { RecipeId = pho.RecipeId, IngredientId = garlicIngredient.IngredientId, Quantity = 4m, UnitId = (dbContext.Units.FirstOrDefault(u => u.UnitName == "clove") != null ? dbContext.Units.FirstOrDefault(u => u.UnitName == "clove").UnitId : gUnit.UnitId) }
                    };

                    dbContext.RecipeIngredients.AddRange(phoIngredients);

                    // Add recipe steps
                    var phoSteps = new List<RecipeStep>
                    {
                        new RecipeStep { RecipeId = pho.RecipeId, StepNumber = 1, Description = "Boil water and add beef", DurationMinutes = 30 },
                        new RecipeStep { RecipeId = pho.RecipeId, StepNumber = 2, Description = "Add onion and garlic", DurationMinutes = 10 },
                        new RecipeStep { RecipeId = pho.RecipeId, StepNumber = 3, Description = "Simmer for 1.5 hours", DurationMinutes = 90 },
                        new RecipeStep { RecipeId = pho.RecipeId, StepNumber = 4, Description = "Cook noodles separately and serve", DurationMinutes = 10 }
                    };

                    dbContext.RecipeSteps.AddRange(phoSteps);
                }

                // Add ingredients to Grilled Chicken
                var grilledChicken = recipes.FirstOrDefault(r => r.RecipeName == "Grilled Chicken Breast");
                var chickenIngredient = ingredients.FirstOrDefault(i => i.IngredientName == "Chicken Breast");

                if (grilledChicken != null && chickenIngredient != null)
                {
                    var chickenIngredients = new List<RecipeIngredient>
                    {
                        new RecipeIngredient { RecipeId = grilledChicken.RecipeId, IngredientId = chickenIngredient.IngredientId, Quantity = 500m, UnitId = gUnit.UnitId }
                    };

                    dbContext.RecipeIngredients.AddRange(chickenIngredients);

                    var chickenSteps = new List<RecipeStep>
                    {
                        new RecipeStep { RecipeId = grilledChicken.RecipeId, StepNumber = 1, Description = "Season chicken with salt and pepper", DurationMinutes = 5 },
                        new RecipeStep { RecipeId = grilledChicken.RecipeId, StepNumber = 2, Description = "Preheat grill to 200°C", DurationMinutes = 5 },
                        new RecipeStep { RecipeId = grilledChicken.RecipeId, StepNumber = 3, Description = "Grill chicken for 12-15 minutes per side", DurationMinutes = 25 }
                    };

                    dbContext.RecipeSteps.AddRange(chickenSteps);
                }

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
