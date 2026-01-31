using System.Collections.Generic;
using System.Threading.Tasks;
using BepTroLy.Application.DTOs;

namespace BepTroLy.Application.Interfaces
{
    public interface IRecommendationService
    {
        Task<IEnumerable<RecipeDto>> GetRecommendationsAsync(int userId);
    }
}
