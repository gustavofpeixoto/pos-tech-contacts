using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PosTech.Contacts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRerionTableNameToRegions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ddd",
                table: "Contacts");

            migrationBuilder.AddColumn<Guid>(
                name: "DddId",
                table: "Contacts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegionName = table.Column<string>(type: "varchar(500)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ddds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DddCode = table.Column<int>(type: "int", nullable: false),
                    RegionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ddds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ddds_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_DddId",
                table: "Contacts",
                column: "DddId");

            migrationBuilder.CreateIndex(
                name: "IX_Ddds_RegionId",
                table: "Ddds",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Ddds_DddId",
                table: "Contacts",
                column: "DddId",
                principalTable: "Ddds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Ddds_DddId",
                table: "Contacts");

            migrationBuilder.DropTable(
                name: "Ddds");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_DddId",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "DddId",
                table: "Contacts");

            migrationBuilder.AddColumn<int>(
                name: "Ddd",
                table: "Contacts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
