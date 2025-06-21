using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaterProducts.Migrations
{
    /// <inheritdoc />
    public partial class favourites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "favourites",
                columns: table => new
                {
                    productId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_favourites", x => new { x.productId, x.userId });
                    table.ForeignKey(
                        name: "FK_favourites_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_favourites_userId",
                table: "favourites",
                column: "userId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "favourites");
        }
    }
}
