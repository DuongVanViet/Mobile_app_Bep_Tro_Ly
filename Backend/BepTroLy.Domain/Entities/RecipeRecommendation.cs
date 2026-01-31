using System;

namespace BepTroLy.Domain.Entities
{
    public class RecipeRecommendation
    {
        public int RecommendationId { get; set; }
        public int UserId { get; set; }
        public int RecipeId { get; set; }
        public double MatchPercent { get; set; }
        public string? MissingIngredients { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
