using MealPlanner.Data;
using MealPlanner.Models;
using MealPlanner.ViewModels.ShoppingList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MealPlanner.Controllers;

[Authorize]
public class ShoppingListController : BaseController
{
    private readonly AppDbContext _context;
    private readonly ILogger<ShoppingListController> _logger;

    public ShoppingListController(AppDbContext context, ILogger<ShoppingListController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        string currentUserId = GetCurrentUserId();

        try
        {
            DayOfWeek currentDayOfWeek = DateTime.Now.DayOfWeek;

            var daysFromToday = WeekOrder
                .SkipWhile(d => d != currentDayOfWeek)
                .ToList();

            var plans = await _context.WeeklyPlans
                .Where(wp => wp.UserId == currentUserId && daysFromToday.Contains(wp.DayOfWeek))
                .Include(wp => wp.Recipe)
                    .ThenInclude(r => r!.RecipeIngredients)
                        .ThenInclude(ri => ri.Ingredient)
                .ToListAsync(cancellationToken);

            var aggregatedIngredients = plans
                .Select(p => p.Recipe)
                .OfType<Recipe>()
                .SelectMany(r => r.RecipeIngredients)
                .GroupBy(ri => ri.IngredientId)
                .Select(g => new
                {
                    IngredientId = g.Key,
                    IngredientName = g.First().Ingredient.Name,
                    Unit = g.First().Ingredient.Unit,
                    TotalNeeded = g.Sum(ri => ri.Amount)
                })
                .ToList();

            var pantry = await _context.UserIngredients
                .Where(ui => ui.UserId == currentUserId)
                .ToDictionaryAsync(ui => ui.IngredientId, ui => ui.Quantity, cancellationToken);

            var shoppingList = new List<ShoppingListItemViewModel>();

            foreach (var item in aggregatedIngredients)
            {
                int needed = item.TotalNeeded;

                if (pantry.TryGetValue(item.IngredientId, out int inPantry))
                {
                    needed -= inPantry;
                }

                if (needed > 0)
                {
                    shoppingList.Add(new ShoppingListItemViewModel
                    {
                        IngredientName = item.IngredientName,
                        Unit = item.Unit,
                        AmountNeeded = needed
                    });
                }
            }

            shoppingList = shoppingList.OrderBy(x => x.IngredientName).ToList();

            ViewData["CurrentDayName"] = GetRussianDayName(currentDayOfWeek);

            return View(shoppingList);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Формирование списка покупок пользователя {UserId} было отменено", currentUserId);
            return new StatusCodeResult(499);
        }
    }

    private static string GetRussianDayName(DayOfWeek day) => day switch
    {
        DayOfWeek.Monday => "Понедельник",
        DayOfWeek.Tuesday => "Вторник",
        DayOfWeek.Wednesday => "Среда",
        DayOfWeek.Thursday => "Четверг",
        DayOfWeek.Friday => "Пятница",
        DayOfWeek.Saturday => "Суббота",
        DayOfWeek.Sunday => "Воскресенье",
        _ => day.ToString()
    };
    private static readonly DayOfWeek[] WeekOrder =
    {
        DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday,
        DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday
    };
}