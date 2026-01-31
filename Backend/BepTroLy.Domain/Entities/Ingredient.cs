namespace BepTroLy.Domain.Entities
{
    public class Ingredient
    {
        public int IngredientId { get; set; }
        public string? IngredientName { get; set; }
        public int? CategoryId { get; set; }
        
        // Navigation properties
        public IngredientCategory? Category { get; set; }
        
        // Compatibility wrappers
        public int Id { get => IngredientId; set => IngredientId = value; }
        public string? Name { get => IngredientName; set => IngredientName = value; }
        public string? Unit { get; set; }
    }
}
