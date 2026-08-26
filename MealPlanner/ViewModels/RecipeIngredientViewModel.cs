using System.ComponentModel.DataAnnotations;

namespace MealPlanner.ViewModels
{
    public class RecipeIngredientViewModel
    {
        [Required(ErrorMessage = "Выберите ингредиент")]
        public int IngredientId { get; set; }

        [Required(ErrorMessage = "Укажите количество ингредиента")]
        [Range(1, 10000, ErrorMessage = "Количество должно быть от 1 до 10000")]
        public int Amount { get; set; }
    }
}
