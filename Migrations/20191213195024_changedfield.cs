using Microsoft.EntityFrameworkCore.Migrations;

namespace ActivityCenter.Migrations
{
    public partial class changedfield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "plans",
                newName: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_plans_UserId",
                table: "plans",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_plans_users_UserId",
                table: "plans",
                column: "UserId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_plans_users_UserId",
                table: "plans");

            migrationBuilder.DropIndex(
                name: "IX_plans_UserId",
                table: "plans");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "plans",
                newName: "CreatorId");
        }
    }
}
