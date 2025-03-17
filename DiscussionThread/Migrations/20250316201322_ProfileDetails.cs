using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscussionThread.Migrations
{
    /// <inheritdoc />
    public partial class ProfileDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "Discussions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Discussions_ApplicationUserId1",
                table: "Discussions",
                column: "ApplicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Discussions_AspNetUsers_ApplicationUserId1",
                table: "Discussions",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discussions_AspNetUsers_ApplicationUserId1",
                table: "Discussions");

            migrationBuilder.DropIndex(
                name: "IX_Discussions_ApplicationUserId1",
                table: "Discussions");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "Discussions");
        }
    }
}
