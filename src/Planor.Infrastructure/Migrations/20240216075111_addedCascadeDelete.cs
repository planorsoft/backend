using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_customers_CustomerId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_projects_customers_customer_id",
                table: "projects");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_customers_CustomerId",
                table: "AspNetUsers",
                column: "CustomerId",
                principalTable: "customers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_projects_customers_customer_id",
                table: "projects",
                column: "customer_id",
                principalTable: "customers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_customers_CustomerId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_projects_customers_customer_id",
                table: "projects");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_customers_CustomerId",
                table: "AspNetUsers",
                column: "CustomerId",
                principalTable: "customers",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_projects_customers_customer_id",
                table: "projects",
                column: "customer_id",
                principalTable: "customers",
                principalColumn: "id");
        }
    }
}
