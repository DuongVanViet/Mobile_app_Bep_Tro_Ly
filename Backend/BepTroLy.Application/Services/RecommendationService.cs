using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BepTroLy.Application.DTOs;
using BepTroLy.Application.Interfaces;
using BepTroLy.Infrastructure.Persistence;

namespace BepTroLy.Application.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly AppDbContext _dbContext;

        public RecommendationService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<RecipeDto>> GetRecommendationsAsync(int userId)
        {
            // Get user's pantry ingredients
            var userIngredients = await _dbContext.UserIngredients
                .Where(ui => ui.UserId == userId && !ui.IsDeleted)
                .Select(ui => ui.IngredientId)
                .ToListAsync();

            if (!userIngredients.Any())
                return new List<RecipeDto>();

            // Get all recipes with ingredients
            var recipes = await _dbContext.Recipes
                .Include(r => r.Category)
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Unit)
                .Include(r => r.Steps)
                .ToListAsync();

            // Score recipes based on how many ingredients the user has
            var scoredRecipes = recipes
                .Select(r => new
                {
                    Recipe = r,
                    MatchScore = CalculateRecipeScore(r, userIngredients),
                    MatchPercentage = CalculateMatchPercentage(r, userIngredients)
                })
                .Where(x => x.MatchPercentage > 0) // Only recipes with at least one available ingredient
                .OrderByDescending(x => x.MatchPercentage)
                .ThenByDescending(x => x.MatchScore)
                .Take(10) // Return top 10 recommendations
                .Select(x => MapToDto(x.Recipe))
                .ToList();

            return scoredRecipes;
        }

        private int CalculateRecipeScore(BepTroLy.Domain.Entities.Recipe recipe, List<int> userIngredients)
        {
            if (!recipe.RecipeIngredients.Any())
                return 0;

            var matchingIngredients = recipe.RecipeIngredients
                .Count(ri => userIngredients.Contains(ri.IngredientId));

            return matchingIngredients;
        }

        private decimal CalculateMatchPercentage(BepTroLy.Domain.Entities.Recipe recipe, List<int> userIngredients)
        {
            if (!recipe.RecipeIngredients.Any())
                return 0;

            var matchingIngredients = recipe.RecipeIngredients
                .Count(ri => userIngredients.Contains(ri.IngredientId));

            return (decimal)matchingIngredients / recipe.RecipeIngredients.Count * 100;
        }

        private RecipeDto MapToDto(BepTroLy.Domain.Entities.Recipe recipe)
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
    }
}
