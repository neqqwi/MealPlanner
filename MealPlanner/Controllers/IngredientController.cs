using MealPlanner.Data;
using MealPlanner.Models;
using MealPlanner.ViewModels.Ingredient;
using MealPlanner.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MealPlanner.Controllers
{
    public class IngredientController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<IngredientController> _logger;

        public IngredientController(AppDbContext context, ILogger<IngredientController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken, int page = 1, int pageSize = 25, string? searchTerm = null)
        {
            IQueryable<Ingredient> query = _context.Ingredients.OrderBy(i => i.Name);

            if (searchTerm != null) query = query.Where(i => i.Name.ToLower().Contains(searchTerm.ToLower()));

            var totalItems = await query.CountAsync(cancellationToken);

            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            if (page < 1) page = 1;

            if (page > totalPages && totalPages > 0) page = totalPages;

            int itemsToSkip = (page - 1) * pageSize;

            var pageResult = await query
                .Skip(itemsToSkip)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            PaginatedListViewModel<Ingredient> ingredientList = new PaginatedListViewModel<Ingredient>
            {
                Items = pageResult,
                CurrentPage = page,
                TotalPages = totalPages,
                SearchTerm = searchTerm
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
                var trimmedName = viewModel.Name.Trim();
                var normalizedName = char.ToUpper(trimmedName[0]) + trimmedName.Substring(1).ToLower();
                var existingIngredient = await _context.Ingredients
                    .FirstOrDefaultAsync(i => i.Name.ToLower() == normalizedName.ToLower(), cancellationToken);

                if (existingIngredient != null)
                {
                    if (existingIngredient.Unit == viewModel.Unit)
                    {
                        ModelState.AddModelError("",
                            $"Ингредиент \"{existingIngredient.Name}\" ({existingIngredient.Unit}) уже существует в базе данных");
                    }
                    else
                    {
                        ModelState.AddModelError("",
                            $"Ингредиент \"{existingIngredient.Name}\" уже существует в базе данных с единицей измерения \"{existingIngredient.Unit}\". Дублирование с другой единицей измерения не допускается.");
                    }
                    return View(viewModel);
                }

                var ingredient = new Ingredient
                {
                    Name = normalizedName,
                    Unit = viewModel.Unit
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
    }
}