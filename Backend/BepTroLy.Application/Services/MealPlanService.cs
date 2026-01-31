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
    public class MealPlanService : IMealPlanService
    {
        private readonly AppDbContext _dbContext;

        public MealPlanService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<MealPlanDto>> GetUserMealPlansAsync(int userId)
        {
            var mealPlans = await _dbContext.MealPlans
                .Include(mp => mp.User)
                .Include(mp => mp.Recipe)
                    .ThenInclude(r => r.RecipeIngredients)
                        .ThenInclude(ri => ri.Ingredient)
                .Include(mp => mp.Recipe)
                    .ThenInclude(r => r.RecipeIngredients)
                        .ThenInclude(ri => ri.Unit)
                .Include(mp => mp.Recipe)
                    .ThenInclude(r => r.Steps)
                .Where(mp => mp.UserId == userId)
                .OrderByDescending(mp => mp.PlannedDate)
                .ToListAsync();

            return mealPlans.Select(mp => MapToDto(mp));
        }

        public async Task<IEnumerable<MealPlanDto>> GetMealPlansForWeekAsync(int userId, DateTime startDate)
        {
            var endDate = startDate.AddDays(7);

            var mealPlans = await _dbContext.MealPlans
                .Include(mp => mp.Recipe)
                    .ThenInclude(r => r.RecipeIngredients)
                        .ThenInclude(ri => ri.Ingredient)
                .Include(mp => mp.Recipe)
                    .ThenInclude(r => r.RecipeIngredients)
                        .ThenInclude(ri => ri.Unit)
                .Include(mp => mp.Recipe)
                    .ThenInclude(r => r.Steps)
                .Where(mp => mp.UserId == userId && mp.PlannedDate >= startDate && mp.PlannedDate < endDate)
                .OrderBy(mp => mp.PlannedDate)
                .ToListAsync();

            return mealPlans.Select(mp => MapToDto(mp));
        }

        public async Task<MealPlanDto> GetMealPlanByIdAsync(int mealPlanId)
        {
            var mealPlan = await _dbContext.MealPlans
                .Include(mp => mp.Recipe)
                    .ThenInclude(r => r.RecipeIngredients)
                        .ThenInclude(ri => ri.Ingredient)
                .Include(mp => mp.Recipe)
                    .ThenInclude(r => r.RecipeIngredients)
                        .ThenInclude(ri => ri.Unit)
                .Include(mp => mp.Recipe)
                    .ThenInclude(r => r.Steps)
                .FirstOrDefaultAsync(mp => mp.MealPlanId == mealPlanId);

            if (mealPlan == null)
                throw new KeyNotFoundException($"Meal plan with ID {mealPlanId} not found");

            return MapToDto(mealPlan);
        }

        public async Task<MealPlanDto> CreateMealPlanAsync(int userId, CreateMealPlanRequest request)
        {
            // Verify recipe exists
            var recipe = await _dbContext.Recipes.FindAsync(request.RecipeId);
            if (recipe == null)
                throw new KeyNotFoundException($"Recipe with ID {request.RecipeId} not found");

            var mealPlan = new MealPlan
            {
                UserId = userId,
                RecipeId = request.RecipeId,
                PlannedDate = request.PlannedDate,
                MealType = request.MealType,
                Servings = request.Servings
            };

            _dbContext.MealPlans.Add(mealPlan);
            await _dbContext.SaveChangesAsync();

            return await GetMealPlanByIdAsync(mealPlan.MealPlanId);
        }

        public async Task UpdateMealPlanAsync(int mealPlanId, UpdateMealPlanRequest request)
        {
            var mealPlan = await _dbContext.MealPlans.FindAsync(mealPlanId);
            if (mealPlan == null)
                throw new KeyNotFoundException($"Meal plan with ID {mealPlanId} not found");

            if (request.PlannedDate.HasValue)
                mealPlan.PlannedDate = request.PlannedDate.Value;

            if (!string.IsNullOrWhiteSpace(request.MealType))
                mealPlan.MealType = request.MealType;

            if (request.Servings.HasValue && request.Servings > 0)
                mealPlan.Servings = request.Servings.Value;

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteMealPlanAsync(int mealPlanId)
        {
            var mealPlan = await _dbContext.MealPlans.FindAsync(mealPlanId);
            if (mealPlan == null)
                throw new KeyNotFoundException($"Meal plan with ID {mealPlanId} not found");

            _dbContext.MealPlans.Remove(mealPlan);
            await _dbContext.SaveChangesAsync();
        }

        private MealPlanDto MapToDto(MealPlan mealPlan)
        {
            return new MealPlanDto
            {
                MealPlanId = mealPlan.MealPlanId,
                UserId = mealPlan.UserId,
                RecipeId = mealPlan.RecipeId,
                RecipeName = mealPlan.Recipe == null ? null : mealPlan.Recipe.RecipeName,
                PlannedDate = mealPlan.PlannedDate,
                MealType = mealPlan.MealType,
                Servings = mealPlan.Servings,
                Ingredients = mealPlan.Recipe == null ? new List<RecipeIngredientDto>() : mealPlan.Recipe.RecipeIngredients?.Select(ri => new RecipeIngredientDto
                {
                    RecipeIngredientId = ri.RecipeIngredientId,
                    IngredientId = ri.IngredientId,
                    IngredientName = ri.Ingredient == null ? null : ri.Ingredient.IngredientName,
                    Quantity = ri.Quantity,
                    UnitId = ri.UnitId,
                    UnitName = ri.Unit == null ? null : ri.Unit.UnitName
                }).ToList() ?? new List<RecipeIngredientDto>(),
                Steps = mealPlan.Recipe == null ? new List<RecipeStepDto>() : mealPlan.Recipe.Steps?.OrderBy(s => s.StepNumber).Select(s => new RecipeStepDto
                {
                    StepId = s.StepId,
                    StepNumber = s.StepNumber,
                    Description = s.Description,
                    DurationMinutes = s.DurationMinutes
                }).ToList() ?? new List<RecipeStepDto>()
            };
        }
    }
}
