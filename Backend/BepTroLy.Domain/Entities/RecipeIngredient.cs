namespace BepTroLy.Domain.Entities
{
    public class RecipeIngredient
    {
        public int RecipeIngredientId { get; set; }
        public int RecipeId { get; set; }
        public int IngredientId { get; set; }
        public decimal Quantity { get; set; }
        public int? UnitId { get; set; }

        // Navigation properties
        public Recipe? Recipe { get; set; }
        public Ingredient? Ingredient { get; set; }
        public Unit? Unit { get; set; }
    }
}
