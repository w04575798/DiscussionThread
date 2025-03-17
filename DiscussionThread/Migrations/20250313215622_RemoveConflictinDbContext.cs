using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscussionThread.Migrations
{
    /// <inheritdoc />
    public partial class RemoveConflictinDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Discussions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Discussions_ApplicationUserId",
                table: "Discussions",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Discussions_AspNetUsers_ApplicationUserId",
                table: "Discussions",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discussions_AspNetUsers_ApplicationUserId",
                table: "Discussions");

            migrationBuilder.DropIndex(
                name: "IX_Discussions_ApplicationUserId",
                table: "Discussions");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Discussions");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
