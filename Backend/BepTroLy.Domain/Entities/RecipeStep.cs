namespace BepTroLy.Domain.Entities
{
    public class RecipeStep
    {
        public int StepId { get; set; }
        public int RecipeId { get; set; }
        public int StepNumber { get; set; }
        public string? Description { get; set; }
        public int DurationMinutes { get; set; }

        // Navigation property
        public Recipe? Recipe { get; set; }
    }
}
