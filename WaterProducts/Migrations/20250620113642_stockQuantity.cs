using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaterProducts.Migrations
{
    /// <inheritdoc />
    public partial class stockQuantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "stockQuantiy",
                table: "products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "stockQuantiy",
                table: "products");
        }
    }
}
