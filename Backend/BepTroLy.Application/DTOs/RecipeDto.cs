namespace BepTroLy.Application.DTOs
{
    public class RecipeDto
    {
        public int RecipeId { get; set; }
        public string? RecipeName { get; set; }
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int PrepTimeMinutes { get; set; }
        public int CookTimeMinutes { get; set; }
        public int Servings { get; set; }
        public string? ImageUrl { get; set; }
        public List<RecipeIngredientDto>? Ingredients { get; set; }
        public List<RecipeStepDto>? Steps { get; set; }
    }

    public class CreateRecipeRequest
    {
        public string? RecipeName { get; set; }
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public int PrepTimeMinutes { get; set; }
        public int CookTimeMinutes { get; set; }
        public int Servings { get; set; }
        public List<CreateRecipeIngredientRequest>? Ingredients { get; set; }
        public List<CreateRecipeStepRequest>? Steps { get; set; }
    }

    public class UpdateRecipeRequest
    {
        public string? RecipeName { get; set; }
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public int PrepTimeMinutes { get; set; }
        public int CookTimeMinutes { get; set; }
        public int Servings { get; set; }
    }

    public class RecipeIngredientDto
    {
        public int RecipeIngredientId { get; set; }
        public int IngredientId { get; set; }
        public string? IngredientName { get; set; }
        public decimal Quantity { get; set; }
        public int? UnitId { get; set; }
        public string? UnitName { get; set; }
    }

    public class CreateRecipeIngredientRequest
    {
        public int IngredientId { get; set; }
        public decimal Quantity { get; set; }
        public int? UnitId { get; set; }
    }

    public class RecipeStepDto
    {
        public int StepId { get; set; }
        public int StepNumber { get; set; }
        public string? Description { get; set; }
        public int DurationMinutes { get; set; }
    }

    public class CreateRecipeStepRequest
    {
        public int StepNumber { get; set; }
        public string? Description { get; set; }
        public int DurationMinutes { get; set; }
    }

    public class RecipeCategoryDto
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }

    public class CreateRecipeCategoryRequest
    {
        public string? CategoryName { get; set; }
    }

    public class SearchRecipeRequest
    {
        public string? SearchTerm { get; set; }
        public int? CategoryId { get; set; }
        public int? MaxPrepTime { get; set; }
        public int? MaxCookTime { get; set; }
        public List<int>? RequiredIngredientIds { get; set; }
    }
}
