using Microsoft.AspNetCore.Identity;

namespace MealPlanner.Models;

public class ApplicationUser : IdentityUser
{
    public virtual ICollection<Recipe> Recipes { get; } = new List<Recipe>();
    public virtual ICollection<Ingredient> Ingredients { get; } = new List<Ingredient>();
    public ICollection<UserIngredient> UserIngredients { get; } = new List<UserIngredient>();
}