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
    public class IngredientService : IIngredientService
    {
        private readonly AppDbContext _dbContext;

        public IngredientService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ============= Ingredient Category Management =============
        public async Task<IEnumerable<IngredientCategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _dbContext.IngredientCategories.ToListAsync();
            return categories.Select(c => new IngredientCategoryDto
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName
            });
        }

        public async Task<IngredientCategoryDto> GetCategoryByIdAsync(int categoryId)
        {
            var category = await _dbContext.IngredientCategories.FindAsync(categoryId);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {categoryId} not found");

            return new IngredientCategoryDto
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName
            };
        }

        public async Task<IngredientCategoryDto> CreateCategoryAsync(CreateIngredientCategoryRequest request)
        {
            var category = new IngredientCategory
            {
                CategoryName = request.CategoryName
            };

            _dbContext.IngredientCategories.Add(category);
            await _dbContext.SaveChangesAsync();

            return new IngredientCategoryDto
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName
            };
        }

        public async Task UpdateCategoryAsync(int categoryId, CreateIngredientCategoryRequest request)
        {
            var category = await _dbContext.IngredientCategories.FindAsync(categoryId);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {categoryId} not found");

            category.CategoryName = request.CategoryName;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            var category = await _dbContext.IngredientCategories.FindAsync(categoryId);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {categoryId} not found");

            _dbContext.IngredientCategories.Remove(category);
            await _dbContext.SaveChangesAsync();
        }

        // ============= Ingredient Management =============
        public async Task<IEnumerable<IngredientDto>> GetAllIngredientsAsync()
        {
            var ingredients = await _dbContext.Ingredients
                .Include(i => i.Category)
                .ToListAsync();

            return ingredients.Select(i => new IngredientDto
            {
                IngredientId = i.IngredientId,
                IngredientName = i.IngredientName,
                CategoryId = i.CategoryId,
                CategoryName = i.Category?.CategoryName
            });
        }

        public async Task<IngredientDto> GetIngredientByIdAsync(int ingredientId)
        {
            var ingredient = await _dbContext.Ingredients
                .Include(i => i.Category)
                .FirstOrDefaultAsync(i => i.IngredientId == ingredientId);

            if (ingredient == null)
                throw new KeyNotFoundException($"Ingredient with ID {ingredientId} not found");

            return new IngredientDto
            {
                IngredientId = ingredient.IngredientId,
                IngredientName = ingredient.IngredientName,
                CategoryId = ingredient.CategoryId,
                CategoryName = ingredient.Category?.CategoryName
            };
        }

        public async Task<IngredientDto> CreateIngredientAsync(CreateIngredientRequest request)
        {
            var ingredient = new Ingredient
            {
                IngredientName = request.IngredientName,
                CategoryId = request.CategoryId
            };

            _dbContext.Ingredients.Add(ingredient);
            await _dbContext.SaveChangesAsync();

            var category = request.CategoryId.HasValue
                ? await _dbContext.IngredientCategories.FindAsync(request.CategoryId)
                : null;

            return new IngredientDto
            {
                IngredientId = ingredient.IngredientId,
                IngredientName = ingredient.IngredientName,
                CategoryId = ingredient.CategoryId,
                CategoryName = category?.CategoryName
            };
        }

        public async Task UpdateIngredientAsync(int ingredientId, UpdateIngredientRequest request)
        {
            var ingredient = await _dbContext.Ingredients.FindAsync(ingredientId);
            if (ingredient == null)
                throw new KeyNotFoundException($"Ingredient with ID {ingredientId} not found");

            ingredient.IngredientName = request.IngredientName ?? ingredient.IngredientName;
            ingredient.CategoryId = request.CategoryId ?? ingredient.CategoryId;

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteIngredientAsync(int ingredientId)
        {
            var ingredient = await _dbContext.Ingredients.FindAsync(ingredientId);
            if (ingredient == null)
                throw new KeyNotFoundException($"Ingredient with ID {ingredientId} not found");

            _dbContext.Ingredients.Remove(ingredient);
            await _dbContext.SaveChangesAsync();
        }

        // ============= User Ingredient (Pantry) Management =============
        public async Task<IEnumerable<UserIngredientDto>> GetUserPantryAsync(int userId)
        {
            var userIngredients = await _dbContext.UserIngredients
                .Include(ui => ui.Ingredient)
                .Include(ui => ui.Unit)
                .Where(ui => ui.UserId == userId && !ui.IsDeleted)
                .ToListAsync();

            return userIngredients.Select(ui => new UserIngredientDto
            {
                UserIngredientId = ui.UserIngredientId,
                UserId = ui.UserId,
                IngredientId = ui.IngredientId,
                IngredientName = ui.Ingredient?.IngredientName,
                Quantity = ui.Quantity,
                UnitId = ui.UnitId,
                UnitName = ui.Unit?.UnitName,
                ExpiryDate = ui.ExpiryDate,
                IsDeleted = ui.IsDeleted
            });
        }

        public async Task<UserIngredientDto> GetUserIngredientAsync(int userIngredientId)
        {
            var userIngredient = await _dbContext.UserIngredients
                .Include(ui => ui.Ingredient)
                .Include(ui => ui.Unit)
                .FirstOrDefaultAsync(ui => ui.UserIngredientId == userIngredientId);

            if (userIngredient == null)
                throw new KeyNotFoundException($"User ingredient with ID {userIngredientId} not found");

            return new UserIngredientDto
            {
                UserIngredientId = userIngredient.UserIngredientId,
                UserId = userIngredient.UserId,
                IngredientId = userIngredient.IngredientId,
                IngredientName = userIngredient.Ingredient?.IngredientName,
                Quantity = userIngredient.Quantity,
                UnitId = userIngredient.UnitId,
                UnitName = userIngredient.Unit?.UnitName,
                ExpiryDate = userIngredient.ExpiryDate,
                IsDeleted = userIngredient.IsDeleted
            };
        }

        public async Task<UserIngredientDto> AddIngredientToPantryAsync(int userId, AddUserIngredientRequest request)
        {
            // Verify ingredient exists
            var ingredient = await _dbContext.Ingredients.FindAsync(request.IngredientId);
            if (ingredient == null)
                throw new KeyNotFoundException($"Ingredient with ID {request.IngredientId} not found");

            // Verify unit exists if provided
            if (request.UnitId.HasValue)
            {
                var unit = await _dbContext.Units.FindAsync(request.UnitId);
                if (unit == null)
                    throw new KeyNotFoundException($"Unit with ID {request.UnitId} not found");
            }

            var userIngredient = new UserIngredient
            {
                UserId = userId,
                IngredientId = request.IngredientId,
                Quantity = request.Quantity,
                UnitId = request.UnitId,
                ExpiryDate = request.ExpiryDate,
                IsDeleted = false
            };

            _dbContext.UserIngredients.Add(userIngredient);
            await _dbContext.SaveChangesAsync();

            return await GetUserIngredientAsync(userIngredient.UserIngredientId);
        }

        public async Task UpdatePantryIngredientAsync(int userIngredientId, UpdateUserIngredientRequest request)
        {
            var userIngredient = await _dbContext.UserIngredients.FindAsync(userIngredientId);
            if (userIngredient == null)
                throw new KeyNotFoundException($"User ingredient with ID {userIngredientId} not found");

            // Verify unit exists if provided
            if (request.UnitId.HasValue && request.UnitId != userIngredient.UnitId)
            {
                var unit = await _dbContext.Units.FindAsync(request.UnitId);
                if (unit == null)
                    throw new KeyNotFoundException($"Unit with ID {request.UnitId} not found");
            }

            userIngredient.Quantity = request.Quantity;
            userIngredient.UnitId = request.UnitId ?? userIngredient.UnitId;
            userIngredient.ExpiryDate = request.ExpiryDate ?? userIngredient.ExpiryDate;

            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveIngredientFromPantryAsync(int userIngredientId)
        {
            var userIngredient = await _dbContext.UserIngredients.FindAsync(userIngredientId);
            if (userIngredient == null)
                throw new KeyNotFoundException($"User ingredient with ID {userIngredientId} not found");

            _dbContext.UserIngredients.Remove(userIngredient);
            await _dbContext.SaveChangesAsync();
        }
    }
}
