using MealPlanner.Data;
using MealPlanner.Models;
using MealPlanner.ViewModels;
using MealPlanner.ViewModels.Recipe;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace MealPlanner.Controllers
{
    public class RecipeController : Controller
    {
        private const int MaxFileSize = 5 * 1024 * 1024; // 5MB
        private const int ImageSize = 640;
        private const string DefaultNoImagePath = "/images/no-image.svg";
        private static readonly string[] AllowedImageExtensions = [".jpg", ".jpeg", ".png"];

        private readonly AppDbContext _context;
        private readonly ILogger<RecipeController> _logger;

        public RecipeController(AppDbContext context, ILogger<RecipeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var recipes = await _context.Recipes.ToListAsync(cancellationToken);
            return View(recipes);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var viewModel = new RecipeCreateViewModel
            {
                PossibleRecipeIngredients = await GetIngredientsListAsync(cancellationToken)
            };

            viewModel.RecipeIngredients.Add(new RecipeIngredientViewModel());

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RecipeCreateViewModel viewModel, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return await ReturnViewWithIngredients(viewModel, cancellationToken);
            }

            string imagePath = DefaultNoImagePath;

            try
            {
                if (viewModel.ImageFile != null && viewModel.ImageFile.Length > 0)
                {
                    var ext = Path.GetExtension(viewModel.ImageFile.FileName).ToLowerInvariant();

                    if (!AllowedImageExtensions.Contains(ext))
                    {
                        ModelState.AddModelError("ImageFile", "Можно загружать только JPG или PNG файлы");
                        return await ReturnViewWithIngredients(viewModel, cancellationToken);
                    }

                    if (viewModel.ImageFile.Length > MaxFileSize)
                    {
                        ModelState.AddModelError("ImageFile", "Размер файла не должен превышать 5MB");
                        return await ReturnViewWithIngredients(viewModel, cancellationToken);
                    }

                    if (!viewModel.ImageFile.ContentType.StartsWith("image/"))
                    {
                        ModelState.AddModelError("ImageFile", "Файл должен быть изображением");
                        return await ReturnViewWithIngredients(viewModel, cancellationToken);
                    }

                    var newFileName = Guid.NewGuid().ToString() + ext;
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "recipes");
                    Directory.CreateDirectory(uploadsFolder);
                    var filePath = Path.Combine(uploadsFolder, newFileName);

                    using (var image = Image.Load(viewModel.ImageFile.OpenReadStream()))
                    {
                        image.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Size = new Size(ImageSize, ImageSize),
                            Mode = ResizeMode.Crop,
                            Position = AnchorPositionMode.Center,
                            Sampler = KnownResamplers.Lanczos3
                        }));

                        image.Save(filePath);
                    }

                    imagePath = "/uploads/recipes/" + newFileName;
                }

                var recipe = new Recipe
                {
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    CookingTime = viewModel.CookingTime,
                    Instructions = viewModel.Instructions?.Trim(),
                    ImagePath = imagePath,
                    RecipeIngredients = []
                };

                var addedIngredients = new HashSet<int>();

                foreach (var ingredientVm in viewModel.RecipeIngredients)
                {
                    if (ingredientVm.IngredientId > 0 && ingredientVm.Amount > 0)
                    {
                        if (!addedIngredients.Add(ingredientVm.IngredientId))
                        {
                            ModelState.AddModelError("", "Один из ингредиентов добавлен в рецепт дважды");
                            return await ReturnViewWithIngredients(viewModel, cancellationToken);
                        }

                        recipe.RecipeIngredients.Add(new RecipeIngredient
                        {
                            IngredientId = ingredientVm.IngredientId,
                            Amount = ingredientVm.Amount
                        });
                    }
                }

                if (!viewModel.RecipeIngredients.Any(i => i.IngredientId > 0 && i.Amount > 0))
                {
                    ModelState.AddModelError("", "Добавьте хотя бы один ингредиент");
                    return await ReturnViewWithIngredients(viewModel, cancellationToken);
                }

                _context.Recipes.Add(recipe);
                await _context.SaveChangesAsync(cancellationToken);

                return RedirectToAction(nameof(Index));
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Создание рецепта '{RecipeName}' было отменено", viewModel.Name);
                return new StatusCodeResult(499);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании рецепта '{RecipeName}'", viewModel.Name);
                ModelState.AddModelError("", "Произошла ошибка при сохранении рецепта. Попробуйте еще раз.");
                return await ReturnViewWithIngredients(viewModel, cancellationToken);
            }
        }

        private async Task<List<SelectListItem>> GetIngredientsListAsync(CancellationToken cancellationToken)
        {
            return await _context.Ingredients
                .OrderBy(i => i.Name)
                .Select(i => new SelectListItem
                {
                    Value = i.Id.ToString(),
                    Text = $"{i.Name} ({i.Unit})"
                })
                .ToListAsync(cancellationToken);
        }

        private async Task<IActionResult> ReturnViewWithIngredients(RecipeCreateViewModel viewModel, CancellationToken cancellationToken)
        {
            viewModel.PossibleRecipeIngredients = await GetIngredientsListAsync(cancellationToken);
            return View("Create", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
        {
            var recipe = await _context.Recipes
                .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var recipe = await _context.Recipes
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

            if (recipe == null) return NotFound();

            var allIngredients = await _context.Ingredients
                .OrderBy(i => i.Name)
                .Select(i => new SelectListItem
                {
                    Value = i.Id.ToString(),
                    Text = $"{i.Name} ({i.Unit})"
                })
                .ToListAsync(cancellationToken);

            var viewModel = new RecipeEditViewModel
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Description = recipe.Description,
                CookingTime = recipe.CookingTime,
                Instructions = recipe.Instructions,
                CurrentImagePath = recipe.ImagePath,
                PossibleRecipeIngredients = allIngredients,
                RecipeIngredients = recipe.RecipeIngredients.Select(ri => new RecipeIngredientViewModel
                {
                    IngredientId = ri.IngredientId,
                    Amount = ri.Amount
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RecipeEditViewModel viewModel, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return await ReturnViewWithIngredients(viewModel, cancellationToken);
            }

            try
            {
                var recipe = await _context.Recipes
                    .Include(r => r.RecipeIngredients)
                    .FirstOrDefaultAsync(r => r.Id == viewModel.Id, cancellationToken);

                if (recipe == null) return NotFound();

                string oldImagePath = recipe.ImagePath;
                string newImagePath = viewModel.CurrentImagePath ?? DefaultNoImagePath;

                if (viewModel.ImageFile != null && viewModel.ImageFile.Length > 0)
                {
                    var ext = Path.GetExtension(viewModel.ImageFile.FileName).ToLowerInvariant();

                    if (!AllowedImageExtensions.Contains(ext))
                    {
                        ModelState.AddModelError("ImageFile", "Можно загружать только JPG или PNG файлы");
                        return await ReturnViewWithIngredients(viewModel, cancellationToken);
                    }

                    if (viewModel.ImageFile.Length > MaxFileSize)
                    {
                        ModelState.AddModelError("ImageFile", "Размер файла не должен превышать 5MB");
                        return await ReturnViewWithIngredients(viewModel, cancellationToken);
                    }

                    if (!viewModel.ImageFile.ContentType.StartsWith("image/"))
                    {
                        ModelState.AddModelError("ImageFile", "Файл должен быть изображением");
                        return await ReturnViewWithIngredients(viewModel, cancellationToken);
                    }

                    var newFileName = Guid.NewGuid().ToString() + ext;
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "recipes");
                    Directory.CreateDirectory(uploadsFolder);
                    var filePath = Path.Combine(uploadsFolder, newFileName);

                    using (var image = Image.Load(viewModel.ImageFile.OpenReadStream()))
                    {
                        image.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Size = new Size(ImageSize, ImageSize),
                            Mode = ResizeMode.Crop,
                            Position = AnchorPositionMode.Center,
                            Sampler = KnownResamplers.Lanczos3
                        }));

                        image.Save(filePath);
                    }

                    newImagePath = "/uploads/recipes/" + newFileName;
                }

                recipe.Name = viewModel.Name;
                recipe.Description = viewModel.Description;
                recipe.CookingTime = viewModel.CookingTime;
                recipe.Instructions = viewModel.Instructions?.Trim();
                recipe.ImagePath = newImagePath;

                recipe.RecipeIngredients.Clear();
                var addedIngredients = new HashSet<int>();

                foreach (var ingredientVm in viewModel.RecipeIngredients)
                {
                    if (ingredientVm.IngredientId > 0 && ingredientVm.Amount > 0)
                    {
                        if (!addedIngredients.Add(ingredientVm.IngredientId))
                        {
                            ModelState.AddModelError("", "Один из ингредиентов добавлен в рецепт дважды");
                            return await ReturnViewWithIngredients(viewModel, cancellationToken);
                        }

                        recipe.RecipeIngredients.Add(new RecipeIngredient
                        {
                            IngredientId = ingredientVm.IngredientId,
                            Amount = ingredientVm.Amount
                        });
                    }
                }

                if (!viewModel.RecipeIngredients.Any(i => i.IngredientId > 0 && i.Amount > 0))
                {
                    ModelState.AddModelError("", "Добавьте хотя бы один ингредиент");
                    return await ReturnViewWithIngredients(viewModel, cancellationToken);
                }

                await _context.SaveChangesAsync(cancellationToken);

                if (!string.IsNullOrEmpty(oldImagePath) &&
                    oldImagePath != DefaultNoImagePath &&
                    oldImagePath != newImagePath)
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldImagePath.TrimStart('/'));

                    try
                    {
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }
                    catch (IOException ex)
                    {
                        _logger.LogWarning(ex, "Не удалось удалить старый файл изображения: {FilePath}", oldFilePath);
                    }
                }

                return RedirectToAction(nameof(Details), new { id = recipe.Id });
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Редактирование рецепта '{RecipeName}' было отменено", viewModel.Name);
                return new StatusCodeResult(499);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при редактировании рецепта '{RecipeName}'", viewModel.Name);
                ModelState.AddModelError("", "Произошла ошибка при сохранении изменений. Попробуйте еще раз.");
                return await ReturnViewWithIngredients(viewModel, cancellationToken);
            }
        }

        private async Task<IActionResult> ReturnViewWithIngredients(RecipeEditViewModel viewModel, CancellationToken cancellationToken)
        {
            viewModel.PossibleRecipeIngredients = await _context.Ingredients
                .OrderBy(i => i.Name)
                .Select(i => new SelectListItem
                {
                    Value = i.Id.ToString(),
                    Text = $"{i.Name} ({i.Unit})"
                })
                .ToListAsync(cancellationToken);

            return View("Edit", viewModel);
        }
    }
}