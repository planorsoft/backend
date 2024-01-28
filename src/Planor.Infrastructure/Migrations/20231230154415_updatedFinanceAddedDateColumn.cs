using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatedFinanceAddedDateColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "date",
                table: "finances",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "date",
                table: "finances");
        }
    }
}
