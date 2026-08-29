using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedNewTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    reservationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    reservationStatus = table.Column<string>(type: "text", nullable: false),
                    holdExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    bookId = table.Column<Guid>(type: "uuid", nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.id);
                    table.ForeignKey(
                        name: "FK_Reservations_Books_bookId",
                        column: x => x.bookId,
                        principalTable: "Books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservations_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_bookId",
                table: "Reservations",
                column: "bookId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_MemberId",
                table: "Reservations",
                column: "MemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservations");
        }
    }
}
