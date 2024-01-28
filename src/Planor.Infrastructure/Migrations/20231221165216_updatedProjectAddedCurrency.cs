using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatedProjectAddedCurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "currency_id",
                table: "projects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_projects_currency_id",
                table: "projects",
                column: "currency_id");

            migrationBuilder.AddForeignKey(
                name: "FK_projects_currencies_currency_id",
                table: "projects",
                column: "currency_id",
                principalTable: "currencies",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_projects_currencies_currency_id",
                table: "projects");

            migrationBuilder.DropIndex(
                name: "IX_projects_currency_id",
                table: "projects");

            migrationBuilder.DropColumn(
                name: "currency_id",
                table: "projects");
        }
    }
}
