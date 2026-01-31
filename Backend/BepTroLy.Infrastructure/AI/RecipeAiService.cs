using System.Collections.Generic;
using BepTroLy.Domain.Entities;

namespace BepTroLy.Infrastructure.AI
{
    public class RecipeAiService
    {
        public IEnumerable<Recipe> GetRecommendations()
        {
            // Placeholder: integrate ML/AI model or external API
            return new List<Recipe>
            {
                new Recipe { Id = 100, Title = "AI Suggested Salad", Ingredients = new List<string>{"Lettuce","Tomato"} }
            };
        }
    }
}
