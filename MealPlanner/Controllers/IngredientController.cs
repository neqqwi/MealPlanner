using MealPlanner.Data;
using MealPlanner.Models;
using MealPlanner.ViewModels.Ingredients;
using MealPlanner.ViewModels.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Claims;

namespace MealPlanner.Controllers;

[Authorize]
public class IngredientController : BaseController
{
    private readonly AppDbContext _context;
    private readonly ILogger<IngredientController> _logger;

    public IngredientController(AppDbContext context, ILogger<IngredientController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 25, string? searchTerm = null, 
        CancellationToken cancellationToken = default)
    {
        string currentUserId = GetCurrentUserId();

        IQueryable<Ingredient> query = _context.Ingredients
            .Where(i => i.UserId == currentUserId)
            .OrderBy(i => i.Name);

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(i => EF.Functions.ILike(i.Name, $"%{searchTerm}%"));
        }

        var totalItems = await query.CountAsync(cancellationToken);
        int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        if (page < 1) page = 1;
        if (page > totalPages && totalPages > 0) page = totalPages;

        int itemsToSkip = (page - 1) * pageSize;

        var pageResult = await query
            .Skip(itemsToSkip)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var ingredientList = new PaginatedListViewModel<Ingredient>
        {
            Items = pageResult,
            CurrentPage = page,
            TotalPages = totalPages,
            SearchTerm = searchTerm ?? string.Empty
        };

        return View(ingredientList);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var viewModel = new IngredientCreateViewModel();
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(IngredientCreateViewModel viewModel, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(viewModel);

        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        try
        {
            string currentUserId = GetCurrentUserId();
            var normalizedName = NormalizeIngredientName(viewModel.Name);

            var existingIngredient = await _context.Ingredients
                .FirstOrDefaultAsync(i => EF.Functions.ILike(i.Name, normalizedName)
                                       && i.UserId == currentUserId, cancellationToken);

            if (existingIngredient != null)
            {
                ModelState.AddModelError("", $"Ингредиент \"{existingIngredient.Name}\"  уже существует в вашей базе данных" +
                    $"с единицей измерения \"{existingIngredient.Unit}\". У ингредиента может быть только одна единица измерения.");
                return View(viewModel);
            }

            var ingredient = new Ingredient
            {
                Name = normalizedName,
                Unit = viewModel.Unit,
                UserId = currentUserId
            };

            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Создание ингредиента '{IngredientName}' было отменено", viewModel.Name);
            return new StatusCodeResult(499);
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "Ошибка базы данных при добавлении ингредиента '{IngredientName}'", viewModel.Name);
            ModelState.AddModelError("", "Произошла ошибка базы данных при сохранении. Попробуйте еще раз.");
            return View(viewModel);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken = default)
    {
        string currentUserId = GetCurrentUserId();

        var ingredient = await _context.Ingredients
            .FirstOrDefaultAsync(i => i.Id == id && i.UserId == currentUserId, cancellationToken);

        if (ingredient == null) return NotFound();

        var ingredientEdit = new IngredientEditViewModel
        {
            Id = id,
            Name = ingredient.Name,
            Unit = ingredient.Unit
        };

        return View(ingredientEdit);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(IngredientEditViewModel viewModel, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(viewModel);
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        try
        {
            string currentUserId = GetCurrentUserId();
            var normalizedName = NormalizeIngredientName(viewModel.Name);

            var existingIngredient = await _context.Ingredients
                .FirstOrDefaultAsync(i => EF.Functions.ILike(i.Name, normalizedName)
                                       && i.Id != viewModel.Id
                                       && i.UserId == currentUserId, cancellationToken);

            if (existingIngredient != null)
            {
                ModelState.AddModelError("", $"Ингредиент \"{existingIngredient.Name}\"  уже существует в вашей базе данных" +
                    $"с единицей измерения \"{existingIngredient.Unit}\". У ингредиента может быть только одна единица измерения.");
                return View(viewModel);
            }

            var ingredientToEdit = await _context.Ingredients
                .FirstOrDefaultAsync(i => i.Id == viewModel.Id && i.UserId == currentUserId, cancellationToken);

            if (ingredientToEdit == null) return NotFound();

            ingredientToEdit.Name = normalizedName;
            ingredientToEdit.Unit = viewModel.Unit;

            await _context.SaveChangesAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Редактирование ингредиента '{IngredientName}' было отменено", viewModel.Name);
            return new StatusCodeResult(499);
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "Ошибка базы данных при редактировании ингредиента '{IngredientName}'", viewModel.Name);
            ModelState.AddModelError("", "Произошла ошибка базы данных при сохранении. Попробуйте еще раз.");
            return View(viewModel);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
    {
        string currentUserId = GetCurrentUserId();

        var ingredient = await _context.Ingredients
            .FirstOrDefaultAsync(i => i.Id == id && i.UserId == currentUserId, cancellationToken);

        if (ingredient == null) return NotFound();

        var recipeUsageQuery = _context.RecipeIngredients
            .Where(ri => ri.IngredientId == id);

        var recipesCount = await recipeUsageQuery.CountAsync(cancellationToken);

        var viewModel = new IngredientDeleteViewModel
        {
            Id = ingredient.Id,
            Name = ingredient.Name,
            Unit = ingredient.Unit,
            RecipesCount = recipesCount,
            ExampleRecipes = await recipeUsageQuery
                .Take(3)
                .Select(ri => new RecipeReference
                {
                    RecipeId = ri.RecipeId,
                    RecipeName = ri.Recipe.Name
                })
                .ToListAsync(cancellationToken)
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        string currentUserId = GetCurrentUserId();

        var ingredient = await _context.Ingredients
            .FirstOrDefaultAsync(i => i.Id == id && i.UserId == currentUserId, cancellationToken);

        if (ingredient == null) return NotFound();

        var recipeUsageQuery = _context.RecipeIngredients
            .Where(ri => ri.IngredientId == id);

        var recipesCount = await recipeUsageQuery.CountAsync(cancellationToken);

        if (recipesCount > 0)
        {
            TempData["ErrorMessage"] = $"Нельзя удалить ингредиент \"{ingredient.Name} " +
                $"({ingredient.Unit})\", так как он используется в {recipesCount} рецептах.";
            return RedirectToAction(nameof(Delete), new { id = id });
        }

        _context.Ingredients.Remove(ingredient);
        await _context.SaveChangesAsync(cancellationToken);

        TempData["SuccessMessage"] = $"Ингредиент \"{ingredient.Name} ({ingredient.Unit})\" успешно удален.";
        return RedirectToAction(nameof(Index));
    }
    private static string NormalizeIngredientName(string name)
    {
        var trimmed = name.Trim();

        // Используется для форматирования отображаемого имени, а не для сравнения или идентификации
        #pragma warning disable CA1308
        return char.ToUpper(trimmed[0], CultureInfo.InvariantCulture) + trimmed.Substring(1).ToLowerInvariant();
        #pragma warning restore CA1308
    }
}