using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAssetItemPicture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PicturePath",
                table: "AssetItems",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PicturePath",
                table: "AssetItems");
        }
    }
}
