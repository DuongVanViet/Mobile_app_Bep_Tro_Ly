using System.Collections.Generic;
using System.Linq;
using BepTroLy.Domain.Entities;
using BepTroLy.Infrastructure.Persistence;

namespace BepTroLy.Infrastructure.Repositories
{
    public class IngredientRepository
    {
        private readonly AppDbContext _db;

        public IngredientRepository(AppDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Ingredient> GetAll()
        {
            // For scaffold purposes, return in-memory sample if DB empty
            var items = _db.Ingredients?.ToList();
            if (items == null || items.Count == 0)
            {
                return new List<Ingredient>
                {
                    new Ingredient { Id = 1, Name = "Tomato", Unit = "pcs" },
                    new Ingredient { Id = 2, Name = "Salt", Unit = "g" }
                };
            }

            return items;
        }
    }
}
