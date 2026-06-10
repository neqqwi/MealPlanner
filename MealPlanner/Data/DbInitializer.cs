using MealPlanner.Models;

namespace MealPlanner.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            if (context.Ingredients.Any())
            {
                return;
            }

            var ingridients = new Ingredient[]
            {
                new Ingredient { Name = "Мука пшеничная", Unit="г" },
                new Ingredient { Name = "Мука ржаная", Unit="г" },
                new Ingredient { Name = "Сахар", Unit="г" },
                new Ingredient { Name = "Соль", Unit="г" },
                new Ingredient { Name = "Сода", Unit="г" },
                new Ingredient { Name = "Разрыхлитель", Unit="г" },
                new Ingredient { Name = "Крахмал", Unit="г" },
                new Ingredient { Name = "Макароны", Unit="г" },
                new Ingredient { Name = "Рис", Unit="г" },
                new Ingredient { Name = "Гречка", Unit="г" },
                new Ingredient { Name = "Овсянка", Unit="г" },
                new Ingredient { Name = "Манка", Unit="г" },
                new Ingredient { Name = "Перец черный", Unit="г" },
                new Ingredient { Name = "Паприка", Unit="г" },
                new Ingredient { Name = "Корица", Unit="г" },
                new Ingredient { Name = "Ванилин", Unit="г" },
                new Ingredient { Name = "Кориандр", Unit="г" },
                new Ingredient { Name = "Куркума", Unit="г" },
                new Ingredient { Name = "Сыр твердый", Unit="г" },
                new Ingredient { Name = "Творог", Unit="г" },
                new Ingredient { Name = "Масло сливочное", Unit="г" },
                new Ingredient { Name = "Куриное филе", Unit="г" },
                new Ingredient { Name = "Фарш говяжий", Unit="г" },
                new Ingredient { Name = "Свинина", Unit="г" },
                new Ingredient { Name = "Говядина", Unit="г" },
                new Ingredient { Name = "Рыбное филе", Unit="г" },
                new Ingredient { Name = "Лук репчатый", Unit="г" },
                new Ingredient { Name = "Чеснок", Unit="г" },
                new Ingredient { Name = "Картофель", Unit="г" },
                new Ingredient { Name = "Морковь", Unit="г" },
                new Ingredient { Name = "Помидоры", Unit="г" },
                new Ingredient { Name = "Огурцы", Unit="г" },
                new Ingredient { Name = "Перец болгарский", Unit="г" },
                new Ingredient { Name = "Кабачок", Unit="г" },
                new Ingredient { Name = "Баклажан", Unit="г" },
                new Ingredient { Name = "Укроп", Unit="г" },
                new Ingredient { Name = "Петрушка", Unit="г" },

                new Ingredient { Name = "Молоко", Unit="мл" },
                new Ingredient { Name = "Сливки", Unit="мл" },
                new Ingredient { Name = "Сметана", Unit="мл" },
                new Ingredient { Name = "Кефир", Unit="мл" },
                new Ingredient { Name = "Масло подсолнечное", Unit="мл" },
                new Ingredient { Name = "Масло оливковое", Unit="мл" },
                new Ingredient { Name = "Уксус", Unit="мл" },
                new Ingredient { Name = "Соевый соус", Unit="мл" },
                new Ingredient { Name = "Томатная паста", Unit="мл" },
                new Ingredient { Name = "Майонез", Unit="мл" },
                new Ingredient { Name = "Кетчуп", Unit="мл" },
                new Ingredient { Name = "Горчица", Unit="мл" },

                new Ingredient { Name = "Яйца", Unit="шт" }
            };

            context.Ingredients.AddRange(ingridients);
            context.SaveChanges();
        }
    }
}