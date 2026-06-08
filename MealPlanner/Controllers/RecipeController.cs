using MealPlanner.Models;
using Microsoft.AspNetCore.Mvc;

namespace MealPlanner.Controllers
{
    public class RecipeController : Controller
    {
        public IActionResult Index()
        {
            List<RecipeViewModel> recipes = new List<RecipeViewModel>();

            recipes.Add (new RecipeViewModel(1, "Лимонно-розмариновый цыпленок",
            "Сочная курица, запеченная с чесноком, свежим розмарином и" +
            " дольками лимона.", 90, "", "/images/lemon-rosemary-chicken.jpg"));

            recipes.Add(new RecipeViewModel(2, "Шоколадно-вишневый фондан",
            "Французский десерт с хрустящей корочкой снаружи и горячей, " +
            "тягучей шоколадной начинкой, в центре которой прячется ягода " +
            "вишни.", 120, "", "/images/chocolate-cherry-fondant.jpg"));

            recipes.Add(new RecipeViewModel(3, "Крамбл с яблоками и корицей",
            "Ароматный десерт из запеченных с корицей яблок, покрытых " +
            "хрустящей крошкой из овсяных хлопьев, муки и сливочного масла.",
            40, "", "/images/apple-crumble-pie.jpg"));

            recipes.Add(new RecipeViewModel(4, "Тайский салат с говядиной",
            "Сочные кусочки быстро обжаренного мяса с салатом, огурцом и кинзой" +
            " под острой заправкой из чили и лайма", 25, "", "/images/thai-beef-salad.jpg"));

            return View(recipes);
        }
    }
}
