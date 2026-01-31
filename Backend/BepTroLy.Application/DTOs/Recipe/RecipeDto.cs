using System.Collections.Generic;

namespace BepTroLy.Application.DTOs.Recipe
{
    public class RecipeDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public List<string>? Ingredients { get; set; }
    }
}
