using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedHelpersToDuty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DutyUser",
                columns: table => new
                {
                    DutyId = table.Column<int>(type: "int", nullable: false),
                    HelpersId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DutyUser", x => new { x.DutyId, x.HelpersId });
                    table.ForeignKey(
                        name: "FK_DutyUser_AspNetUsers_HelpersId",
                        column: x => x.HelpersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DutyUser_duties_DutyId",
                        column: x => x.DutyId,
                        principalTable: "duties",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DutyUser_HelpersId",
                table: "DutyUser",
                column: "HelpersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DutyUser");
        }
    }
}
