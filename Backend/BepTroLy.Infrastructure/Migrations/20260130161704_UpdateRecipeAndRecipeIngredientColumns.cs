using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BepTroLy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRecipeAndRecipeIngredientColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequiredQuantity",
                table: "RecipeIngredients");

            migrationBuilder.RenameColumn(
                name: "CookingTime",
                table: "Recipes",
                newName: "CategoryId1");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "RecipeSteps",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "RecipeSteps",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RecipeId1",
                table: "RecipeSteps",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Recipes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CookTimeMinutes",
                table: "Recipes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PrepTimeMinutes",
                table: "Recipes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Servings",
                table: "Recipes",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "RecipeIngredients",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "IngredientId1",
                table: "RecipeIngredients",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Quantity",
                table: "RecipeIngredients",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "RecipeId1",
                table: "RecipeIngredients",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnitId1",
                table: "RecipeIngredients",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeSteps_RecipeId1",
                table: "RecipeSteps",
                column: "RecipeId1");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_CategoryId1",
                table: "Recipes",
                column: "CategoryId1");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_IngredientId1",
                table: "RecipeIngredients",
                column: "IngredientId1");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_RecipeId1",
                table: "RecipeIngredients",
                column: "RecipeId1");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_UnitId1",
                table: "RecipeIngredients",
                column: "UnitId1");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_Ingredients_IngredientId1",
                table: "RecipeIngredients",
                column: "IngredientId1",
                principalTable: "Ingredients",
                principalColumn: "IngredientId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_Recipes_RecipeId1",
                table: "RecipeIngredients",
                column: "RecipeId1",
                principalTable: "Recipes",
                principalColumn: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_Units_UnitId1",
                table: "RecipeIngredients",
                column: "UnitId1",
                principalTable: "Units",
                principalColumn: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_RecipeCategories_CategoryId1",
                table: "Recipes",
                column: "CategoryId1",
                principalTable: "RecipeCategories",
                principalColumn: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeSteps_Recipes_RecipeId1",
                table: "RecipeSteps",
                column: "RecipeId1",
                principalTable: "Recipes",
                principalColumn: "RecipeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_Ingredients_IngredientId1",
                table: "RecipeIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_Recipes_RecipeId1",
                table: "RecipeIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_Units_UnitId1",
                table: "RecipeIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_RecipeCategories_CategoryId1",
                table: "Recipes");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeSteps_Recipes_RecipeId1",
                table: "RecipeSteps");

            migrationBuilder.DropIndex(
                name: "IX_RecipeSteps_RecipeId1",
                table: "RecipeSteps");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_CategoryId1",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_RecipeIngredients_IngredientId1",
                table: "RecipeIngredients");

            migrationBuilder.DropIndex(
                name: "IX_RecipeIngredients_RecipeId1",
                table: "RecipeIngredients");

            migrationBuilder.DropIndex(
                name: "IX_RecipeIngredients_UnitId1",
                table: "RecipeIngredients");

            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "RecipeSteps");

            migrationBuilder.DropColumn(
                name: "RecipeId1",
                table: "RecipeSteps");

            migrationBuilder.DropColumn(
                name: "CookTimeMinutes",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "PrepTimeMinutes",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Servings",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "IngredientId1",
                table: "RecipeIngredients");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "RecipeIngredients");

            migrationBuilder.DropColumn(
                name: "RecipeId1",
                table: "RecipeIngredients");

            migrationBuilder.DropColumn(
                name: "UnitId1",
                table: "RecipeIngredients");

            migrationBuilder.RenameColumn(
                name: "CategoryId1",
                table: "Recipes",
                newName: "CookingTime");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "RecipeSteps",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "RecipeIngredients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RequiredQuantity",
                table: "RecipeIngredients",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
