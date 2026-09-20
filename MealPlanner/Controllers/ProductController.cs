using System.Globalization;
using MealPlanner.Data;
using MealPlanner.Models;
using MealPlanner.ViewModels.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MealPlanner.Controllers
{
    [Authorize]
    public class ProductController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductController> _logger;

        public ProductController(AppDbContext context, ILogger<ProductController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
        {
            string currentUserId = GetCurrentUserId();

            var viewModel = new MyProductsViewModel
            {
                PossibleIngredients = await GetPossibleIngredientsAsync(currentUserId, cancellationToken),
                Products = await _context.UserIngredients
                    .Where(ui => ui.UserId == currentUserId && ui.Quantity > 0)
                    .Include(ui => ui.Ingredient)
                    .OrderBy(ui => ui.Ingredient.Name)
                    .Select(ui => new ProductQuantityViewModel
                    {
                        IngredientId = ui.IngredientId,
                        Quantity = ui.Quantity
                    })
                    .ToListAsync(cancellationToken)
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(MyProductsViewModel viewModel, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(viewModel);

            if (!ModelState.IsValid)
            {
                return await ReturnViewWithPossibleIngredients(viewModel, cancellationToken);
            }

            try
            {
                string currentUserId = GetCurrentUserId();

                var validIngredientIds = await _context.Ingredients
                    .Where(i => i.UserId == currentUserId)
                    .Select(i => i.Id)
                    .ToHashSetAsync(cancellationToken);

                var existing = await _context.UserIngredients
                    .Where(ui => ui.UserId == currentUserId)
                    .ToDictionaryAsync(ui => ui.IngredientId, cancellationToken);

                var submittedIds = new HashSet<int>();

                foreach (var product in viewModel.Products)
                {
                    if (!validIngredientIds.Contains(product.IngredientId))
                    {
                        continue;
                    }

                    if (!submittedIds.Add(product.IngredientId))
                    {
                        ModelState.AddModelError("", "Один продукт указан в списке дважды");
                        return await ReturnViewWithPossibleIngredients(viewModel, cancellationToken);
                    }

                    if (existing.TryGetValue(product.IngredientId, out var userIngredient))
                    {
                        if (product.Quantity > 0)
                        {
                            userIngredient.Quantity = product.Quantity;
                        }
                        else
                        {
                            _context.UserIngredients.Remove(userIngredient);
                        }
                    }
                    else if (product.Quantity > 0)
                    {
                        _context.UserIngredients.Add(new UserIngredient
                        {
                            UserId = currentUserId,
                            IngredientId = product.IngredientId,
                            Quantity = product.Quantity
                        });
                    }
                }

                foreach (var kvp in existing)
                {
                    if (!submittedIds.Contains(kvp.Key))
                    {
                        _context.UserIngredients.Remove(kvp.Value);
                    }
                }

                await _context.SaveChangesAsync(cancellationToken);

                return RedirectToAction(nameof(Index));
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Обновление списка продуктов было отменено");
                return new StatusCodeResult(499);
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Ошибка базы данных при обновлении списка продуктов");
                ModelState.AddModelError("", "Произошла ошибка базы данных при сохранении. Попробуйте еще раз.");
                return await ReturnViewWithPossibleIngredients(viewModel, cancellationToken);
            }
        }

        private async Task<List<SelectListItem>> GetPossibleIngredientsAsync(string currentUserId, CancellationToken cancellationToken)
        {
            return await _context.Ingredients
                .Where(i => i.UserId == currentUserId)
                .OrderBy(i => i.Name)
                .Select(i => new SelectListItem
                {
                    Value = i.Id.ToString(CultureInfo.InvariantCulture),
                    Text = $"{i.Name} ({i.Unit})"
                })
                .ToListAsync(cancellationToken);
        }

        private async Task<IActionResult> ReturnViewWithPossibleIngredients(MyProductsViewModel viewModel,
            CancellationToken cancellationToken)
        {
            viewModel.PossibleIngredients = await GetPossibleIngredientsAsync(GetCurrentUserId(), cancellationToken);
            return View("Index", viewModel);
        }
    }
}