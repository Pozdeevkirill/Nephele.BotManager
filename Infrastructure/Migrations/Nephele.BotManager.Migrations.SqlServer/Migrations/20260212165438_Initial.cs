using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nephele.BotManager.Migrations.SqlServer.Migrations
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
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(300)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BotOwner_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Version = table.Column<DateTime>(type: "datetime2", rowVersion: true, nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BotInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BotOwner",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(300)", nullable: false),
                    Endpoint = table.Column<string>(type: "nvarchar(1000)", nullable: false),
                    Version = table.Column<DateTime>(type: "datetime2", rowVersion: true, nullable: false, defaultValueSql: "getdate()")
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
