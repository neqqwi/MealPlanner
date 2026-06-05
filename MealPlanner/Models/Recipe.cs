namespace MealPlanner.Models
{
    public class Recipe
    {
        public int RecipeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CookingTime { get; set; } // в минутах
        public string Instructions { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public Recipe()
        {
            Name = string.Empty;
            Description = string.Empty;
            Instructions = string.Empty;
        }
        public Recipe(int id, string name, string description, int time, string instructions, string url)
        {
            RecipeId = id;
            Name = name;
            Description = description;
            CookingTime = time;
            Instructions = instructions;
            ImageUrl = url;
        }
        public Recipe(int id, string name, string description, int time, string instructions)
        {
            RecipeId = id;
            Name = name;
            Description = description;
            CookingTime = time;
            Instructions = instructions;
        }
    }
}
