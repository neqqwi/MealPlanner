# MealPlanner

A web application for meal planning and automatic shopping list generation. 
It helps users organize their weekly meals plan and aggregates ingredients to 
create a single, optimized shopping list.

**Status:** Work in progress.

## Features

- [ ] **Recipe Management:** Create, read, update, and delete recipes. *(Create/Read/Update done, Delete pending)*
- [x] **Ingredient Dictionary:** Centralized database of products with strict units (g, ml, pcs) to prevent typos and ensure accurate calculations.
- [ ] **Weekly Meal Planner:** Assign recipes to specific days of the week.
- [ ] **Smart Shopping List:** Automatically aggregates required ingredients for the week and subtracts items already available in the virtual "Pantry".
- [ ] **User Authentication:** Secure registration and login, ensuring each user has isolated data.
- [x] **Modern UI:** Responsive design built with Tailwind CSS.

## Tech Stack

- **Backend:** ASP.NET Core 10 (MVC)
- **Database:** PostgreSQL, Entity Framework Core
- **Image Processing:** SixLabors.ImageSharp
- **Frontend:** Razor Views, Tailwind CSS, TomSelect, jQuery Validation
- **Tools:** Git, GitHub

## Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL 12+](https://www.postgresql.org/download/)

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/neqqwi/MealPlanner.git
   cd MealPlanner
   ```

2. Configure database connection in appsettings.json:
   ```json
   {
     "ConnectionStrings": {
       "PostgresConnection": "Host=localhost;Port=5432;Database=MealPlannerDb;Username=postgres;Password=your_password;"
     }
   }
   ```

   Replace your_password with your actual PostgreSQL password.

3. Apply migrations and seed data:
   ```bash
   dotnet ef database update
   ```

   The database will be automatically seeded with 79 ingredients and 17 recipes on first run.

4. Run the application:
   ```bash
   dotnet run
   ```

5. Open your browser. The application will automatically open, or you can navigate to the URL shown in the terminal console (usually http://localhost:XXXX or https://localhost:XXXX).

## Screenshots

Screenshots will be added as the UI is developed.

## Credits

Photos taken from [Unsplash](https://unsplash.com):

- Apple Crumble Pie by [Diliara Garifullina](https://unsplash.com/@dilja96)
- Thai Beef Salad by [Eiliv Aceron](https://unsplash.com/@shootdelicious)
- Chocolate Fondant by [Max Griss](https://unsplash.com/@grissphoto)
- Roasted Chicken by [Cisco Lin](https://unsplash.com/@cok3nosugar)
- Nashville Hot Chicken Burger by [Eiliv Aceron](https://unsplash.com/@shootdelicious)
- BBQ Chicken Pizza by [Chad Montano](https://unsplash.com/@briewilly)
- Fettuccine Alfredo by [Sama Hosseini](https://unsplash.com/@samahosseini)
- Tomato Beef Soup by [Shoeib Abolhassani](https://unsplash.com/@shoeibabhn)
- Beef Steak by [Orkun Orcan](https://unsplash.com/@orkunorcan)
- Caprese by [David Frye](https://unsplash.com/@davidsfocus)
- Salmon Bruschetta by [Bakd&Raw by Karolin Baitinger](https://unsplash.com/@bakdandraw)
- Teriyaki Chicken With Rice by [Zayed Ahmed Zadu](https://unsplash.com/@zayed_ahmed_zadu)
- Blueberry Cheesecake by [Mink Mingle](https://unsplash.com/@minkmingle)
- Berry Banana Smoothie by [Denis](https://unsplash.com/@denis96)
- Banana Strawberry Crepes by [Paolo Cifuentes](https://unsplash.com/@obitokamui)
- Cheese Toasts by [Elena Leya](https://unsplash.com/@foodistika)
- Blueberry Oatmeal by [Elena Leya](https://unsplash.com/@foodistika)