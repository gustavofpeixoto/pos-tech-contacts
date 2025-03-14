﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PosTech.Contacts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "varchar(200)", nullable: true),
                    Surname = table.Column<string>(type: "varchar(500)", nullable: true),
                    Email = table.Column<string>(type: "varchar(500)", nullable: true),
                    Phone = table.Column<string>(type: "varchar(25)", nullable: true),
                    Ddd = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contacts");
        }
    }
}
