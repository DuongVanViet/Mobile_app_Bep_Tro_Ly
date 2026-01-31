namespace BepTroLy.Application.DTOs
{
    public class IngredientDto
    {
        public int IngredientId { get; set; }
        public string? IngredientName { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }

    public class CreateIngredientRequest
    {
        public string? IngredientName { get; set; }
        public int? CategoryId { get; set; }
    }

    public class UpdateIngredientRequest
    {
        public string? IngredientName { get; set; }
        public int? CategoryId { get; set; }
    }

    public class UserIngredientDto
    {
        public int UserIngredientId { get; set; }
        public int UserId { get; set; }
        public int IngredientId { get; set; }
        public string? IngredientName { get; set; }
        public decimal Quantity { get; set; }
        public int? UnitId { get; set; }
        public string? UnitName { get; set; }
        public System.DateTime? ExpiryDate { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class AddUserIngredientRequest
    {
        public int IngredientId { get; set; }
        public decimal Quantity { get; set; }
        public int? UnitId { get; set; }
        public System.DateTime? ExpiryDate { get; set; }
    }

    public class UpdateUserIngredientRequest
    {
        public decimal Quantity { get; set; }
        public int? UnitId { get; set; }
        public System.DateTime? ExpiryDate { get; set; }
    }

    public class IngredientCategoryDto
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }

    public class CreateIngredientCategoryRequest
    {
        public string? CategoryName { get; set; }
    }
}
