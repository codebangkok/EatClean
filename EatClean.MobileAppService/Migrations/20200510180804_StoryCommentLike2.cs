using Microsoft.EntityFrameworkCore.Migrations;

namespace EatClean.MobileAppService.Migrations
{
    public partial class StoryCommentLike2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "StoryId",
                table: "Likes",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StoryId",
                table: "Comments",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "StoryId",
                table: "Likes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "StoryId",
                table: "Comments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
