using System.Collections.Generic;
using System.Linq;
using BepTroLy.Domain.Entities;
using BepTroLy.Infrastructure.Persistence;

namespace BepTroLy.Infrastructure.Repositories
{
    public class RecipeRepository
    {
        private readonly AppDbContext _db;

        public RecipeRepository(AppDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Recipe> GetAll()
        {
            var items = _db.Set<Recipe>()?.ToList();
            if (items == null || items.Count == 0)
            {
                return new List<Recipe>
                {
                    new Recipe { Id = 1, Title = "Tomato Soup", Ingredients = new List<string>{"Tomato","Salt"} }
                };
            }
            return items;
        }
    }
}
