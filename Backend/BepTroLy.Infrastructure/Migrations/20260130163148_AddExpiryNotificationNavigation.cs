using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BepTroLy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExpiryNotificationNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpiryNotifications_UserIngredients_UserIngredientId",
                table: "ExpiryNotifications");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpiryNotifications_UserIngredients_UserIngredientId",
                table: "ExpiryNotifications",
                column: "UserIngredientId",
                principalTable: "UserIngredients",
                principalColumn: "UserIngredientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpiryNotifications_UserIngredients_UserIngredientId",
                table: "ExpiryNotifications");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpiryNotifications_UserIngredients_UserIngredientId",
                table: "ExpiryNotifications",
                column: "UserIngredientId",
                principalTable: "UserIngredients",
                principalColumn: "UserIngredientId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
