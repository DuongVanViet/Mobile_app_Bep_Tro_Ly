using System.Collections.Generic;

namespace BepTroLy.Domain.Entities
{
    public class Recipe
    {
        public int RecipeId { get; set; }
        public string? RecipeName { get; set; }
        public int? CategoryId { get; set; }
        public string? Description { get; set; }
        public int PrepTimeMinutes { get; set; }
        public int CookTimeMinutes { get; set; }
        public int Servings { get; set; }
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }

        // Navigation properties
        public RecipeCategory? Category { get; set; }
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
        public ICollection<RecipeStep> Steps { get; set; } = new List<RecipeStep>();

        // Compatibility wrappers for existing code
        public int Id { get => RecipeId; set => RecipeId = value; }
        public string? Title { get => RecipeName; set => RecipeName = value; }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public System.Collections.Generic.List<string>? Ingredients { get; set; }
    }
}

