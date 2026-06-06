using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalvageCore.Migrations
{
    /// <inheritdoc />
    public partial class AddLoginChallengePurpose : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Purpose",
                table: "TempCustomers",
                type: "integer",
                nullable: false,
                defaultValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Purpose",
                table: "TempCustomers");
        }
    }
}
