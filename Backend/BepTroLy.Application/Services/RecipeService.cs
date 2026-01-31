using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BepTroLy.Application.DTOs;
using BepTroLy.Application.Interfaces;
using BepTroLy.Domain.Entities;
using BepTroLy.Infrastructure.Persistence;

namespace BepTroLy.Application.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly AppDbContext _dbContext;

        public RecipeService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ============= Recipe Category Management =============
        public async Task<IEnumerable<RecipeCategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _dbContext.RecipeCategories.ToListAsync();
            return categories.Select(c => new RecipeCategoryDto
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName
            });
        }

        public async Task<RecipeCategoryDto> GetCategoryByIdAsync(int categoryId)
        {
            var category = await _dbContext.RecipeCategories.FindAsync(categoryId);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {categoryId} not found");

            return new RecipeCategoryDto
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName
            };
        }

        public async Task<RecipeCategoryDto> CreateCategoryAsync(CreateRecipeCategoryRequest request)
        {
            var category = new RecipeCategory
            {
                CategoryName = request.CategoryName
            };

            _dbContext.RecipeCategories.Add(category);
            await _dbContext.SaveChangesAsync();

            return new RecipeCategoryDto
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName
            };
        }

        public async Task UpdateCategoryAsync(int categoryId, CreateRecipeCategoryRequest request)
        {
            var category = await _dbContext.RecipeCategories.FindAsync(categoryId);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {categoryId} not found");

            category.CategoryName = request.CategoryName;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            var category = await _dbContext.RecipeCategories.FindAsync(categoryId);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {categoryId} not found");

            _dbContext.RecipeCategories.Remove(category);
            await _dbContext.SaveChangesAsync();
        }

        // ============= Recipe Management =============
        public async Task<IEnumerable<RecipeDto>> GetAllRecipesAsync()
        {
            var recipes = await _dbContext.Recipes
                .Include(r => r.Category)
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Unit)
                .Include(r => r.Steps)
                .ToListAsync();

            return recipes.Select(r => MapToDto(r));
        }

        public async Task<RecipeDto> GetRecipeByIdAsync(int recipeId)
        {
            var recipe = await _dbContext.Recipes
                .Include(r => r.Category)
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Unit)
                .Include(r => r.Steps)
                .FirstOrDefaultAsync(r => r.RecipeId == recipeId);

            if (recipe == null)
                throw new KeyNotFoundException($"Recipe with ID {recipeId} not found");

            return MapToDto(recipe);
        }

        public async Task<RecipeDto> CreateRecipeAsync(CreateRecipeRequest request)
        {
            var recipe = new Recipe
            {
                RecipeName = request.RecipeName,
                Description = request.Description,
                CategoryId = request.CategoryId,
                PrepTimeMinutes = request.PrepTimeMinutes,
                CookTimeMinutes = request.CookTimeMinutes,
                Servings = request.Servings
            };

            _dbContext.Recipes.Add(recipe);
            await _dbContext.SaveChangesAsync();

            // Add ingredients if provided
            if (request.Ingredients != null && request.Ingredients.Any())
            {
                foreach (var ing in request.Ingredients)
                {
                    var recipeIngredient = new RecipeIngredient
                    {
                        RecipeId = recipe.RecipeId,
                        IngredientId = ing.IngredientId,
                        Quantity = ing.Quantity,
                        UnitId = ing.UnitId
                    };
                    _dbContext.RecipeIngredients.Add(recipeIngredient);
                }
            }

            // Add steps if provided
            if (request.Steps != null && request.Steps.Any())
            {
                foreach (var step in request.Steps)
                {
                    var recipeStep = new RecipeStep
                    {
                        RecipeId = recipe.RecipeId,
                        StepNumber = step.StepNumber,
                        Description = step.Description,
                        DurationMinutes = step.DurationMinutes
                    };
                    _dbContext.RecipeSteps.Add(recipeStep);
                }
            }

            await _dbContext.SaveChangesAsync();
            return await GetRecipeByIdAsync(recipe.RecipeId);
        }

        public async Task UpdateRecipeAsync(int recipeId, UpdateRecipeRequest request)
        {
            var recipe = await _dbContext.Recipes.FindAsync(recipeId);
            if (recipe == null)
                throw new KeyNotFoundException($"Recipe with ID {recipeId} not found");

            recipe.RecipeName = request.RecipeName ?? recipe.RecipeName;
            recipe.Description = request.Description ?? recipe.Description;
            recipe.CategoryId = request.CategoryId ?? recipe.CategoryId;
            recipe.PrepTimeMinutes = request.PrepTimeMinutes;
            recipe.CookTimeMinutes = request.CookTimeMinutes;
            recipe.Servings = request.Servings;

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRecipeAsync(int recipeId)
        {
            var recipe = await _dbContext.Recipes.FindAsync(recipeId);
            if (recipe == null)
                throw new KeyNotFoundException($"Recipe with ID {recipeId} not found");

            _dbContext.Recipes.Remove(recipe);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<RecipeDto>> SearchRecipesAsync(SearchRecipeRequest request)
        {
            var query = _dbContext.Recipes
                .Include(r => r.Category)
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Unit)
                .Include(r => r.Steps)
                .AsQueryable();

            // Filter by search term (recipe name or description)
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                query = query.Where(r => r.RecipeName.Contains(request.SearchTerm) || 
                                        r.Description.Contains(request.SearchTerm));
            }

            // Filter by category
            if (request.CategoryId.HasValue)
            {
                query = query.Where(r => r.CategoryId == request.CategoryId);
            }

            // Filter by prep time
            if (request.MaxPrepTime.HasValue)
            {
                query = query.Where(r => r.PrepTimeMinutes <= request.MaxPrepTime);
            }

            // Filter by cook time
            if (request.MaxCookTime.HasValue)
            {
                query = query.Where(r => r.CookTimeMinutes <= request.MaxCookTime);
            }

            // Filter by required ingredients
            if (request.RequiredIngredientIds != null && request.RequiredIngredientIds.Any())
            {
                query = query.Where(r => request.RequiredIngredientIds.All(id => 
                    r.RecipeIngredients.Any(ri => ri.IngredientId == id)));
            }

            var recipes = await query.ToListAsync();
            return recipes.Select(r => MapToDto(r));
        }

        // ============= Helper Methods =============
        private RecipeDto MapToDto(Recipe recipe)
        {
            return new RecipeDto
            {
                RecipeId = recipe.RecipeId,
                RecipeName = recipe.RecipeName,
                Description = recipe.Description,
                CategoryId = recipe.CategoryId,
                CategoryName = recipe.Category?.CategoryName,
                PrepTimeMinutes = recipe.PrepTimeMinutes,
                CookTimeMinutes = recipe.CookTimeMinutes,
                Servings = recipe.Servings,
                    ImageUrl = recipe.ImageUrl,
                Ingredients = recipe.RecipeIngredients?.Select(ri => new RecipeIngredientDto
                {
                    RecipeIngredientId = ri.RecipeIngredientId,
                    IngredientId = ri.IngredientId,
                    IngredientName = ri.Ingredient?.IngredientName,
                    Quantity = ri.Quantity,
                    UnitId = ri.UnitId,
                    UnitName = ri.Unit?.UnitName
                }).ToList(),
                Steps = recipe.Steps?.OrderBy(s => s.StepNumber).Select(s => new RecipeStepDto
                {
                    StepId = s.StepId,
                    StepNumber = s.StepNumber,
                    Description = s.Description,
                    DurationMinutes = s.DurationMinutes
                }).ToList()
            };
        }

        public async Task<RecipeDto> UpdateRecipeImageAsync(int recipeId, string imageUrl)
        {
            var recipe = await _dbContext.Recipes
                .Include(r => r.Category)
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Unit)
                .Include(r => r.Steps)
                .FirstOrDefaultAsync(r => r.RecipeId == recipeId);

            if (recipe == null)
                throw new KeyNotFoundException($"Recipe with ID {recipeId} not found");

            recipe.ImageUrl = imageUrl;
            _dbContext.Recipes.Update(recipe);
            await _dbContext.SaveChangesAsync();

            return MapToDto(recipe);
        }
    }
}
