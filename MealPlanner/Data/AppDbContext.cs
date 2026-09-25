using MealPlanner.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MealPlanner.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Recipe> Recipes { get; set; } = null!;
    public DbSet<Ingredient> Ingredients { get; set; } = null!;
    public DbSet<RecipeIngredient> RecipeIngredients { get; set; } = null!;
    public DbSet<UserIngredient> UserIngredients { get; set; } = null!;
    public DbSet<WeeklyPlan> WeeklyPlans { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        base.OnModelCreating(builder);

        builder.Entity<RecipeIngredient>()
            .HasKey(ri => new { ri.RecipeId, ri.IngredientId });

        builder.Entity<RecipeIngredient>()
            .HasOne(ri => ri.Recipe)
            .WithMany(r => r.RecipeIngredients)
            .HasForeignKey(ri => ri.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<RecipeIngredient>()
            .HasOne(ri => ri.Ingredient)
            .WithMany(i => i.RecipeIngredients)
            .HasForeignKey(ri => ri.IngredientId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Recipe>()
            .HasIndex(r => r.Name);

        builder.Entity<Recipe>()
            .HasIndex(r => r.UserId);

        builder.Entity<Ingredient>()
            .HasIndex(i => new { i.Name, i.UserId })
            .IsUnique();

        builder.Entity<Ingredient>()
            .HasIndex(i => i.UserId);

        builder.Entity<Recipe>()
            .HasOne(r => r.User)
            .WithMany(u => u.Recipes)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Ingredient>()
            .HasOne(i => i.User)
            .WithMany(u => u.Ingredients)
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<UserIngredient>()
            .HasKey(ui => new { ui.UserId, ui.IngredientId });

        builder.Entity<UserIngredient>()
            .HasOne(ui => ui.User)
            .WithMany(u => u.UserIngredients)
            .HasForeignKey(ui => ui.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<UserIngredient>()
            .HasOne(ui => ui.Ingredient)
            .WithMany(i => i.UserIngredients)
            .HasForeignKey(ui => ui.IngredientId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<WeeklyPlan>()
            .HasOne(wp => wp.Recipe)
            .WithMany()
            .HasForeignKey(wp => wp.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<WeeklyPlan>()
            .HasOne(wp => wp.User)
            .WithMany()
            .HasForeignKey(wp => wp.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<WeeklyPlan>()
            .HasIndex(wp => new { wp.UserId, wp.DayOfWeek });
    }
}