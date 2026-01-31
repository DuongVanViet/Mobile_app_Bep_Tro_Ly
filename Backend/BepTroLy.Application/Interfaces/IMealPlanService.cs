using System.Collections.Generic;
using System.Threading.Tasks;
using BepTroLy.Application.DTOs;

namespace BepTroLy.Application.Interfaces
{
    public interface IMealPlanService
    {
        Task<IEnumerable<MealPlanDto>> GetUserMealPlansAsync(int userId);
        Task<IEnumerable<MealPlanDto>> GetMealPlansForWeekAsync(int userId, System.DateTime startDate);
        Task<MealPlanDto> GetMealPlanByIdAsync(int mealPlanId);
        Task<MealPlanDto> CreateMealPlanAsync(int userId, CreateMealPlanRequest request);
        Task UpdateMealPlanAsync(int mealPlanId, UpdateMealPlanRequest request);
        Task DeleteMealPlanAsync(int mealPlanId);
    }
}
