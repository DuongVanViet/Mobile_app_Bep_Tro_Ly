using System;
using System.Collections.Generic;

namespace BepTroLy.Application.DTOs
{
    public class ShoppingListDto
    {
        public int ShoppingListId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ShoppingListItemDto> Items { get; set; } = new();
    }

    public class ShoppingListItemDto
    {
        public int ItemId { get; set; }
        public string? IngredientName { get; set; }
        public decimal Quantity { get; set; }
        public string? Unit { get; set; }
        public bool IsChecked { get; set; }
    }

    public class CreateShoppingListRequest
    {
        public int UserId { get; set; }
        public List<CreateShoppingListItemRequest> Items { get; set; } = new();
    }

    public class CreateShoppingListItemRequest
    {
        public string? IngredientName { get; set; }
        public decimal Quantity { get; set; }
        public string? Unit { get; set; }
    }

    public class UpdateShoppingListItemRequest
    {
        public int ItemId { get; set; }
        public bool IsChecked { get; set; }
    }

    public class GenerateShoppingListRequest
    {
        public int UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
