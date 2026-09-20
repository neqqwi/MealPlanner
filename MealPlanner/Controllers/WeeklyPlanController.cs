using MealPlanner.Data;
using MealPlanner.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace MealPlanner.Controllers;

[Authorize]
public class WeeklyPlanController : BaseController
{
    private readonly AppDbContext _context;
    private readonly ILogger<WeeklyPlanController> _logger;

    public WeeklyPlanController(AppDbContext context, ILogger<WeeklyPlanController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        string currentUserId = GetCurrentUserId();

        var plans = await _context.WeeklyPlans
            .Where(wp => wp.UserId == currentUserId)
            .Include(wp => wp.Recipe)
            .OrderBy(wp => wp.DayOfWeek)
            .ThenBy(wp => wp.Id)
            .ToListAsync(cancellationToken);

        var groupedPlans = plans
            .GroupBy(wp => wp.DayOfWeek)
            .ToDictionary(g => g.Key, g => g.ToList());

        return View(groupedPlans);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int recipeId, string selectedDays, CancellationToken cancellationToken)
    {
        string currentUserId = GetCurrentUserId();

        var recipe = await _context.Recipes
            .FirstOrDefaultAsync(r => r.Id == recipeId && r.UserId == currentUserId, cancellationToken);

        if (recipe == null)
        {
            _logger.LogWarning("Попытка добавить несуществующий или чужой рецепт {RecipeId} в план пользователем {UserId}",
                recipeId, currentUserId);
            return NotFound();
        }

        if (!string.IsNullOrEmpty(selectedDays))
        {
            var dayValues = selectedDays.Split(',', StringSplitOptions.RemoveEmptyEntries);

            foreach (var dayValue in dayValues)
            {
                if (int.TryParse(dayValue, out int dayInt))
                {
                    var dayOfWeek = (DayOfWeek)dayInt;

                    var exists = await _context.WeeklyPlans
                        .AnyAsync(wp => wp.RecipeId == recipeId
                                     && wp.DayOfWeek == dayOfWeek
                                     && wp.UserId == currentUserId, cancellationToken);

                    if (!exists)
                    {
                        _context.WeeklyPlans.Add(new WeeklyPlan
                        {
                            RecipeId = recipeId,
                            DayOfWeek = dayOfWeek,
                            UserId = currentUserId
                        });
                    }
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Пользователь {UserId} добавил рецепт {RecipeId} в план на {DaysCount} дней",
                currentUserId, recipeId, dayValues.Length);
            }
        }

        return RedirectToAction("Index", "Recipe");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        string currentUserId = GetCurrentUserId();

        var plan = await _context.WeeklyPlans
            .FirstOrDefaultAsync(wp => wp.Id == id && wp.UserId == currentUserId, cancellationToken);

        if (plan == null)
        {
            _logger.LogWarning("Попытка удалить несуществующий или чужой план с ID {PlanId} пользователем {UserId}",
                id, currentUserId);
            return NotFound();
        }

        _context.WeeklyPlans.Remove(plan);
        await _context.SaveChangesAsync(cancellationToken);

        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("Пользователь {UserId} удалил рецепт из плана на день {DayOfWeek}",
            currentUserId, plan.DayOfWeek);
        }

        return RedirectToAction(nameof(Index));
    }
}