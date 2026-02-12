using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nephele.BotManager.Migrations.PostgreSql.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BotInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(300)", nullable: false),
                    Token = table.Column<string>(type: "character varying(50)", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateStart = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    BotOwner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<DateTime>(type: "timestamp without time zone", rowVersion: true, nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BotInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BotOwner",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(300)", nullable: false),
                    Endpoint = table.Column<string>(type: "character varying(1000)", nullable: false),
                    Version = table.Column<DateTime>(type: "timestamp without time zone", rowVersion: true, nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BotOwner", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BotOwner_BotInfo_Id",
                        column: x => x.Id,
                        principalTable: "BotInfo",
                        principalColumn: "Id");
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BotOwner");

            migrationBuilder.DropTable(
                name: "BotInfo");
        }
    }
}
