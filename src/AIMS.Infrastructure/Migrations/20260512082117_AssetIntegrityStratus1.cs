using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AssetIntegrityStratus1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IntegrityStatus",
                table: "AssetItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IntegrityStatus",
                table: "AssetItems");
        }
    }
}
