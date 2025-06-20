using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class trinersession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainerTimeSlots_Users_UserId1",
                table: "TrainerTimeSlots");

            migrationBuilder.DropIndex(
                name: "IX_TrainerTimeSlots_UserId1",
                table: "TrainerTimeSlots");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "TrainerTimeSlots");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "TrainerTimeSlots",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainerTimeSlots_UserId1",
                table: "TrainerTimeSlots",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainerTimeSlots_Users_UserId1",
                table: "TrainerTimeSlots",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
