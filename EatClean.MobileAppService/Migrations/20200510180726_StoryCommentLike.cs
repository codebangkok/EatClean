using Microsoft.EntityFrameworkCore.Migrations;

namespace EatClean.MobileAppService.Migrations
{
    public partial class StoryCommentLike : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StoryId",
                table: "Likes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StoryId",
                table: "Comments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Likes_StoryId",
                table: "Likes",
                column: "StoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_StoryId",
                table: "Comments",
                column: "StoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Stories_StoryId",
                table: "Comments",
                column: "StoryId",
                principalTable: "Stories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Stories_StoryId",
                table: "Likes",
                column: "StoryId",
                principalTable: "Stories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Stories_StoryId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Stories_StoryId",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Likes_StoryId",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Comments_StoryId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "StoryId",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "StoryId",
                table: "Comments");
        }
    }
}
