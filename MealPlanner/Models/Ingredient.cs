using System.ComponentModel.DataAnnotations;

namespace MealPlanner.Models
{
    public class Ingredient
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите название ингредиента")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Название должно быть от 2 до 100 символов")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Выберите единицу измерения")]
        [RegularExpression("^(г|мл)$", ErrorMessage = "Недопустимая единица измерения")]
        public string Unit { get; set; } = string.Empty;

        public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
    }
}
