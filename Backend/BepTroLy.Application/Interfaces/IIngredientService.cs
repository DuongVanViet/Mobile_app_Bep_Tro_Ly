using System.Collections.Generic;
using System.Threading.Tasks;
using BepTroLy.Application.DTOs;

namespace BepTroLy.Application.Interfaces
{
    public interface IIngredientService
    {
        // Ingredient Category Management
        Task<IEnumerable<IngredientCategoryDto>> GetAllCategoriesAsync();
        Task<IngredientCategoryDto> GetCategoryByIdAsync(int categoryId);
        Task<IngredientCategoryDto> CreateCategoryAsync(CreateIngredientCategoryRequest request);
        Task UpdateCategoryAsync(int categoryId, CreateIngredientCategoryRequest request);
        Task DeleteCategoryAsync(int categoryId);

        // Ingredient Management
        Task<IEnumerable<IngredientDto>> GetAllIngredientsAsync();
        Task<IngredientDto> GetIngredientByIdAsync(int ingredientId);
        Task<IngredientDto> CreateIngredientAsync(CreateIngredientRequest request);
        Task UpdateIngredientAsync(int ingredientId, UpdateIngredientRequest request);
        Task DeleteIngredientAsync(int ingredientId);

        // User Ingredient (Pantry) Management
        Task<IEnumerable<UserIngredientDto>> GetUserPantryAsync(int userId);
        Task<UserIngredientDto> GetUserIngredientAsync(int userIngredientId);
        Task<UserIngredientDto> AddIngredientToPantryAsync(int userId, AddUserIngredientRequest request);
        Task UpdatePantryIngredientAsync(int userIngredientId, UpdateUserIngredientRequest request);
        Task RemoveIngredientFromPantryAsync(int userIngredientId);
    }
}
