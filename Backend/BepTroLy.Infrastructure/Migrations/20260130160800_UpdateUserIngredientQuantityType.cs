using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BepTroLy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserIngredientQuantityType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "UserIngredients",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<int>(
                name: "IngredientId1",
                table: "UserIngredients",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnitId1",
                table: "UserIngredients",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "UserIngredients",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId1",
                table: "Ingredients",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserIngredients_IngredientId1",
                table: "UserIngredients",
                column: "IngredientId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserIngredients_UnitId1",
                table: "UserIngredients",
                column: "UnitId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserIngredients_UserId1",
                table: "UserIngredients",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_CategoryId1",
                table: "Ingredients",
                column: "CategoryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_IngredientCategories_CategoryId1",
                table: "Ingredients",
                column: "CategoryId1",
                principalTable: "IngredientCategories",
                principalColumn: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserIngredients_Ingredients_IngredientId1",
                table: "UserIngredients",
                column: "IngredientId1",
                principalTable: "Ingredients",
                principalColumn: "IngredientId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserIngredients_Units_UnitId1",
                table: "UserIngredients",
                column: "UnitId1",
                principalTable: "Units",
                principalColumn: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserIngredients_Users_UserId1",
                table: "UserIngredients",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_IngredientCategories_CategoryId1",
                table: "Ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_UserIngredients_Ingredients_IngredientId1",
                table: "UserIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_UserIngredients_Units_UnitId1",
                table: "UserIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_UserIngredients_Users_UserId1",
                table: "UserIngredients");

            migrationBuilder.DropIndex(
                name: "IX_UserIngredients_IngredientId1",
                table: "UserIngredients");

            migrationBuilder.DropIndex(
                name: "IX_UserIngredients_UnitId1",
                table: "UserIngredients");

            migrationBuilder.DropIndex(
                name: "IX_UserIngredients_UserId1",
                table: "UserIngredients");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_CategoryId1",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "IngredientId1",
                table: "UserIngredients");

            migrationBuilder.DropColumn(
                name: "UnitId1",
                table: "UserIngredients");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "UserIngredients");

            migrationBuilder.DropColumn(
                name: "CategoryId1",
                table: "Ingredients");

            migrationBuilder.AlterColumn<double>(
                name: "Quantity",
                table: "UserIngredients",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
