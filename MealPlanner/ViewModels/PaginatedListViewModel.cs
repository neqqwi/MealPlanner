using MealPlanner.Models;

namespace MealPlanner.ViewModels
{
    public class PaginatedListViewModel<T>
    {
        public List<T> Items { get; set; } = [];
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string SearchTerm { get; set; }
    }
}