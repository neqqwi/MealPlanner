namespace MealPlanner.ViewModels.ShoppingList;

public class ShoppingListItemViewModel
{
    public string IngredientName { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public int AmountNeeded { get; set; }
}
