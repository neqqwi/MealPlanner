using MealPlanner.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MealPlanner.Controllers;

public abstract class BaseController : Controller
{
    protected string GetCurrentUserId()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? AppConstants.DemoUserId;
        }

        return AppConstants.DemoUserId;
    }
}
