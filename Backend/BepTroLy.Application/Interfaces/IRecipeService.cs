using System.Collections.Generic;
using System.Threading.Tasks;
using BepTroLy.Application.DTOs;

namespace BepTroLy.Application.Interfaces
{
    public interface IRecipeService
    {
        // Recipe Category Management
        Task<IEnumerable<RecipeCategoryDto>> GetAllCategoriesAsync();
        Task<RecipeCategoryDto> GetCategoryByIdAsync(int categoryId);
        Task<RecipeCategoryDto> CreateCategoryAsync(CreateRecipeCategoryRequest request);
        Task UpdateCategoryAsync(int categoryId, CreateRecipeCategoryRequest request);
        Task DeleteCategoryAsync(int categoryId);

        // Recipe Management
        Task<IEnumerable<RecipeDto>> GetAllRecipesAsync();
        Task<RecipeDto> GetRecipeByIdAsync(int recipeId);
        Task<RecipeDto> CreateRecipeAsync(CreateRecipeRequest request);
        Task UpdateRecipeAsync(int recipeId, UpdateRecipeRequest request);
        Task DeleteRecipeAsync(int recipeId);
            Task<RecipeDto> UpdateRecipeImageAsync(int recipeId, string imageUrl);
        Task<IEnumerable<RecipeDto>> SearchRecipesAsync(SearchRecipeRequest request);
    }
}
