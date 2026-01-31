using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BepTroLy.Infrastructure.Persistence;
using BepTroLy.Application.DTOs;
using BepTroLy.Application.Interfaces;
using BepTroLy.Domain.Entities;

namespace BepTroLy.Application.Services
{
    public class ShoppingListService : IShoppingListService
    {
        private readonly AppDbContext _dbContext;

        public ShoppingListService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ShoppingListDto> GenerateFromMealPlanAsync(int userId, DateTime startDate, DateTime endDate)
        {
            // Get meal plans for the date range
            var mealPlans = await _dbContext.MealPlans
                .Include(mp => mp.Recipe)
                    .ThenInclude(r => r.RecipeIngredients)
                        .ThenInclude(ri => ri.Ingredient)
                .Include(mp => mp.Recipe)
                    .ThenInclude(r => r.RecipeIngredients)
                        .ThenInclude(ri => ri.Unit)
                .Where(mp => mp.UserId == userId && mp.PlannedDate >= startDate && mp.PlannedDate <= endDate)
                .ToListAsync();

            // Get user's pantry items to exclude
            var userPantryItems = await _dbContext.UserIngredients
                .Where(ui => ui.UserId == userId)
                .Select(ui => ui.IngredientId)
                .ToListAsync();

            // Aggregate ingredients from all meal plans
            var ingredientMap = new Dictionary<string, (decimal quantity, string unit)>();

            foreach (var mealPlan in mealPlans)
            {
                if (mealPlan.Recipe?.RecipeIngredients == null)
                    continue;

                var servingMultiplier = mealPlan.Servings > 0 ? mealPlan.Servings / (decimal)(mealPlan.Recipe.Servings > 0 ? mealPlan.Recipe.Servings : 1) : 1;

                foreach (var ingredient in mealPlan.Recipe.RecipeIngredients)
                {
                    // Skip ingredients already in user's pantry
                    if (userPantryItems.Contains(ingredient.IngredientId))
                        continue;

                    var key = $"{ingredient.Ingredient?.IngredientName ?? "Unknown"}_{ingredient.Unit?.UnitName ?? ""}";

                    if (ingredientMap.ContainsKey(key))
                    {
                        var (qty, unit) = ingredientMap[key];
                        ingredientMap[key] = (qty + (ingredient.Quantity * servingMultiplier), unit);
                    }
                    else
                    {
                        ingredientMap[key] = (ingredient.Quantity * servingMultiplier, ingredient.Unit?.UnitName ?? "");
                    }
                }
            }

            // Create shopping list
            var shoppingList = new ShoppingList
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.ShoppingLists.Add(shoppingList);
            await _dbContext.SaveChangesAsync();

            // Add items to shopping list
            var items = new List<ShoppingListItem>();
            foreach (var (key, (qty, unit)) in ingredientMap)
            {
                var ingredientName = key.Split('_')[0];
                var item = new ShoppingListItem
                {
                    ShoppingListId = shoppingList.ShoppingListId,
                    IngredientName = ingredientName,
                    Quantity = (double)qty,
                    Unit = unit,
                    IsChecked = false
                };
                items.Add(item);
            }

            if (items.Any())
            {
                _dbContext.ShoppingListItems.AddRange(items);
                await _dbContext.SaveChangesAsync();
            }

            return MapToDto(shoppingList, items);
        }

        public async Task<ShoppingListDto> GetShoppingListByIdAsync(int shoppingListId)
        {
            var shoppingList = await _dbContext.ShoppingLists
                .FirstOrDefaultAsync(sl => sl.ShoppingListId == shoppingListId);

            if (shoppingList == null)
                throw new KeyNotFoundException($"Shopping list with ID {shoppingListId} not found");

            var items = await _dbContext.ShoppingListItems
                .Where(sli => sli.ShoppingListId == shoppingListId)
                .ToListAsync();

            return MapToDto(shoppingList, items);
        }

        public async Task<IEnumerable<ShoppingListDto>> GetUserShoppingListsAsync(int userId)
        {
            var shoppingLists = await _dbContext.ShoppingLists
                .Where(sl => sl.UserId == userId)
                .OrderByDescending(sl => sl.CreatedAt)
                .ToListAsync();

            var result = new List<ShoppingListDto>();
            foreach (var sl in shoppingLists)
            {
                var items = await _dbContext.ShoppingListItems
                    .Where(sli => sli.ShoppingListId == sl.ShoppingListId)
                    .ToListAsync();
                result.Add(MapToDto(sl, items));
            }

            return result;
        }

        public async Task<ShoppingListDto> CreateShoppingListAsync(CreateShoppingListRequest request)
        {
            var shoppingList = new ShoppingList
            {
                UserId = request.UserId,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.ShoppingLists.Add(shoppingList);
            await _dbContext.SaveChangesAsync();

            var items = request.Items.Select(item => new ShoppingListItem
            {
                ShoppingListId = shoppingList.ShoppingListId,
                IngredientName = item.IngredientName,
                Quantity = (double)item.Quantity,
                Unit = item.Unit,
                IsChecked = false
            }).ToList();

            if (items.Any())
            {
                _dbContext.ShoppingListItems.AddRange(items);
                await _dbContext.SaveChangesAsync();
            }

            return MapToDto(shoppingList, items);
        }

        public async Task<ShoppingListDto> UpdateShoppingListItemAsync(int shoppingListId, UpdateShoppingListItemRequest request)
        {
            var item = await _dbContext.ShoppingListItems
                .FirstOrDefaultAsync(sli => sli.ItemId == request.ItemId && sli.ShoppingListId == shoppingListId);

            if (item == null)
                throw new KeyNotFoundException($"Shopping list item with ID {request.ItemId} not found");

            item.IsChecked = request.IsChecked;
            _dbContext.ShoppingListItems.Update(item);
            await _dbContext.SaveChangesAsync();

            var shoppingList = await _dbContext.ShoppingLists
                .FirstOrDefaultAsync(sl => sl.ShoppingListId == shoppingListId);

            var items = await _dbContext.ShoppingListItems
                .Where(sli => sli.ShoppingListId == shoppingListId)
                .ToListAsync();

            return MapToDto(shoppingList, items);
        }

        public async Task<bool> DeleteShoppingListAsync(int shoppingListId, int userId)
        {
            var shoppingList = await _dbContext.ShoppingLists
                .FirstOrDefaultAsync(sl => sl.ShoppingListId == shoppingListId && sl.UserId == userId);

            if (shoppingList == null)
                return false;

            var items = await _dbContext.ShoppingListItems
                .Where(sli => sli.ShoppingListId == shoppingListId)
                .ToListAsync();

            _dbContext.ShoppingListItems.RemoveRange(items);
            _dbContext.ShoppingLists.Remove(shoppingList);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        private ShoppingListDto MapToDto(ShoppingList shoppingList, List<ShoppingListItem> items)
        {
            return new ShoppingListDto
            {
                ShoppingListId = shoppingList.ShoppingListId,
                UserId = shoppingList.UserId,
                CreatedAt = shoppingList.CreatedAt,
                Items = items?.Select(item => new ShoppingListItemDto
                {
                    ItemId = item.ItemId,
                    IngredientName = item.IngredientName,
                    Quantity = (decimal)item.Quantity,
                    Unit = item.Unit,
                    IsChecked = item.IsChecked
                }).ToList() ?? new List<ShoppingListItemDto>()
            };
        }
    }
}
