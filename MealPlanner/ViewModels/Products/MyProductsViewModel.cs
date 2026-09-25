using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MealPlanner.ViewModels.Products;

public class MyProductsViewModel
{
    public IList<ProductQuantityViewModel> Products { get; set; } = new List<ProductQuantityViewModel>();
    public IList<SelectListItem> PossibleIngredients { get; set; } = new List<SelectListItem>();
}

public class ProductQuantityViewModel
{
    [Required(ErrorMessage = "Выберите продукт")]
    public int IngredientId { get; set; }

    [Required(ErrorMessage = "Укажите количество")]
    [Range(0, 100000, ErrorMessage = "Количество не может быть отрицательным")]
    public int Quantity { get; set; } = 1;
}