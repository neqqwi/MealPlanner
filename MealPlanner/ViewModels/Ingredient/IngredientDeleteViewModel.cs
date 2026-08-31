namespace MealPlanner.ViewModels.Ingredient
{
    public class IngredientDeleteViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public int RecipesCount { get; set; }
        public List<RecipeReference> ExampleRecipes { get; set; } = [];
    }
    public class RecipeReference
    {
        public int RecipeId { get; set; }
        public string RecipeName { get; set; } = string.Empty;
    }
}
