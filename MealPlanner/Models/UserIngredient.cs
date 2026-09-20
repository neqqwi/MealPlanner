namespace MealPlanner.Models
{
    public class UserIngredient
    {
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;

        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; } = null!;

        public int Quantity { get; set; }
    }
}