using System.ComponentModel.DataAnnotations;

namespace MealPlanner.Models
{
    public class RecipeIngredient
    {
        public int RecipeId { get; set; }
        public int IngredientId { get; set; }

        [Required(ErrorMessage = "Укажите количество ингредиента")]
        [Range(1, 10000, ErrorMessage = "Количество должно быть от 1 до 10000")]
        public int Amount { get; set; }

        public Recipe Recipe { get; set; } = null!;
        public Ingredient Ingredient { get; set; } = null!;
    }
}
