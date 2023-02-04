using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilyPlanner.api.Migrations
{
    /// <inheritdoc />
    public partial class STORY5UpdatedMealObject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RecipeUri",
                table: "Meals",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Meals",
                keyColumn: "RecipeUri",
                keyValue: null,
                column: "RecipeUri",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "RecipeUri",
                table: "Meals",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
