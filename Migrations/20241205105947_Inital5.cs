using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarkDownTakingAPI.Migrations
{
    /// <inheritdoc />
    public partial class Inital5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "MDDatas",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "FileSize",
                table: "MDDatas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "MDDatas");

            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "MDDatas");
        }
    }
}
