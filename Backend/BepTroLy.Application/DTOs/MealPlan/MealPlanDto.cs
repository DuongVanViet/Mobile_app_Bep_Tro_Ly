using System.Collections.Generic;

namespace BepTroLy.Application.DTOs.MealPlan
{
    public class MealPlanDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<int>? RecipeIds { get; set; }
    }
}
