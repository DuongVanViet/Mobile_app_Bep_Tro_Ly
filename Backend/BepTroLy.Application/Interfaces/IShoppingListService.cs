using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BepTroLy.Application.DTOs;

namespace BepTroLy.Application.Interfaces
{
    public interface IShoppingListService
    {
        Task<ShoppingListDto> GenerateFromMealPlanAsync(int userId, DateTime startDate, DateTime endDate);
        Task<ShoppingListDto> GetShoppingListByIdAsync(int shoppingListId);
        Task<IEnumerable<ShoppingListDto>> GetUserShoppingListsAsync(int userId);
        Task<ShoppingListDto> CreateShoppingListAsync(CreateShoppingListRequest request);
        Task<ShoppingListDto> UpdateShoppingListItemAsync(int shoppingListId, UpdateShoppingListItemRequest request);
        Task<bool> DeleteShoppingListAsync(int shoppingListId, int userId);
    }
}
