using MealPlanner.Data;
using MealPlanner.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
}