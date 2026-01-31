using Microsoft.EntityFrameworkCore;
using BepTroLy.Domain.Entities;
using BepTroLy.Domain.Enums;

namespace BepTroLy.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserSettings> UserSettings { get; set; }

        public DbSet<IngredientCategory> IngredientCategories { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<UserIngredient> UserIngredients { get; set; }

        public DbSet<RecipeCategory> RecipeCategories { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public DbSet<RecipeStep> RecipeSteps { get; set; }

        public DbSet<RecipeRecommendation> RecipeRecommendations { get; set; }

        public DbSet<ExpiryNotification> ExpiryNotifications { get; set; }

        public DbSet<MealPlan> MealPlans { get; set; }

        public DbSet<ShoppingList> ShoppingLists { get; set; }
        public DbSet<ShoppingListItem> ShoppingListItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Email).HasMaxLength(100).IsRequired();
                b.Property(x => x.Name).HasMaxLength(100);
                b.Property(x => x.PasswordHash).HasMaxLength(255);
                b.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                b.HasIndex(x => x.Email).IsUnique();
            });

            modelBuilder.Entity<UserSettings>(b =>
            {
                b.HasKey(x => x.SettingId);
                b.HasOne<User>().WithOne().HasForeignKey<UserSettings>(s => s.UserId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<IngredientCategory>(b =>
            {
                b.HasKey(x => x.CategoryId);
                b.Property(x => x.CategoryName).HasMaxLength(50).IsRequired();
            });

            modelBuilder.Entity<Unit>(b =>
            {
                b.HasKey(x => x.UnitId);
                b.Property(x => x.UnitName).HasMaxLength(20).IsRequired();
            });

            modelBuilder.Entity<Ingredient>(b =>
            {
                b.HasKey(x => x.IngredientId);
                b.Property(x => x.IngredientName).HasMaxLength(100).IsRequired();
                b.HasOne<IngredientCategory>().WithMany().HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<UserIngredient>(b =>
            {
                b.HasKey(x => x.UserIngredientId);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne<Ingredient>().WithMany().HasForeignKey(x => x.IngredientId).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<Unit>().WithMany().HasForeignKey(x => x.UnitId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<RecipeCategory>(b =>
            {
                b.HasKey(x => x.CategoryId);
                b.Property(x => x.CategoryName).HasMaxLength(50).IsRequired();
            });

            modelBuilder.Entity<Recipe>(b =>
            {
                b.HasKey(x => x.RecipeId);
                b.Property(x => x.RecipeName).HasMaxLength(150).IsRequired();
                b.Property(x => x.Description).HasMaxLength(500);
                b.Property(x => x.PrepTimeMinutes).HasDefaultValue(0);
                b.Property(x => x.CookTimeMinutes).HasDefaultValue(0);
                b.Property(x => x.Servings).HasDefaultValue(1);
                b.HasOne<RecipeCategory>().WithMany().HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<RecipeIngredient>(b =>
            {
                b.HasKey(x => x.RecipeIngredientId);
                b.Property(x => x.Quantity).HasPrecision(18, 2);
                b.HasOne<Recipe>().WithMany(r => r.RecipeIngredients).HasForeignKey(x => x.RecipeId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne<Ingredient>().WithMany().HasForeignKey(x => x.IngredientId).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<Unit>().WithMany().HasForeignKey(x => x.UnitId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<RecipeStep>(b =>
            {
                b.HasKey(x => x.StepId);
                b.Property(x => x.Description).HasMaxLength(1000);
                b.Property(x => x.DurationMinutes).HasDefaultValue(0);
                b.HasOne<Recipe>().WithMany(r => r.Steps).HasForeignKey(x => x.RecipeId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<RecipeRecommendation>(b =>
            {
                b.HasKey(x => x.RecommendationId);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne<Recipe>().WithMany().HasForeignKey(x => x.RecipeId).OnDelete(DeleteBehavior.Cascade);
                b.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            modelBuilder.Entity<ExpiryNotification>(b =>
            {
                b.HasKey(x => x.NotificationId);
                b.HasOne(x => x.UserIngredient).WithMany().HasForeignKey(x => x.UserIngredientId).OnDelete(DeleteBehavior.NoAction);
                b.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            modelBuilder.Entity<MealPlan>(b =>
            {
                b.HasKey(x => x.MealPlanId);
                b.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(x => x.Recipe).WithMany().HasForeignKey(x => x.RecipeId).OnDelete(DeleteBehavior.Restrict);
                b.Property(x => x.PlannedDate).IsRequired();
            });

            modelBuilder.Entity<ShoppingList>(b =>
            {
                b.HasKey(x => x.ShoppingListId);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
                b.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            modelBuilder.Entity<ShoppingListItem>(b =>
            {
                b.HasKey(x => x.ItemId);
                b.HasOne<ShoppingList>().WithMany().HasForeignKey(x => x.ShoppingListId).OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
