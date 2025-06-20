using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class attandance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrainerTimeSlotAttendances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainerTimeSlotId = table.Column<int>(type: "int", nullable: false),
                    PunchInTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PunchOutTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainerTimeSlotAttendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainerTimeSlotAttendances_TrainerTimeSlots_TrainerTimeSlotId",
                        column: x => x.TrainerTimeSlotId,
                        principalTable: "TrainerTimeSlots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainerTimeSlotAttendances_TrainerTimeSlotId",
                table: "TrainerTimeSlotAttendances",
                column: "TrainerTimeSlotId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainerTimeSlotAttendances");
        }
    }
}
