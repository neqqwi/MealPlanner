using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MealPlanner.ViewModels
{
    public class RecipeCreateViewModel
    {
        [Required(ErrorMessage = "Введите название рецепта")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Название должно быть от 2 до 150 символов")]
        public string Name { get; set; } = string.Empty;

        [DataType(DataType.MultilineText)]
        [StringLength(2000, ErrorMessage = "Описание не должно превышать 2000 символов")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Укажите время приготовления")]
        [Range(1, 1440, ErrorMessage = "Время приготовления должно быть от 1 до 1440 минут (24 часа)")]
        public int CookingTime { get; set; } // в минутах

        [DataType(DataType.MultilineText)]
        [StringLength(10000, ErrorMessage = "Инструкции не должны превышать 10000 символов")]
        public string? Instructions { get; set; }
        public IFormFile? ImageFile { get; set; }

        public List<RecipeIngredientViewModel> RecipeIngredients { get; set; } = [];

        public List<SelectListItem> PossibleRecipeIngredients { get; set; } = [];
    }
}
