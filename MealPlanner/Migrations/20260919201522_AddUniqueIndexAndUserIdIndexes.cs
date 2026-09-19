using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MealPlanner.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexAndUserIdIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_Name_UserId",
                table: "Ingredients",
                columns: new[] { "Name", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ingredients_Name_UserId",
                table: "Ingredients");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Recipes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Ingredients",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_ApplicationUserId",
                table: "Recipes",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_ApplicationUserId",
                table: "Ingredients",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_Name",
                table: "Ingredients",
                column: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_AspNetUsers_ApplicationUserId",
                table: "Ingredients",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_AspNetUsers_ApplicationUserId",
                table: "Recipes",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
