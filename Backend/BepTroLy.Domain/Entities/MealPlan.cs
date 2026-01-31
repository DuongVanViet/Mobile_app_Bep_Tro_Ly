using System.Collections.Generic;

namespace BepTroLy.Domain.Entities
{
    public class MealPlan
    {
        public int MealPlanId { get; set; }
        public int UserId { get; set; }
        public int RecipeId { get; set; }
        public System.DateTime PlannedDate { get; set; }
        public string? MealType { get; set; }
        public int Servings { get; set; }

        // Navigation properties
        public User? User { get; set; }
        public Recipe? Recipe { get; set; }
    }
}
