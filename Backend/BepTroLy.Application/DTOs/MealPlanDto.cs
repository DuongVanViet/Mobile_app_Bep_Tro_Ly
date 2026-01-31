namespace BepTroLy.Application.DTOs
{
    public class MealPlanDto
    {
        public int MealPlanId { get; set; }
        public int UserId { get; set; }
        public int RecipeId { get; set; }
        public string? RecipeName { get; set; }
        public System.DateTime PlannedDate { get; set; }
        public string? MealType { get; set; } // Breakfast, Lunch, Dinner
        public int Servings { get; set; }
        public List<RecipeIngredientDto>? Ingredients { get; set; }
        public List<RecipeStepDto>? Steps { get; set; }
    }

    public class CreateMealPlanRequest
    {
        public int RecipeId { get; set; }
        public System.DateTime PlannedDate { get; set; }
        public string? MealType { get; set; }
        public int Servings { get; set; }
    }

    public class UpdateMealPlanRequest
    {
        public System.DateTime? PlannedDate { get; set; }
        public string? MealType { get; set; }
        public int? Servings { get; set; }
    }

    public class MealPlanWeekDto
    {
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public List<MealPlanDto>? Meals { get; set; }
    }
}
