using System.ComponentModel.DataAnnotations;

namespace MealPlanner.Models
{
    public class WeeklyPlan
    {
        public int Id { get; set; }

        [Required]
        public DayOfWeek DayOfWeek { get; set; }

        [Required]
        public int RecipeId { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        public Recipe? Recipe { get; set; }
        public ApplicationUser? User { get; set; }
    }
}