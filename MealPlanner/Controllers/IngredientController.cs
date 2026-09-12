using MealPlanner.Data;
using MealPlanner.Models;
using MealPlanner.ViewModels.Ingredient;
using MealPlanner.ViewModels.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MealPlanner.Controllers
{
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

        [AllowAnonymous]
        public async Task<IActionResult> Index(CancellationToken cancellationToken, int page = 1, int pageSize = 25, 
            string? searchTerm = null)
        {
            string currentUserId = GetCurrentUserId();

            IQueryable<Ingredient> query = _context.Ingredients
                .Where(i => i.UserId == currentUserId || i.UserId == AppConstants.DemoUserId)
                .OrderBy(i => i.Name);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(i => i.Name.ToLower().Contains(searchTerm.ToLower()));
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
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                string currentUserId = GetCurrentUserId();
                var trimmedName = viewModel.Name.Trim();
                var normalizedName = char.ToUpper(trimmedName[0]) + trimmedName.Substring(1).ToLower();
                var existingIngredient = await _context.Ingredients
                    .FirstOrDefaultAsync(i => i.Name.ToLower() == normalizedName.ToLower()
                                           && i.Unit == viewModel.Unit
                                           && i.UserId == currentUserId, cancellationToken);

                if (existingIngredient != null)
                {
                    ModelState.AddModelError("", $"Ингредиент \"{existingIngredient.Name}\" " +
                        $"({existingIngredient.Unit}) уже существует в вашей базе данных");
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при добавлении ингредиента '{IngredientName}'", viewModel.Name);
                ModelState.AddModelError("", "Произошла ошибка при сохранении ингредиента. Попробуйте еще раз.");
                return View(viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(CancellationToken cancellationToken, int id)
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
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                string currentUserId = GetCurrentUserId();
                var trimmedName = viewModel.Name.Trim();
                var normalizedName = char.ToUpper(trimmedName[0]) + trimmedName.Substring(1).ToLower();
                var existingIngredient = await _context.Ingredients
                    .FirstOrDefaultAsync(i => i.Name.ToLower() == normalizedName.ToLower()
                                           && i.Unit == viewModel.Unit
                                           && i.Id != viewModel.Id
                                           && i.UserId == currentUserId, cancellationToken);

                if (existingIngredient != null)
                {
                    ModelState.AddModelError("", $"Ингредиент \"{existingIngredient.Name}\" " +
                        $"({existingIngredient.Unit}) уже существует в вашей базе данных");
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при изменении ингредиента '{IngredientName}'", viewModel.Name);
                ModelState.AddModelError("", "Произошла ошибка при сохранении изменений. Попробуйте еще раз.");
                return View(viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(CancellationToken cancellationToken, int id)
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
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            try
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
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Удаление ингредиента с ID {IngredientId} было отменено", id);
                return new StatusCodeResult(499);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении ингредиента с ID {IngredientId}", id);
                TempData["ErrorMessage"] = "Произошла ошибка при удалении ингредиента. Попробуйте еще раз.";
                return RedirectToAction(nameof(Delete), new { id = id });
            }
        }
    }
}