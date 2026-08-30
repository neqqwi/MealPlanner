using System.ComponentModel.DataAnnotations;

namespace MealPlanner.ViewModels.Ingredient
{
    public class IngredientEditViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите название ингредиента")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Название должно быть от 2 до 100 символов")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Выберите единицу измерения")]
        [RegularExpression("^(г|мл)$", ErrorMessage = "Недопустимая единица измерения")]
        public string Unit { get; set; } = string.Empty;
    }
}
