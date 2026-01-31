using System;

namespace BepTroLy.Domain.Entities
{
    public class UserIngredient
    {
        public int UserIngredientId { get; set; }
        public int UserId { get; set; }
        public int IngredientId { get; set; }
        public decimal Quantity { get; set; }
        public int? UnitId { get; set; }
        public System.DateTime? PurchaseDate { get; set; }
        public System.DateTime? ExpiryDate { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation properties
        public User? User { get; set; }
        public Ingredient? Ingredient { get; set; }
        public Unit? Unit { get; set; }
    }
}
