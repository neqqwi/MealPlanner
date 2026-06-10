using MealPlanner.Models;
using Microsoft.AspNetCore.Mvc;

namespace MealPlanner.Controllers
{
    public class RecipeController : Controller
    {
        public IActionResult Index()
        {
            List<Recipe> recipes = new List<Recipe>();

            return View(recipes);
        }
    }
}
