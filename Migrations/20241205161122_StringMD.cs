using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarkDownTakingAPI.Migrations
{
    /// <inheritdoc />
    public partial class StringMD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ContentType",
                table: "MDDatas",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "MDString",
                table: "MDDatas",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MDString",
                table: "MDDatas");

            migrationBuilder.AlterColumn<string>(
                name: "ContentType",
                table: "MDDatas",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
