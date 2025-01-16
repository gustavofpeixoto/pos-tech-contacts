using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PosTech.Contacts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnStateToDdds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Ddds",
                type: "varchar(10)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "Ddds");
        }
    }
}
