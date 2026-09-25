using MealPlanner.Data;
using MealPlanner.Models;
using MealPlanner.ViewModels.Accounts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MealPlanner.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly AppDbContext _context;
    private readonly ILogger<AccountController> _logger;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
        AppDbContext context, ILogger<AccountController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel viewModel)
    {
        ArgumentNullException.ThrowIfNull(viewModel);

        if (!ModelState.IsValid) return View(viewModel);

        var user = await _userManager.FindByEmailAsync(viewModel.Email);
        if (user == null)
        {
            ModelState.AddModelError("", "Неверный email или пароль");
            return View(viewModel);
        }

        var result = await _signInManager
            .PasswordSignInAsync(user, viewModel.Password, viewModel.RememberMe, lockoutOnFailure: true);

        if (result.IsLockedOut)
        {
            ModelState.AddModelError("", "Аккаунт временно заблокирован из-за большого количества неудачных " +
                "попыток входа. Попробуйте позже.");
            return View(viewModel);
        }

        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Неверный email или пароль");
            return View(viewModel);
        }

        return RedirectToAction("Index", "Recipe");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel viewModel)
    {
        ArgumentNullException.ThrowIfNull(viewModel);

        if (!ModelState.IsValid) return View(viewModel);

        var user = new ApplicationUser
        {
            UserName = viewModel.Email,
            Email = viewModel.Email
        };

        var result = await _userManager.CreateAsync(user, viewModel.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(viewModel);
        }

        await CopySeedDataForUser(user.Id);

        await _signInManager.SignInAsync(user, isPersistent: false);
        return RedirectToAction("Index", "Recipe");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Recipe");
    }

    private async Task CopySeedDataForUser(string newUserId)
    {
        var demoIngredients = await _context.Ingredients
            .Where(i => i.UserId == AppConstants.DemoUserId)
            .ToListAsync();

        var newIngredients = new List<Ingredient>();
        var ingredientIdMap = new Dictionary<int, int>();

        foreach (var demoIngredient in demoIngredients)
        {
            var newIngredient = new Ingredient
            {
                Name = demoIngredient.Name,
                Unit = demoIngredient.Unit,
                UserId = newUserId
            };
            newIngredients.Add(newIngredient);
        }

        _context.Ingredients.AddRange(newIngredients);
        await _context.SaveChangesAsync();

        for (int i = 0; i < demoIngredients.Count; i++)
        {
            ingredientIdMap[demoIngredients[i].Id] = newIngredients[i].Id;
        }

        var demoRecipes = await _context.Recipes
            .Include(r => r.RecipeIngredients)
            .Where(r => r.UserId == AppConstants.DemoUserId)
            .ToListAsync();

        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "recipes");

        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        foreach (var demoRecipe in demoRecipes)
        {
            string? newImagePath = null;

            if (!string.IsNullOrEmpty(demoRecipe.ImagePath))
            {
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", demoRecipe.ImagePath.TrimStart('/'));

                if (System.IO.File.Exists(oldFilePath))
                {
                    var extension = Path.GetExtension(demoRecipe.ImagePath);
                    var newFileName = $"user_{newUserId}_{Guid.NewGuid()}{extension}";
                    var newFilePath = Path.Combine(uploadsFolder, newFileName);

                    try
                    {
                        System.IO.File.Copy(oldFilePath, newFilePath);
                        newImagePath = "/uploads/recipes/" + newFileName;
                    }
                    catch (IOException ex)
                    {
                        _logger.LogWarning(ex, "Не удалось скопировать демо-изображение рецепта для нового " +
                            "пользователя: {FilePath}", oldFilePath);
                    }
                }
            }

            var newRecipe = new Recipe
            {
                Name = demoRecipe.Name,
                Description = demoRecipe.Description,
                CookingTime = demoRecipe.CookingTime,
                Instructions = demoRecipe.Instructions,
                ImagePath = newImagePath,
                UserId = newUserId
            };

            foreach (var demoRecipeIngredient in demoRecipe.RecipeIngredients)
            {
                if (ingredientIdMap.TryGetValue(demoRecipeIngredient.IngredientId, out int value))
                {
                    newRecipe.RecipeIngredients.Add(new RecipeIngredient
                    {
                        IngredientId = value,
                        Amount = demoRecipeIngredient.Amount
                    });
                }
            }

            _context.Recipes.Add(newRecipe);
        }

        await _context.SaveChangesAsync();
    }
}