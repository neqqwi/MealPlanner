using MealPlanner.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace MealPlanner.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(userManager);

            var demoUser = await userManager.FindByIdAsync(AppConstants.DemoUserId);
            if (demoUser == null)
            {
                demoUser = new ApplicationUser
                {
                    Id = AppConstants.DemoUserId,
                    UserName = "demouser",
                    Email = "demo@mealplanner.local",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(demoUser, "DemoPassword123!");
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Не удалось создать демо-пользователя: {errors}");
                }
            }

            Initialize(context, AppConstants.DemoUserId);
        }
        public static void Initialize(AppDbContext context, string userId)
        {
            ArgumentNullException.ThrowIfNull(context);

            if (!context.Ingredients.Any(i => i.UserId == userId))
            {
                var ingredients = new Ingredient[]
                {
                    new Ingredient { Name = "Мука пшеничная", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Мука ржаная", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Сахар", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Соль", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Сода", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Разрыхлитель", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Крахмал", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Макароны", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Рис", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Гречка", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Овсянка", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Манка", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Перец черный молотый", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Паприка", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Корица молотая", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Ванилин", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Кориандр", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Куркума", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Сыр твердый", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Творог", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Масло сливочное", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Куриное филе", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Фарш говяжий", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Свинина", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Говядина", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Рыбное филе", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Лук репчатый", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Чеснок", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Картофель", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Морковь", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Помидоры", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Огурцы", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Перец болгарский", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Кабачок", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Баклажан", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Укроп", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Петрушка", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Молоко", Unit = "мл", UserId = userId },
                    new Ingredient { Name = "Сливки", Unit = "мл", UserId = userId },
                    new Ingredient { Name = "Сметана", Unit = "мл", UserId = userId },
                    new Ingredient { Name = "Кефир", Unit = "мл", UserId = userId },
                    new Ingredient { Name = "Масло подсолнечное", Unit = "мл", UserId = userId },
                    new Ingredient { Name = "Масло оливковое", Unit = "мл", UserId = userId },
                    new Ingredient { Name = "Уксус", Unit = "мл", UserId = userId },
                    new Ingredient { Name = "Соевый соус", Unit = "мл", UserId = userId },
                    new Ingredient { Name = "Томатная паста", Unit = "мл", UserId = userId },
                    new Ingredient { Name = "Майонез", Unit = "мл", UserId = userId },
                    new Ingredient { Name = "Кетчуп", Unit = "мл", UserId = userId },
                    new Ingredient { Name = "Горчица", Unit = "мл", UserId = userId },
                    new Ingredient { Name = "Яйца", Unit = "шт", UserId = userId },
                    new Ingredient { Name = "Яблоки", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Салат листовой", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Мята", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Шоколад темный", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Сахарная пудра", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Курица целая", Unit = "шт", UserId = userId },
                    new Ingredient { Name = "Розмарин", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Лимон", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Маринованные огурцы", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Зеленый лук", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Булочки для бургеров", Unit = "шт", UserId = userId },
                    new Ingredient { Name = "Вода", Unit = "мл", UserId = userId },
                    new Ingredient { Name = "Ананас", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Шампиньоны", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Базилик", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Моцарелла", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Уксус бальзамический", Unit = "мл", UserId = userId },
                    new Ingredient { Name = "Лосось копченый", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Сыр сливочный", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Хлеб ржаной", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Каперсы", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Кунжут", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Имбирь свежий", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Мед", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Черника", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Малина", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Банан", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Клубника", Unit = "г", UserId = userId },
                    new Ingredient { Name = "Вяленые томаты", Unit = "г", UserId = userId }
                };

                context.Ingredients.AddRange(ingredients);
                context.SaveChanges();

                var ingredientDict = ingredients.ToDictionary(i => i.Name, i => i);

                var recipes = new Recipe[]
                {
                    new Recipe
                    {
                        Name = "Яблочный крамбл",
                        Description = "Классический британский десерт с нежными печеными яблоками и хрустящей " +
                            "песочной крошкой. Готовится проще пирога, а получается не менее вкусно!",
                        CookingTime = 60,
                        Instructions = "Разогрейте духовку до 190°C. Форму для запекания смажьте маслом.\n\n" +
                            "Яблоки очистите от кожуры и сердцевины, нарежьте кубиками или дольками.\n\n" +
                            "Смешайте яблоки с сахаром (50 г) и корицей. Выложите в форму для запекания.\n\n" +
                            "В миске смешайте муку, оставшийся сахар (100 г) и холодное сливочное масло, нарезанное кубиками.\n\n" +
                            "Руками перетрите масло с мукой и сахаром до состояния крупной крошки.\n\n" +
                            "Равномерно посыпьте яблочную начинку крошкой.\n\n" +
                            "Выпекайте 30-35 минут до золотистого цвета крошки и мягкости яблок.\n\n" +
                            "Подавайте теплым, можно с шариком ванильного мороженого или взбитыми сливками.",
                        ImagePath = "/uploads/recipes/apple-crumble-pie.jpg",
                        UserId = userId,
                        RecipeIngredients = new List<RecipeIngredient>
                        {
                            new RecipeIngredient { Ingredient = ingredientDict["Яблоки"], Amount = 500 },
                            new RecipeIngredient { Ingredient = ingredientDict["Сахар"], Amount = 150 },
                            new RecipeIngredient { Ingredient = ingredientDict["Масло сливочное"], Amount = 100 },
                            new RecipeIngredient { Ingredient = ingredientDict["Корица молотая"], Amount = 12 },
                            new RecipeIngredient { Ingredient = ingredientDict["Мука пшеничная"], Amount = 150 }
                        }
                    },
                    new Recipe
                    {
                        Name = "Тайский салат с говядиной",
                        Description = "Освежающий азиатский салат с сочной говядиной, свежими овощами и пряной " +
                            "заправкой. Идеальное сочетание теплого мяса и хрустящих овощей.",
                        CookingTime = 30,
                        Instructions = "Подготовьте все ингредиенты для салата. Говядину нарежьте тонкими ломтиками " +
                            "поперек волокон.\n\n" +
                            "Разогрейте сковороду-гриль или обычную сковороду на сильном огне. Обжарьте говядину по 2-3 " +
                            "минуты с каждой стороны до золотистой корочки, но чтобы мясо внутри осталось сочным. Переложите " +
                            "на тарелку и дайте отдохнуть 5 минут.\n\n" +
                            "Пока мясо отдыхает, приготовьте заправку. В небольшой миске смешайте соевый соус, оливковое масло, " +
                            "сок лайма (или уксус), сахар и щепотку черного перца. Хорошо взбейте венчиком до растворения сахара.\n\n" +
                            "Огурцы нарежьте тонкими полукружочками или используйте овощечистку для создания длинных лент. Черри " +
                            "разрежьте пополам. Красный лук нарежьте очень тонкими кольцами или полукольцами.\n\n" +
                            "Салатные листья порвите руками на средние кусочки и выложите на дно глубокой тарелки или миски. " +
                            "Сверху распределите огурцы, помидоры черри и кольца красного лука.\n\n" +
                            "Отдохнувшую говядину нарежьте тонкими полосками. Равномерно разложите мясо поверх овощей.\n\n" +
                            "Щедро посыпьте салат свежей мятой и петрушкой. Полейте подготовленной заправкой непосредственно " +
                            "перед подачей.\n\n" +
                            "Аккуратно перемешайте салат прямо в тарелке, чтобы заправка равномерно распределилась по всем " +
                            "ингредиентам. Подавайте, пока мясо еще теплое.",
                        ImagePath = "/uploads/recipes/thai-beef-salad.jpg",
                        UserId = userId,
                        RecipeIngredients = new List<RecipeIngredient>
                        {
                            new RecipeIngredient { Ingredient = ingredientDict["Говядина"], Amount = 300 },
                            new RecipeIngredient { Ingredient = ingredientDict["Огурцы"], Amount = 150 },
                            new RecipeIngredient { Ingredient = ingredientDict["Помидоры"], Amount = 200 },
                            new RecipeIngredient { Ingredient = ingredientDict["Лук репчатый"], Amount = 100 },
                            new RecipeIngredient { Ingredient = ingredientDict["Соевый соус"], Amount = 40 },
                            new RecipeIngredient { Ingredient = ingredientDict["Масло оливковое"], Amount = 30 },
                            new RecipeIngredient { Ingredient = ingredientDict["Уксус"], Amount = 15 },
                            new RecipeIngredient { Ingredient = ingredientDict["Сахар"], Amount = 10 },
                            new RecipeIngredient { Ingredient = ingredientDict["Перец черный молотый"], Amount = 2 },
                            new RecipeIngredient { Ingredient = ingredientDict["Соль"], Amount = 5 },
                            new RecipeIngredient { Ingredient = ingredientDict["Петрушка"], Amount = 20 },
                            new RecipeIngredient { Ingredient = ingredientDict["Салат листовой"], Amount = 100 },
                            new RecipeIngredient { Ingredient = ingredientDict["Мята"], Amount = 15 }
                        }
                    },
                    new Recipe
                    {
                        Name = "Шоколадный фондан",
                        Description = "Изысканный французский десерт с хрустящей корочкой и жидкой шоколадной " +
                            "начинкой внутри. Главный секрет - точное время выпекания, чтобы центр остался тягучим.",
                        CookingTime = 35,
                        Instructions = "Разогрейте духовку до 200°C. Формочки для кексов щедро смажьте сливочным маслом " +
                            "и присыпьте какао или мукой, чтобы десерт легко вынимался.\n\n" +
                            "Темный шоколад поломайте на кусочки и вместе со сливочным маслом растопите на водяной бане " +
                            "или в микроволновке короткими импульсами по 20 секунд, помешивая после каждого. Масса должна " +
                            "стать гладкой и блестящей. Остудите до комнатной температуры.\n\n" +
                            "В отдельной миске взбейте яйца с сахаром и щепоткой соли до пышной светлой пены. Это займет " +
                            "около 3-4 минут миксером на средней скорости. Масса должна увеличиться в объеме примерно вдвое.\n\n" +
                            "Аккуратно влейте остывшую шоколадно-масляную смесь во взбитые яйца и бережно перемешайте " +
                            "лопаткой движениями снизу вверх, чтобы не потерять воздушность.\n\n" +
                            "Просейте муку через сито и аккуратно вмешайте ее в шоколадную массу до однородности. Тесто " +
                            "должно получиться довольно жидким - это нормально.\n\n" +
                            "Разлейте тесто по подготовленным формочкам, заполняя их примерно на три четверти. Поставьте " +
                            "в разогретую духовку и выпекайте ровно 10-12 минут. Края должны схватиться, а центр оставаться жидким.\n\n" +
                            "Достаньте формочки из духовки и дайте постоять 1 минуту. Аккуратно переверните каждый фондан " +
                            "на тарелку, слегка постучав по дну формочки.\n\n" +
                            "Посыпьте десерт сахарной пудрой через мелкое сито и подавайте. Идеально сочетается с шариком " +
                            "ванильного мороженого или свежими ягодами.",
                        ImagePath = "/uploads/recipes/chocolate-fondant.jpg",
                        UserId = userId,
                        RecipeIngredients = new List<RecipeIngredient>
                        {
                            new RecipeIngredient { Ingredient = ingredientDict["Шоколад темный"], Amount = 50 },
                            new RecipeIngredient { Ingredient = ingredientDict["Масло сливочное"], Amount = 100 },
                            new RecipeIngredient { Ingredient = ingredientDict["Яйца"], Amount = 3 },
                            new RecipeIngredient { Ingredient = ingredientDict["Сахар"], Amount = 80 },
                            new RecipeIngredient { Ingredient = ingredientDict["Мука пшеничная"], Amount = 50 },
                            new RecipeIngredient { Ingredient = ingredientDict["Соль"], Amount = 2 },
                            new RecipeIngredient { Ingredient = ingredientDict["Сахарная пудра"], Amount = 10 }
                        }
                    },
                    new Recipe
                    {
                        Name = "Запеченная курица с картофелем и розмарином",
                        Description = "Классическое блюдо для уютного семейного ужина. Сочная курица с хрустящей " +
                            "золотистой корочкой, запеченная вместе с молодым картофелем, чесноком, лимоном и ароматным розмарином.",
                        CookingTime = 90,
                        Instructions = "Духовку разогрейте до 200°C. Курицу тщательно промойте под холодной водой и обсушите " +
                            "бумажными полотенцами - это ключ к хрустящей золотистой корочке.\n\n" +
                            "Приготовьте ароматную смесь для натирания. В небольшой миске смешайте соль, черный перец, паприку " +
                            "и мелко рубленый чеснок. Добавьте пару столовых ложек оливкового масла и перемешайте до состояния " +
                            "густой пасты.\n\n" +
                            "Щедро натрите курицу полученной смесью со всех сторон, не забывая про внутреннюю полость. Особенно " +
                            "тщательно вотрите специи под кожу грудки и ножек - там мясо самое толстое и требует больше вкуса.\n\n" +
                            "Картофель тщательно вымойте щеткой (чистить не обязательно, если картофель молодой). Крупные клубни " +
                            "разрежьте на четвертинки, мелкие - пополам. Выложите картофель вокруг курицы в чугунную сковороду или " +
                            "форму для запекания.\n\n" +
                            "Посыпьте картофель солью, перцем и сбрызните оливковым маслом. Веточки розмарина разложите между " +
                            "картофелем и вокруг курицы - при запекании они отдадут свой невероятный аромат маслу и сокам.\n\n" +
                            "Поставьте сковороду в разогретую духовку и запекайте 60-75 минут. Через 40 минут проверьте готовность: " +
                            "проткните ножом самое толстое место ножки - должен вытекать прозрачный сок, без розового оттенка.\n\n" +
                            "Если корочка подрумянилась слишком быстро, а мясо внутри еще сырое, накройте курицу фольгой и доведите " +
                            "до готовности. За 10 минут до конца снимите фольгу, чтобы корочка стала хрустящей.\n\n" +
                            "Достаньте курицу из духовки и дайте ей отдохнуть 10-15 минут перед разделкой - это позволит сокам " +
                            "равномерно распределиться по мясу. Подавайте прямо в сковороде, украсив дольками лимона и свежим розмарином.",
                        ImagePath = "/uploads/recipes/lemon-rosemary-chicken.jpg",
                        UserId = userId,
                        RecipeIngredients = new List<RecipeIngredient>
                        {
                            new RecipeIngredient { Ingredient = ingredientDict["Курица целая"], Amount = 1 },
                            new RecipeIngredient { Ingredient = ingredientDict["Картофель"], Amount = 800 },
                            new RecipeIngredient { Ingredient = ingredientDict["Масло оливковое"], Amount = 60 },
                            new RecipeIngredient { Ingredient = ingredientDict["Соль"], Amount = 15 },
                            new RecipeIngredient { Ingredient = ingredientDict["Перец черный молотый"], Amount = 5 },
                            new RecipeIngredient { Ingredient = ingredientDict["Паприка"], Amount = 10 },
                            new RecipeIngredient { Ingredient = ingredientDict["Чеснок"], Amount = 20 },
                            new RecipeIngredient { Ingredient = ingredientDict["Розмарин"], Amount = 15 },
                            new RecipeIngredient { Ingredient = ingredientDict["Лимон"], Amount = 100 }
                        }
                    },
                    new Recipe
                    {
                        Name = "Хрустящий куриный бургер в стиле Нэшвилл",
                        Description = "Сочная курица в хрустящей панировке с острой глазурью, маринованными " +
                            "огурчиками и фирменным соусом на булочке бриошь. Американский фастфуд домашнего приготовления!",
                        CookingTime = 40,
                        Instructions = "Подготовьте куриное филе. Если куски слишком толстые, слегка отбейте их через " +
                            "пищевую пленку до равномерной толщины около 2 см. Посолите и поперчите с обеих сторон.\n\n" +
                            "В глубокой миске взбейте яйца с молоком. В отдельной широкой тарелке смешайте муку, соль, " +
                            "черный перец, паприку и куркуму. Обваляйте каждый кусок курицы сначала в мучной смеси, затем " +
                            "окуните в яичную смесь, и снова в муку, хорошо прижимая панировку руками.\n\n" +
                            "Разогрейте масло в глубокой сковороде или фритюрнице до 170-180°C. Масло должно покрывать курицу " +
                            "хотя бы наполовину. Обжаривайте курицу по 5-6 минут с каждой стороны до золотисто-коричневого " +
                            "цвета и полной готовности (внутренняя температура должна быть 75°C). Переложите на бумажные полотенца.\n\n" +
                            "Пока жарится курица, приготовьте острый соус. В небольшой кастрюльке смешайте майонез, кетчуп, " +
                            "горчицу, немного острого перца (по желанию) и щепотку паприки. Хорошо перемешайте до однородности. " +
                            "Поставьте в холодильник.\n\n" +
                            "Булочки разрежьте пополам. Слегка обжарьте срезы на сухой сковороде до легкого румянца или смажьте " +
                            "мягким маслом и подсушите.\n\n" +
                            "На нижнюю половинку булочки выложите 2-3 кружочка маринованных огурцов. Сверху поместите горячую " +
                            "куриную котлету.\n\n" +
                            "Щедро полейте курицу приготовленным соусом, чтобы он стекал по краям. Посыпьте мелко нарезанным " +
                            "зеленым луком.\n\n" +
                            "Накройте верхней половинкой булочки. Подавайте немедленно, пока курица еще горячая и хрустящая, " +
                            "с дополнительными овощами или картофелем фри.",
                        ImagePath = "/uploads/recipes/nashville-chicken-burger.jpg",
                        UserId = userId,
                        RecipeIngredients = new List<RecipeIngredient>
                        {
                            new RecipeIngredient { Ingredient = ingredientDict["Куриное филе"], Amount = 400 },
                            new RecipeIngredient { Ingredient = ingredientDict["Мука пшеничная"], Amount = 150 },
                            new RecipeIngredient { Ingredient = ingredientDict["Яйца"], Amount = 2 },
                            new RecipeIngredient { Ingredient = ingredientDict["Молоко"], Amount = 50 },
                            new RecipeIngredient { Ingredient = ingredientDict["Масло подсолнечное"], Amount = 200 },
                            new RecipeIngredient { Ingredient = ingredientDict["Соль"], Amount = 10 },
                            new RecipeIngredient { Ingredient = ingredientDict["Перец черный молотый"], Amount = 5 },
                            new RecipeIngredient { Ingredient = ingredientDict["Паприка"], Amount = 10 },
                            new RecipeIngredient { Ingredient = ingredientDict["Куркума"], Amount = 3 },
                            new RecipeIngredient { Ingredient = ingredientDict["Майонез"], Amount = 100 },
                            new RecipeIngredient { Ingredient = ingredientDict["Кетчуп"], Amount = 50 },
                            new RecipeIngredient { Ingredient = ingredientDict["Горчица"], Amount = 20 },
                            new RecipeIngredient { Ingredient = ingredientDict["Маринованные огурцы"], Amount = 100 },
                            new RecipeIngredient { Ingredient = ingredientDict["Зеленый лук"], Amount = 20 },
                            new RecipeIngredient { Ingredient = ingredientDict["Булочки для бургеров"], Amount = 2 }
                        }
                    },
                    new Recipe
                    {
                        Name = "BBQ пицца с курицей и ананасом",
                        Description = "Сочная домашняя пицца с нежным куриным филе, сладким ананасом, красным " +
                            "луком и ароматным соусом барбекю. Идеальный баланс вкусов для любителей гавайской пиццы!",
                        CookingTime = 60,
                        Instructions = "Приготовьте тесто для пиццы. В глубокой миске смешайте просеянную муку, " +
                            "соль и разрыхлитель. Постепенно влейте теплую воду и оливковое масло. Замесите " +
                            "эластичное тесто в течение 8-10 минут. Накройте полотенцем и оставьте в теплом месте " +
                            "на 30-40 минут для подъема.\n\n" +
                            "Пока тесто поднимается, приготовьте курицу. Куриное филе отварите в подсоленной воде до " +
                            "готовности (около 20 минут) или запеките в духовке при 180°C. Остывшее мясо разберите на " +
                            "волокна или нарежьте небольшими кусочками.\n\n" +
                            "Приготовьте соус BBQ. В небольшой кастрюле смешайте томатную пасту, сахар, уксус, горчицу, " +
                            "немного воды и специи (паприку, черный перец). Доведите до кипения на медленном огне, помешивая, " +
                            "пока сахар полностью не растворится. Снимите с огня и дайте остыть.\n\n" +
                            "Подготовьте остальные ингредиенты. Красный лук нарежьте очень тонкими кольцами или полукольцами. " +
                            "Если используете консервированный ананас, слейте жидкость и нарежьте его кубиками. Твердый сыр " +
                            "натрите на крупной терке.\n\n" +
                            "Разогрейте духовку до максимальной температуры (220-250°C). Если есть камень для пиццы - " +
                            "поместите его в духовку заранее.\n\n" +
                            "Раскатайте поднявшееся тесто на присыпанной мукой поверхности в круг толщиной около 3-4 мм. " +
                            "Переложите на пергамент для выпечки или противень, смазанный маслом.\n\n" +
                            "Распределите по тесту соус BBQ, оставив бортики около 1-2 см. Равномерно разложите кусочки " +
                            "курицы, кубики ананаса и кольца красного лука. Щедро посыпьте тертым сыром.\n\n" +
                            "Выпекайте пиццу 12-15 минут до золотистого цвета корочки и расплавленного сыра. Готовность " +
                            "проверяйте по корочке - она должна стать хрустящей и подрумяниться.\n\n" +
                            "Готовую пиццу достаньте из духовки, дайте отдохнуть 2-3 минуты. Посыпьте свежим кориандром, " +
                            "нарежьте на кусочки и подавайте горячей.",
                        ImagePath = "/uploads/recipes/bbq-chicken-pizza.jpg",
                        UserId = userId,
                        RecipeIngredients = new List<RecipeIngredient>
                        {
                            new RecipeIngredient { Ingredient = ingredientDict["Мука пшеничная"], Amount = 300 },
                            new RecipeIngredient { Ingredient = ingredientDict["Соль"], Amount = 5 },
                            new RecipeIngredient { Ingredient = ingredientDict["Разрыхлитель"], Amount = 5 },
                            new RecipeIngredient { Ingredient = ingredientDict["Масло оливковое"], Amount = 30 },
                            new RecipeIngredient { Ingredient = ingredientDict["Вода"], Amount = 150 },
                            new RecipeIngredient { Ingredient = ingredientDict["Куриное филе"], Amount = 300 },
                            new RecipeIngredient { Ingredient = ingredientDict["Сыр твердый"], Amount = 200 },
                            new RecipeIngredient { Ingredient = ingredientDict["Томатная паста"], Amount = 100 },
                            new RecipeIngredient { Ingredient = ingredientDict["Сахар"], Amount = 30 },
                            new RecipeIngredient { Ingredient = ingredientDict["Уксус"], Amount = 15 },
                            new RecipeIngredient { Ingredient = ingredientDict["Горчица"], Amount = 10 },
                            new RecipeIngredient { Ingredient = ingredientDict["Паприка"], Amount = 5 },
                            new RecipeIngredient { Ingredient = ingredientDict["Перец черный молотый"], Amount = 3 },
                            new RecipeIngredient { Ingredient = ingredientDict["Лук репчатый"], Amount = 100 },
                            new RecipeIngredient { Ingredient = ingredientDict["Ананас"], Amount = 200 },
                            new RecipeIngredient { Ingredient = ingredientDict["Кориандр"], Amount = 10 }
                        }
                    },
                    new Recipe
                    {
                        Name = "Феттучини Альфредо с курицей и грибами",
                        Description = "Нежная итальянская паста в сливочном соусе с сочными кусочками куриного филе " +
                            "и ароматными шампиньонами. Классическое блюдо, которое готовится за 25 минут и покоряет с первого кусочка.",
                        CookingTime = 35,
                        Instructions = "Поставьте большую кастрюлю с подсоленной водой на сильный огонь и доведите до " +
                            "кипения. Отварите феттучини согласно инструкции на упаковке до состояния аль денте " +
                            "(обычно 8-10 минут). Перед сливом воды оставьте полстакана крахмальной воды от варки.\n\n" +
                            "Пока варится паста, разогрейте большую сковороду на среднем огне. Добавьте сливочное " +
                            "масло и растопите его. Нарежьте куриное филе небольшими кубиками или полосками и " +
                            "обжаривайте 5-7 минут до золотистого цвета и полной готовности. Переложите курицу на тарелку.\n\n" +
                            "В ту же сковороду добавьте нарезанные пластинками шампиньоны. Обжаривайте грибы 4-5 " +
                            "минут, пока они не подрумянятся и не выпустят всю влагу. Добавьте мелко нарезанный чеснок " +
                            "и готовьте еще 30 секунд до появления аромата.\n\n" +
                            "Влейте в сковороду сливки, уменьшите огонь до среднего и доведите до легкого кипения. " +
                            "Готовите соус 3-4 минуты, периодически помешивая, пока он немного не загустеет.\n\n" +
                            "Добавьте в соус большую часть тертого сыра (оставьте немного для подачи) и хорошо " +
                            "перемешайте до полного растворения. Посолите и поперчите по вкусу.\n\n" +
                            "Верните обжаренную курицу в сковороду с соусом. Слейте воду с пасты и добавьте феттучини " +
                            "прямо в сковороду. Аккуратно перемешайте все вместе, чтобы каждая ниточка пасты покрылась " +
                            "соусом. Если соус слишком густой, добавьте немного воды от варки пасты.\n\n" +
                            "Подавайте немедленно, посыпав оставшимся сыром и свежей петрушкой. Можно добавить щепотку " +
                            "черного перца сверху.",
                        ImagePath = "/uploads/recipes/fettuccine-alfredo.jpg",
                        UserId = userId,
                        RecipeIngredients = new List<RecipeIngredient>
                        {
                            new RecipeIngredient { Ingredient = ingredientDict["Макароны"], Amount = 400 },
                            new RecipeIngredient { Ingredient = ingredientDict["Куриное филе"], Amount = 300 },
                            new RecipeIngredient { Ingredient = ingredientDict["Шампиньоны"], Amount = 200 },
                            new RecipeIngredient { Ingredient = ingredientDict["Сливки"], Amount = 200 },
                            new RecipeIngredient { Ingredient = ingredientDict["Сыр твердый"], Amount = 100 },
                            new RecipeIngredient { Ingredient = ingredientDict["Масло сливочное"], Amount = 30 },
                            new RecipeIngredient { Ingredient = ingredientDict["Чеснок"], Amount = 10 },
                            new RecipeIngredient { Ingredient = ingredientDict["Соль"], Amount = 5 },
                            new RecipeIngredient { Ingredient = ingredientDict["Перец черный молотый"], Amount = 2 },
                            new RecipeIngredient { Ingredient = ingredientDict["Петрушка"], Amount = 10 }
                        }
                    },
                    new Recipe
                    {
                        Name = "Острый томатный суп с говядиной",
                        Description = "Насыщенный, пряный суп с нежной говядиной, рисом и ароматными специями. " +
                            "Подается с долькой лимона и свежей зеленью - идеальное согревающее блюдо для прохладного дня.",
                        CookingTime = 90,
                        Instructions = "Говядину промойте, нарежьте небольшими кубиками по 2-3 см. В кастрюле с " +
                            "толстым дном разогрейте подсолнечное масло и обжарьте мясо на сильном огне до румяной " +
                            "корочки со всех сторон, около 5-7 минут.\n\n" +
                            "Лук репчатый очистите и мелко нарежьте. Добавьте к мясу и обжаривайте вместе, помешивая, " +
                            "пока лук не станет прозрачным и слегка золотистым.\n\n" +
                            "Чеснок очистите и пропустите через пресс или мелко порубите ножом. Добавьте к мясу с луком " +
                            "и обжаривайте еще 1-2 минуты до появления характерного аромата.\n\n" +
                            "В кастрюлю влейте горячую воду, доведите до кипения и убавьте огонь до среднего. Снимите " +
                            "образовавшуюся пену шумовкой. Варите бульон под крышкой около 40 минут, пока говядина не " +
                            "станет мягкой.\n\n" +
                            "Рис тщательно промойте в нескольких водах, пока вода не станет прозрачной. Добавьте рис в " +
                            "бульон и варите 15 минут.\n\n" +
                            "Томатную пасту разведите в половине стакана теплой воды и влейте в суп. Добавьте соль, " +
                            "черный перец, кориандр и паприку по вкусу. Тщательно перемешайте.\n\n" +
                            "Варите суп еще 10-15 минут на слабом огне, пока рис полностью не приготовится, а вкусы " +
                            "не объединятся. Попробуйте на соль и остроту, при необходимости добавьте специи.\n\n" +
                            "Разлейте горячий суп по глубоким тарелкам. В каждую порцию добавьте дольку лимона, " +
                            "щедро посыпьте свежей петрушкой. Подавайте немедленно с лепешкой или свежим хлебом.",
                        ImagePath = "/uploads/recipes/tomato-beef-soup.jpg",
                        UserId = userId,
                        RecipeIngredients = new List<RecipeIngredient>
                        {
                            new RecipeIngredient { Ingredient = ingredientDict["Говядина"], Amount = 400 },
                            new RecipeIngredient { Ingredient = ingredientDict["Рис"], Amount = 80 },
                            new RecipeIngredient { Ingredient = ingredientDict["Лук репчатый"], Amount = 150 },
                            new RecipeIngredient { Ingredient = ingredientDict["Чеснок"], Amount = 20 },
                            new RecipeIngredient { Ingredient = ingredientDict["Томатная паста"], Amount = 60 },
                            new RecipeIngredient { Ingredient = ingredientDict["Масло подсолнечное"], Amount = 30 },
                            new RecipeIngredient { Ingredient = ingredientDict["Соль"], Amount = 10 },
                            new RecipeIngredient { Ingredient = ingredientDict["Перец черный молотый"], Amount = 3 },
                            new RecipeIngredient { Ingredient = ingredientDict["Кориандр"], Amount = 5 },
                            new RecipeIngredient { Ingredient = ingredientDict["Паприка"], Amount = 5 },
                            new RecipeIngredient { Ingredient = ingredientDict["Петрушка"], Amount = 20 },
                            new RecipeIngredient { Ingredient = ingredientDict["Лимон"], Amount = 100 },
                            new RecipeIngredient { Ingredient = ingredientDict["Вода"], Amount = 1500 }
                        }
                    },
                    new Recipe
                    {
                        Name = "Стейк из говядины с картофельным пюре и овощами",
                        Description = "Сочный стейк из говядины с идеальной прожаркой, поданный с нежным картофельным " +
                            "пюре и овощами на пару. Классическое сочетание для сытного ужина.",
                        CookingTime = 40,
                        Instructions = "Картофель очистите, нарежьте крупными кусками и отварите в подсоленной воде " +
                            "до готовности, около 20 минут. Слейте воду.\n\n" +
                            "Пока варится картофель, подготовьте стейк. Говядину промокните бумажными полотенцами, " +
                            "натрите солью, черным перцем и сбрызните оливковым маслом. Оставьте при комнатной температуре " +
                            "на 15-20 минут.\n\n" +
                            "Разогрейте сковороду-гриль или чугунную сковороду на сильном огне до очень горячего состояния. " +
                            "Выложите стейк и обжаривайте по 3-4 минуты с каждой стороны для средней прожарки (medium). Для " +
                            "более сильной прожарки готовьте дольше.\n\n" +
                            "За 1-2 минуты до готовности добавьте в сковороду веточку розмарина и зубчик чеснока, поливайте " +
                            "стейк ароматным маслом.\n\n" +
                            "Готовый стейк переложите на тарелку, накройте фольгой и дайте отдохнуть 5-7 минут. Это важно " +
                            "для сохранения сочности.\n\n" +
                            "Слейте воду с картофеля, добавьте горячее молоко и сливочное масло. Разомните толкушкой до " +
                            "однородного пюре. Посолите по вкусу.\n\n" +
                            "Овощи (морковь, кабачки) нарежьте брусочками и отварите на пару или в кипящей подсоленной " +
                            "воде 5-7 минут до мягкости, но чтобы остались слегка хрустящими.\n\n" +
                            "Для соуса смешайте томатную пасту, соевый соус, горчицу, немного сахара и воды. Проварите " +
                            "2-3 минуты до загустения.\n\n" +
                            "Подавайте стейк с картофельным пюре, овощами и соусом. Украсьте свежей петрушкой.",
                        ImagePath = "/uploads/recipes/beef-steak-mashed.jpg",
                        UserId = userId,
                        RecipeIngredients = new List<RecipeIngredient>
                        {
                            new RecipeIngredient { Ingredient = ingredientDict["Говядина"], Amount = 350 },
                            new RecipeIngredient { Ingredient = ingredientDict["Картофель"], Amount = 500 },
                            new RecipeIngredient { Ingredient = ingredientDict["Морковь"], Amount = 150 },
                            new RecipeIngredient { Ingredient = ingredientDict["Кабачок"], Amount = 200 },
                            new RecipeIngredient { Ingredient = ingredientDict["Молоко"], Amount = 100 },
                            new RecipeIngredient { Ingredient = ingredientDict["Масло сливочное"], Amount = 50 },
                            new RecipeIngredient { Ingredient = ingredientDict["Масло оливковое"], Amount = 30 },
                            new RecipeIngredient { Ingredient = ingredientDict["Томатная паста"], Amount = 30 },
                            new RecipeIngredient { Ingredient = ingredientDict["Соевый соус"], Amount = 20 },
                            new RecipeIngredient { Ingredient = ingredientDict["Горчица"], Amount = 10 },
                            new RecipeIngredient { Ingredient = ingredientDict["Сахар"], Amount = 10 },
                            new RecipeIngredient { Ingredient = ingredientDict["Соль"], Amount = 10 },
                            new RecipeIngredient { Ingredient = ingredientDict["Перец черный молотый"], Amount = 3 },
                            new RecipeIngredient { Ingredient = ingredientDict["Розмарин"], Amount = 2 },
                            new RecipeIngredient { Ingredient = ingredientDict["Петрушка"], Amount = 15 },
                            new RecipeIngredient { Ingredient = ingredientDict["Вода"], Amount = 3000 }
                        }
                    },
                    new Recipe
                    {
                        Name = "Салат Капрезе",
                        Description = "Итальянская классика: сочные томаты, нежная моцарелла, ароматный базилик " +
                            "и бальзамический уксус. Простота и изысканность в одной тарелке.",
                        CookingTime = 15,
                        Instructions = "Выберите спелые, но плотные томаты. Хорошо помойте их и обсушите бумажным " +
                            "полотенцем. Нарежьте помидоры кружочками толщиной около 5-7 мм.\n\n" +
                            "Сыр также нарежьте кружочками такой же толщины, как и помидоры. Если сыр слишком " +
                            "мягкий и прилипает к ножу, смочите нож холодной водой.\n\n" +
                            "Свежий базилик помойте и аккуратно обсушите. Оставьте несколько целых листиков для " +
                            "украшения, остальные можно слегка порвать руками.\n\n" +
                            "На большое плоское блюдо выложите кружочки помидоров и сыра, чередуя их: ломтик помидора, " +
                            "ломтик сыра, снова помидор и так далее. Можно выкладывать по кругу или в линию.\n\n" +
                            "Посолите и поперчите салат по вкусу. Слегка сбрызните оливковым маслом первого холодного отжима.\n\n" +
                            "Если используете бальзамический уксус или бальзамический крем, полейте им салат тонкой струйкой по " +
                            "спирали.\n\n" +
                            "Украсьте салат свежими листьями базилика, распределив их между ломтиками помидоров и сыра.\n\n" +
                            "Подавайте салат Капрезе сразу после приготовления, пока томаты и сыр не пустили сок. Идеально " +
                            "сочетается с хрустящим багетом или чиабаттой.",
                        ImagePath = "/uploads/recipes/caprese.jpg",
                        UserId = userId,
                        RecipeIngredients = new List<RecipeIngredient>
                        {
                            new RecipeIngredient { Ingredient = ingredientDict["Помидоры"], Amount = 400 },
                            new RecipeIngredient { Ingredient = ingredientDict["Моцарелла"], Amount = 250 },
                            new RecipeIngredient { Ingredient = ingredientDict["Базилик"], Amount = 30 },
                            new RecipeIngredient { Ingredient = ingredientDict["Масло оливковое"], Amount = 30 },
                            new RecipeIngredient { Ingredient = ingredientDict["Соль"], Amount = 3 },
                            new RecipeIngredient { Ingredient = ingredientDict["Перец черный молотый"], Amount = 2 },
                            new RecipeIngredient { Ingredient = ingredientDict["Уксус бальзамический"], Amount = 15 }
                        }
                    },
                    new Recipe
                    {
                        Name = "Брускетта с лососем и сливочным сыром",
                        Description = "Изысканная закуска с нежным сливочным сыром, слабосоленым лососем и свежим " +
                            "укропом. Идеально для завтрака или легкого перекуса.",
                        CookingTime = 10,
                        Instructions = "Подготовьте все ингредиенты. Лосось достаньте из холодильника за 5-10 минут " +
                            "до подачи, чтобы он немного согрелся и раскрыл вкус.\n\n" +
                            "Хлеб нарежьте ломтиками толщиной около 1 см. Если используете багет или чиабатту, " +
                            "нарезайте по диагонали для большей площади поверхности.\n\n" +
                            "Разогрейте сковороду-гриль или обычную сковороду на среднем огне без масла. Обжарьте " +
                            "хлеб с двух сторон до золотистой хрустящей корочки, примерно по 2-3 минуты с каждой " +
                            "стороны. Хлеб должен стать хрустящим снаружи, но остаться мягким внутри.\n\n" +
                            "Пока хлеб горячий, слегка натрите каждый ломтик зубчиком чеснока (по желанию). Это " +
                            "придаст тонкий чесночный аромат.\n\n" +
                            "Сливочный сыр выложите в миску и немного взбейте вилкой, чтобы он стал более воздушным " +
                            "и легче намазывался. Если сыр слишком густой, добавьте чайную ложку сметаны или сливок.\n\n" +
                            "На каждый ломтик поджаренного хлеба равномерно распределите сливочный сыр слоем около " +
                            "5 мм. Не экономьте сыр - он создает важную текстуру и вкус!\n\n" +
                            "Копченый лосось аккуратно разложите поверх сыра. Можно выложить ровными полосками или " +
                            "создать объемные \"розочки\" из ломтиков рыбы для более эффектной подачи.\n\n" +
                            "Свежий укроп мелко порубите и посыпьте брускетты. Добавьте несколько каперсов для " +
                            "пикантной кислинки.\n\n" +
                            "Украсьте каждую брускетту тонким ломтиком лимона или долькой. Лимон не только украшает, " +
                            "но и помогает сбалансировать жирность лосося.\n\n" +
                            "Подавайте немедленно, пока хлеб еще теплый и хрустящий.",
                        ImagePath = "/uploads/recipes/salmon-bruschetta.jpg",
                        UserId = userId,
                        RecipeIngredients = new List<RecipeIngredient>
                        {
                            new RecipeIngredient { Ingredient = ingredientDict["Хлеб ржаной"], Amount = 300 },
                            new RecipeIngredient { Ingredient = ingredientDict["Лосось копченый"], Amount = 200 },
                            new RecipeIngredient { Ingredient = ingredientDict["Сыр сливочный"], Amount = 150 },
                            new RecipeIngredient { Ingredient = ingredientDict["Укроп"], Amount = 20 },
                            new RecipeIngredient { Ingredient = ingredientDict["Лимон"], Amount = 50 },
                            new RecipeIngredient { Ingredient = ingredientDict["Каперсы"], Amount = 30 },
                            new RecipeIngredient { Ingredient = ingredientDict["Чеснок"], Amount = 5 }
                        }
                    },
                    new Recipe
                    {
                        Name = "Курица терияки с рисом",
                        Description = "Сочные кусочки куриного филе в густом глянцевом соусе терияки, поданные на " +
                            "рассыпчатом рисе. Быстрое и невероятно вкусное блюдо в японском стиле.",
                        CookingTime = 40,
                        Instructions = "Рис тщательно промойте в нескольких водах, пока вода не станет прозрачной. " +
                            "Залейте холодной водой в соотношении 1:2, доведите до кипения, убавьте огонь до минимума, " +
                            "накройте крышкой и варите 15-20 минут до полного впитывания воды. Снимите с огня и дайте " +
                            "настояться под крышкой еще 10 минут.\n\n" +
                            "Куриное филе промойте, обсушите бумажным полотенцем и нарежьте небольшими кубиками или " +
                            "полосками примерно по 2-3 сантиметра.\n\n" +
                            "Приготовьте соус терияки. В небольшой миске смешайте соевый соус, мед, сахар, мелко " +
                            "натертый на мелкой терке свежий имбирь и пропущенный через пресс чеснок. Хорошо перемешайте " +
                            "до полного растворения сахара и меда.\n\n" +
                            "В отдельной чашке разведите крахмал в 50 мл холодной воды до однородности без комочков. " +
                            "Это понадобится для загустения соуса в конце готовки.\n\n" +
                            "Разогрейте сковороду или вок на сильном огне, добавьте подсолнечное масло. Выложите " +
                            "кусочки курицы в один слой и обжаривайте 5-7 минут, периодически помешивая, до появления " +
                            "золотистой корочки со всех сторон. При необходимости готовьте партиями, чтобы курица именно " +
                            "жарилась, а не тушилась.\n\n" +
                            "Убавьте огонь до среднего, влейте подготовленный соус терияки в сковороду с курицей. Перемешайте " +
                            "и готовьте 3-4 минуты, пока соус не начнет пузыриться и слегка не загустеет.\n\n" +
                            "Тонкой струйкой влейте разведенный крахмал, постоянно помешивая курицу. Соус на глазах станет " +
                            "густым и глянцевым, равномерно обволакивая каждый кусочек. Готовьте еще 1-2 минуты.\n\n" +
                            "Рис аккуратно взрыхлите вилкой, выложите на тарелки или в глубокие пиалы. Сверху распределите " +
                            "курицу в соусе терияки, полейте оставшимся соусом со сковороды.\n\n" +
                            "Посыпьте блюдо семенами кунжута и мелко нарезанным зеленым луком. Подавайте немедленно, пока " +
                            "курица горячая, а соус блестит.",
                        ImagePath = "/uploads/recipes/chicken-teriyaki.jpg",
                        UserId = userId,
                        RecipeIngredients = new List<RecipeIngredient>
                        {
                            new RecipeIngredient { Ingredient = ingredientDict["Куриное филе"], Amount = 500 },
                            new RecipeIngredient { Ingredient = ingredientDict["Рис"], Amount = 200 },
                            new RecipeIngredient { Ingredient = ingredientDict["Соевый соус"], Amount = 80 },
                            new RecipeIngredient { Ingredient = ingredientDict["Сахар"], Amount = 40 },
                            new RecipeIngredient { Ingredient = ingredientDict["Чеснок"], Amount = 15 },
                            new RecipeIngredient { Ingredient = ingredientDict["Крахмал"], Amount = 15 },
                            new RecipeIngredient { Ingredient = ingredientDict["Вода"], Amount = 1000 },
                            new RecipeIngredient { Ingredient = ingredientDict["Зеленый лук"], Amount = 20 },
                            new RecipeIngredient { Ingredient = ingredientDict["Масло подсолнечное"], Amount = 30 },
                            new RecipeIngredient { Ingredient = ingredientDict["Соль"], Amount = 3 },
                            new RecipeIngredient { Ingredient = ingredientDict["Кунжут"], Amount = 10 },
                            new RecipeIngredient { Ingredient = ingredientDict["Имбирь свежий"], Amount = 10 },
                            new RecipeIngredient { Ingredient = ingredientDict["Мед"], Amount = 30 }
                        }
                    },
                    new Recipe
                    {
                        Name = "Чизкейк с черничным топпингом",
                        Description = "Нежный сливочный чизкейк на хрустящей песочной основе с ярким черничным " +
                            "соусом. Классический американский десерт, который покорит любое сердце.",
                        CookingTime = 90,
                        Instructions = "Разогрейте духовку до 160°C. Форму для выпечки (диаметром 20-22 см) смажьте " +
                            "маслом и застелите дно пергаментом.\n\n" +
                            "Приготовьте основу для чизкейка. В миске смешайте измельченное в крошку песочное печенье " +
                            "(или муку с сахаром и маслом) до состояния влажного песка. Выложите массу на дно формы и " +
                            "плотно утрамбуйте, формируя бортики высотой около 2 см. Уберите в холодильник на 15 минут.\n\n" +
                            "В большой миске взбейте сливочный сыр комнатной температуры с сахаром и ванилином до гладкой " +
                            "однородной массы. Важно не взбивать слишком интенсивно, чтобы не насыщать смесь воздухом - " +
                            "это может привести к трещинам на поверхности чизкейка.\n\n" +
                            "По одному добавляйте яйца, каждый раз аккуратно вмешивая их лопаткой или миксером на низкой " +
                            "скорости. В конце добавьте сок половины лимона для легкой кислинки.\n\n" +
                            "Вылейте сырную начинку на охлажденную основу, разровняйте поверхность лопаткой. Выпекайте в " +
                            "духовке 50-60 минут. Чизкейк готов, когда края схватились, а центр слегка подрагивает при " +
                            "легком встряхивании формы.\n\n" +
                            "Выключите духовку, приоткройте дверцу и оставьте чизкейк остывать внутри еще на час. Это " +
                            "предотвратит резкий перепад температур и появление трещин.\n\n" +
                            "Переложите чизкейк в холодильник минимум на 4 часа, а лучше на ночь. Он должен полностью застыть.\n\n" +
                            "Приготовьте черничный топпинг. В сотейнике смешайте чернику с сахаром и лимонным соком. " +
                            "Доведите до кипения на среднем огне, варите 5-7 минут, пока ягоды не начнут лопаться.\n\n" +
                            "В небольшой чашке разведите крахмал в 2 столовых ложках холодной воды. Тонкой струйкой влейте " +
                            "в кипящую чернику, постоянно помешивая. Варите еще 1-2 минуты, пока соус не загустеет и не " +
                            "станет блестящим.\n\n" +
                            "Остудите топпинг до комнатной температуры. Перед подачей аккуратно распределите черничный " +
                            "соус по поверхности чизкейка. Нарежьте горячим ножом (ополосните его в кипятке перед " +
                            "каждым разрезом) и подавайте.",
                        ImagePath = "/uploads/recipes/blueberry-cheesecake.jpg",
                        UserId = userId,
                        RecipeIngredients = new List<RecipeIngredient>
                        {
                            new RecipeIngredient { Ingredient = ingredientDict["Мука пшеничная"], Amount = 200 },
                            new RecipeIngredient { Ingredient = ingredientDict["Сахар"], Amount = 150 },
                            new RecipeIngredient { Ingredient = ingredientDict["Масло сливочное"], Amount = 100 },
                            new RecipeIngredient { Ingredient = ingredientDict["Сыр сливочный"], Amount = 600 },
                            new RecipeIngredient { Ingredient = ingredientDict["Яйца"], Amount = 3 },
                            new RecipeIngredient { Ingredient = ingredientDict["Ванилин"], Amount = 10 },
                            new RecipeIngredient { Ingredient = ingredientDict["Лимон"], Amount = 180 },
                            new RecipeIngredient { Ingredient = ingredientDict["Черника"], Amount = 300 },
                            new RecipeIngredient { Ingredient = ingredientDict["Крахмал"], Amount = 15 },
                            new RecipeIngredient { Ingredient = ingredientDict["Соль"], Amount = 2 }
                        }
                    },
                    new Recipe
                    {
                        Name = "Ягодный смузи с бананом",
                        Description = "Густой, насыщенный и полезный смузи из свежих ягод и спелого банана. Идеальный " +
                            "завтрак или перекус, который заряжает энергией и витаминами на весь день.",
                        CookingTime = 5,
                        Instructions = "Подготовьте все ингредиенты. Банан очистите от кожуры и нарежьте крупными " +
                            "кусками. Если ягоды замороженные, дайте им постоять при комнатной температуре 5-10 минут, " +
                            "чтобы они слегка оттаяли.\n\n" +
                            "В чашу блендера выложите нарезанный банан, свежую малину и чернику. Банан придает смузи " +
                            "естественную сладость и кремовую текстуру, поэтому сахар можно не добавлять.\n\n" +
                            "Влейте холодный кефир. Он сделает напиток более нежным и добавит полезные пробиотики. " +
                            "Если хотите более жидкую консистенцию, можно использовать молоко вместо кефира.\n\n" +
                            "Добавьте мед для дополнительной сладости. Если банан очень спелый и сладкий, количество " +
                            "меда можно уменьшить по вкусу.\n\n" +
                            "Закройте крышку блендера и взбивайте на максимальной скорости 1-2 минуты до получения " +
                            "однородной гладкой массы без комочков.\n\n" +
                            "Попробуйте смузи на вкус. Если нужно, добавьте еще немного меда или ягод и взбейте еще раз.\n\n" +
                            "Перелейте готовый смузи в высокий стакан. Подавайте немедленно, пока напиток холодный " +
                            "и свежий. Можно украсить сверху несколькими целыми ягодами или долькой банана.",
                        ImagePath = "/uploads/recipes/berry-smoothie.jpg",
                        UserId = userId,
                        RecipeIngredients = new List<RecipeIngredient>
                        {
                            new RecipeIngredient { Ingredient = ingredientDict["Малина"], Amount = 150 },
                            new RecipeIngredient { Ingredient = ingredientDict["Черника"], Amount = 100 },
                            new RecipeIngredient { Ingredient = ingredientDict["Банан"], Amount = 150 },
                            new RecipeIngredient { Ingredient = ingredientDict["Кефир"], Amount = 200 },
                            new RecipeIngredient { Ingredient = ingredientDict["Мед"], Amount = 20 }
                        }
                    },
                    new Recipe
                    {
                        Name = "Блины с бананами и клубникой",
                        Description = "Нежные тонкие блинчики со свежими бананами, клубникой и шоколадной глазурью. " +
                            "Идеальный завтрак или десерт для особого случая.",
                        CookingTime = 40,
                        Instructions = "В глубокой миске смешайте муку, сахар и соль. Сделайте углубление в центре.\n\n" +
                            "В отдельной посуде взбейте яйца с молоком до однородности. Постепенно влейте " +
                            "яично-молочную смесь в муку, постоянно помешивая венчиком, чтобы не было комков.\n\n" +
                            "Добавьте растопленное сливочное масло в тесто и хорошо перемешайте. Тесто должно " +
                            "получиться жидким, как сливки. Если слишком густое - добавьте немного молока.\n\n" +
                            "Разогрейте блинную сковороду или обычную сковороду с антипригарным покрытием на среднем " +
                            "огне. Слегка смажьте маслом (только для первого блина).\n\n" +
                            "Налейте половник теста в центр сковороды и быстрым круговым движением распределите его " +
                            "тонким слоем по всей поверхности.\n\n" +
                            "Жарьте блин 1-2 минуты до золотистого цвета снизу. Переверните лопаткой и жарьте еще " +
                            "30-60 секунд с другой стороны.\n\n" +
                            "Повторите с остальным тестом. У вас должно получиться 8-10 тонких блинчиков.\n\n" +
                            "Пока блины еще теплые, приготовьте начинку. Бананы нарежьте тонкими кружочками. " +
                            "Клубнику помойте, удалите хвостики и нарежьте пластинками.\n\n" +
                            "На каждый блин выложите несколько кружочков банана и пластинок клубники. Сложите " +
                            "блин конвертиком или сверните трубочкой.\n\n" +
                            "Растопите темный шоколад на водяной бане или в микроволновке (импульсами по 15 секунд, " +
                            "помешивая).\n\n" +
                            "Полейте готовые блины растопленным шоколадом. Подавайте сразу, пока блины еще теплые.",
                        ImagePath = "/uploads/recipes/crepes-banana-strawberry.jpg",
                        UserId = userId,
                        RecipeIngredients = new List<RecipeIngredient>
                        {
                            new RecipeIngredient { Ingredient = ingredientDict["Мука пшеничная"], Amount = 200 },
                            new RecipeIngredient { Ingredient = ingredientDict["Молоко"], Amount = 500 },
                            new RecipeIngredient { Ingredient = ingredientDict["Яйца"], Amount = 3 },
                            new RecipeIngredient { Ingredient = ingredientDict["Сахар"], Amount = 40 },
                            new RecipeIngredient { Ingredient = ingredientDict["Соль"], Amount = 3 },
                            new RecipeIngredient { Ingredient = ingredientDict["Масло сливочное"], Amount = 40 },
                            new RecipeIngredient { Ingredient = ingredientDict["Банан"], Amount = 2 },
                            new RecipeIngredient { Ingredient = ingredientDict["Клубника"], Amount = 200 },
                            new RecipeIngredient { Ingredient = ingredientDict["Шоколад темный"], Amount = 50 }
                        }
                    },
                    new Recipe
                    {
                        Name = "Горячие тосты с сыром и вялеными томатами",
                        Description = "Простая, но невероятно вкусная закуска из хрустящего ржаного хлеба с тягучим " +
                            "расплавленным сыром и ароматными вялеными томатами. Готовится за 10 минут и идеально " +
                            "подходит для завтрака или легкого перекуса.",
                        CookingTime = 15,
                        Instructions = "Духовку разогрейте до 200°C. Если есть функция гриля - используйте ее, это " +
                            "даст красивую золотистую корочку на сыре.\n\n" +
                            "Ржаной хлеб нарежьте ломтиками толщиной около 1 сантиметра. Если хлеб уже нарезан - " +
                            "просто выложите ломтики на противень, застеленный пергаментной бумагой, или на решетку духовки.\n\n" +
                            "Каждый ломтик хлеба слегка смажьте мягким сливочным маслом с одной стороны. Это придаст " +
                            "хлебу дополнительный аромат и поможет ему стать еще более хрустящим при запекании.\n\n" +
                            "Сыр твердых сортов натрите на крупной терке. Вяленые томаты нарежьте небольшими кусочками - " +
                            "примерно по 3-4 миллиметра. Если томаты в масле, слегка обсушите их бумажным полотенцем, " +
                            "чтобы убрать лишнюю жидкость.\n\n" +
                            "На каждый ломтик хлеба выложите слой натертого сыра, равномерно распределяя его по всей " +
                            "поверхности. Сверху посыпьте кусочками вяленых томатов. По желанию можно добавить щепотку " +
                            "черного перца или сушеного орегано для дополнительного аромата.\n\n" +
                            "Противень с подготовленными тостами поставьте в разогретую духовку на 5-7 минут. Сыр должен " +
                            "полностью расплавиться и начать слегка пузыриться, а края хлеба - стать золотистыми и " +
                            "хрустящими. Если используете режим гриля - следите внимательно, сыр может подгореть за " +
                            "считанные секунды.\n\n" +
                            "Готовые тосты достаньте из духовки и дайте им остыть буквально 1-2 минуты - сыр немного " +
                            "схватится и не будет стекать при первом укусе. Подавайте горячими, сразу после приготовления, " +
                            "с чашкой ароматного чая или кофе.",
                        ImagePath = "/uploads/recipes/cheese-toasts.jpg",
                        UserId = userId,
                        RecipeIngredients = new List<RecipeIngredient>
                        {
                            new RecipeIngredient { Ingredient = ingredientDict["Хлеб ржаной"], Amount = 350 },
                            new RecipeIngredient { Ingredient = ingredientDict["Сыр твердый"], Amount = 150 },
                            new RecipeIngredient { Ingredient = ingredientDict["Вяленые томаты"], Amount = 60 },
                            new RecipeIngredient { Ingredient = ingredientDict["Масло сливочное"], Amount = 20 },
                            new RecipeIngredient { Ingredient = ingredientDict["Перец черный молотый"], Amount = 2 }
                        }
                    },
                    new Recipe
                    {
                        Name = "Овсянка с черникой и медом",
                        Description = "Полезный и невероятно вкусный завтрак с нежной овсяной кашей, ароматным " +
                            "черничным соусом и свежими ягодами. Готовится за 15 минут и заряжает энергией на всё утро.",
                        CookingTime = 15,
                        Instructions = "В небольшой кастрюле смешайте овсяные хлопья, молоко и щепотку соли. " +
                            "Поставьте на средний огонь и доведите до кипения, периодически помешивая деревянной " +
                            "ложкой, чтобы каша не пригорела ко дну.\n\n" +
                            "После закипания убавьте огонь до минимума и варите овсянку 5-7 минут, продолжая " +
                            "помешивать. Каша должна загустеть и стать кремообразной, но при этом оставаться нежной. " +
                            "Если хотите более жидкую консистенцию, добавьте еще немного молока.\n\n" +
                            "Пока варится овсянка, приготовьте черничный соус. В маленькой сковороде или сотейнике " +
                            "разогрейте половину черники (около 70 г) на среднем огне. Добавьте столовую ложку меда " +
                            "и немного воды (2-3 столовые ложки), чтобы ягоды не пригорели.\n\n" +
                            "Разминайте ягоды ложкой прямо в сковороде, пока они не превратятся в густой ароматный " +
                            "соус. Это займет примерно 3-4 минуты. Соус должен стать темно-фиолетовым и слегка " +
                            "загустеть. Снимите с огня и дайте немного остыть.\n\n" +
                            "Готовую овсянку разложите по глубоким тарелкам или кружкам с ручками, как на фото. " +
                            "Сверху щедро полейте теплым черничным соусом, позволяя ему красиво стечь по краям каши.\n\n" +
                            "Украсьте блюдо оставшейся свежей черникой, разложив ягоды поверх соуса. По желанию " +
                            "добавьте еще одну чайную ложку меда, полив им ягоды тонкой струйкой.\n\n" +
                            "В завершение посыпьте овсянку кунжутом - он добавит приятный ореховый аромат и " +
                            "интересный хруст. Подавайте немедленно, пока каша горячая, а соус еще теплый. Отлично " +
                            "сочетается со стаканом холодного молока или свежезаваренным чаем.",
                        ImagePath = "/uploads/recipes/oatmeal-blueberry.jpg",
                        UserId = userId,
                        RecipeIngredients = new List<RecipeIngredient>
                        {
                            new RecipeIngredient { Ingredient = ingredientDict["Овсянка"], Amount = 80 },
                            new RecipeIngredient { Ingredient = ingredientDict["Молоко"], Amount = 200 },
                            new RecipeIngredient { Ingredient = ingredientDict["Черника"], Amount = 150 },
                            new RecipeIngredient { Ingredient = ingredientDict["Мед"], Amount = 30 },
                            new RecipeIngredient { Ingredient = ingredientDict["Соль"], Amount = 2 },
                            new RecipeIngredient { Ingredient = ingredientDict["Кунжут"], Amount = 5 },
                            new RecipeIngredient { Ingredient = ingredientDict["Вода"], Amount = 50 }
                        }
                    }
                };

                context.Recipes.AddRange(recipes);
                context.SaveChanges();
            }
        }
    }
}