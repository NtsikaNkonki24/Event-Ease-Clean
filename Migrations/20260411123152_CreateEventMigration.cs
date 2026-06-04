using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Event_Ease_2026_Ntsika_Nkonki.Migrations
{
    /// <inheritdoc />
    public partial class CreateEventMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Bookings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
