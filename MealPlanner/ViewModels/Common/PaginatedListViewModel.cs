using MealPlanner.Models;

namespace MealPlanner.ViewModels.Common;

public class PaginatedListViewModel<T>
{
    public IReadOnlyList<T> Items { get; set; } = new List<T>();
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public string SearchTerm { get; set; } = string.Empty;
}