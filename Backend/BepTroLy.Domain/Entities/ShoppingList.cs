using System;

namespace BepTroLy.Domain.Entities
{
    public class ShoppingList
    {
        public int ShoppingListId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
