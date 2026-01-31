namespace BepTroLy.Domain.Entities
{
    public class ShoppingListItem
    {
        public int ItemId { get; set; }
        public int ShoppingListId { get; set; }
        public string? IngredientName { get; set; }
        public double Quantity { get; set; }
        public string? Unit { get; set; }
        public bool IsChecked { get; set; }
    }
}
